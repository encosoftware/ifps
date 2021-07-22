using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class MessageHub : Hub
    {
        protected IHubContext<MessageHub> context;
        private readonly IMessageChannelParticipantRepository messageChannelParticipantRepository;        
        private readonly IIdentityService identityService;
        private readonly UserManager<User> userManager;

        public MessageHub(
            IHubContext<MessageHub> context,
            IMessageChannelParticipantRepository messageChannelParticipantRepository,
            IIdentityService identityService,
            IUserRepository userRepository,
            UserManager<User> userManager)
            {
            this.context = context;
            this.messageChannelParticipantRepository = messageChannelParticipantRepository;
            this.identityService = identityService;
            this.userManager = userManager;
        }

        public async Task SendMessageToGroupAsync(int messageChannelId, int senderId, string message)
        {
            await context.Clients.Group("MessageChannel" + messageChannelId).SendAsync("ReceiveMessage", senderId, message);
        }
   
        public override async Task OnConnectedAsync()
        {
            var userId = await identityService.GetCurrentUserIdAsync();
            //var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);

            //userManager.GetUserId(ClaimsPrincipal.Current.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)));
            List<int> messageChannelId = await messageChannelParticipantRepository.GetMessageChannelIdByuserIdAsync(userId);
            messageChannelId.ForEach(async channelId => await context.Groups.AddToGroupAsync(Context.ConnectionId, "MessageChannel" + channelId));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
