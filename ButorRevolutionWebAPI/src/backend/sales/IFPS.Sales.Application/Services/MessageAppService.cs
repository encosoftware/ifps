using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Implementations;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class MessageAppService : ApplicationService, IMessageAppService
    {

        private readonly IMessageChannelParticipantRepository messageChannelParticipantRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IParticipantMessageRepository participantMessageRepository;
        private readonly IMessageChannelRepository messageChannelRepository;
        private readonly IUserRepository userRepository;
        private readonly MessageHub hubService;
        private readonly IOrderService orderService;

        public MessageAppService(IApplicationServiceDependencyAggregate aggregate,
            IMessageChannelParticipantRepository messageChannelParticipantRepository,
            IMessageRepository messageRepository,
            IParticipantMessageRepository participantMessageRepository,
            IMessageChannelRepository messageChannelRepository,
            IUserRepository userRepository,
            IOrderService orderService,
            MessageHub hubService)
            : base(aggregate)
        {
            this.messageChannelParticipantRepository = messageChannelParticipantRepository;
            this.messageRepository = messageRepository;
            this.participantMessageRepository = participantMessageRepository;
            this.messageChannelRepository = messageChannelRepository;
            this.userRepository = userRepository;
            this.orderService = orderService;
            this.hubService = hubService;
        }

        public async Task<int> CreateMessageChannelAsync(MessageChannelCreateDto messageChannelCreateDto)
        {
            var messageChannel = messageChannelCreateDto.CreateModelObject();
            messageChannelCreateDto.ParticipantIds
                .ForEach(ent => messageChannel.AddMessageChannelParticipant(new MessageChannelParticipant(messageChannel.Id, ent)));
            await messageChannelRepository.InsertAsync(messageChannel);
            await unitOfWork.SaveChangesAsync();
            return messageChannel.Id;
        }

        public async Task SendMessageAsync(MessageCreateDto messageCreateDto)
        {
            var message = messageCreateDto.CreateModelObject();
            var messageChannelParticipant = await messageChannelRepository.GetMessageChannelParticipantByUserIdAsync(
                messageCreateDto.MessageChannelId, messageCreateDto.SenderId);
            message.SenderId = messageChannelParticipant.Id;
            message.TimeStamp = Clock.Now;
            await messageRepository.InsertAsync(message);

            message.MessageChannel = await messageChannelRepository.GetMessageChannelAsync(messageCreateDto.MessageChannelId);

            foreach (MessageChannelParticipant participant in message.MessageChannel.Participants
                .Where(participant => participant.UserId != messageCreateDto.SenderId))
            {
                ParticipantMessage participantMessage = new ParticipantMessage(message.Id, participant.Id);
                await participantMessageRepository.InsertAsync(participantMessage);
            }

            await unitOfWork.SaveChangesAsync();
            await hubService.SendMessageToGroupAsync(message.MessageChannelId, message.SenderId, message.Text);
        }

        public async Task<MessageChannelDetailsDto> GetMessageChannelAsync(int userId, int id)
        {
            var messageChannel = await messageChannelRepository.GetMessageChannelAsync(id);
            List<MessageChannelParticipant> messageChannelParticipants = await messageChannelParticipantRepository.GetMessageChannelParticipantsAsync(userId);

            foreach (var participantMessages in messageChannelParticipants
                .SelectMany(message => message.ParticipantMessages
                .Where(partmessages => partmessages.Seen == false)
                .Select(partmessages => partmessages)))
            {
                participantMessages.Seen = true;
            }

            await unitOfWork.SaveChangesAsync();

            return new MessageChannelDetailsDto(messageChannel);
        }

        public async Task<OrderMessagesDto> GetMessagesByOrderAndUserListAsync(Guid orderId, int userId)
        {
            var orderMessagesDto = new OrderMessagesDto();

            //get the possible contacts from order
            var contactIds = await orderService.GetOrderAvailableContactIds(orderId);

            //get the current messageChannels
            var messageContactList = await messageChannelParticipantRepository.GetAllListAsync<MessageChannelListDbo>(
                ent => ent.MessageChannel.OrderId == orderId &&
                    ent.MessageChannel.Participants.Select(p => p.UserId).Contains(userId)
                    && ent.UserId != userId,
                MessageChannelListDbo.Projection);

            //if there are any messageChannels (with admins for example) who arent listed via order, add them to contact list
            contactIds.AddRange(messageContactList.SelectMany(ent => ent.ContactUserIds).ToList());

            var contactProfiles = await userRepository.GetAllListAsync<UserAvatarDbo>(
                ent => contactIds.Distinct().Contains(ent.Id) && ent.Id != userId, UserAvatarDbo.Projection);

            var unseenMessages = await messageChannelParticipantRepository.GetAllListAsync<UnseenMessageCountDbo>(
                x => x.UserId == userId && x.MessageChannel.OrderId == orderId, UnseenMessageCountDbo.Projection);

            orderMessagesDto.MessageChannels = messageContactList.Select(ent =>
                new UnansweredMessageListDto(
                    ent, contactProfiles.Single(c => c.Id == ent.ParticipantUserId),
                    unseenMessages.Single(m => m.MessageChannelId == ent.MessageChannelId))).ToList();

            orderMessagesDto.Participants = contactProfiles.Where(ent => !messageContactList.Any(c => c.ParticipantUserId == ent.Id))
                .Select(ent => new MessageChannelParticipantListDto(ent)).ToList();

            return orderMessagesDto;
        }

        public async Task<List<UnansweredMessageListDto>> GetUnansweredMessageListByUserAsync(int userId)
        {
            var participantMessages = await participantMessageRepository.GetParticipantMessagesByUser(userId);
            var unansweredMessages = new Dictionary<ParticipantMessage, int>();
            participantMessages
                .GroupBy(ent => ent.Message.MessageChannel)
                .ToList()
                .ForEach(ent => unansweredMessages.Add(ent.Last(), ent.Count()));
            return unansweredMessages.Select(ent => new UnansweredMessageListDto(ent.Key) { MessageCount = ent.Value }).ToList();
        }

        public async Task<int> GetCountedUnansweredMessagesAsync(int userId)
        {
            return await participantMessageRepository.GetCountedUnansweredMessagesByUser(userId);
        }
    }
}
