using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IMessageAppService
    {
        Task<int> CreateMessageChannelAsync(MessageChannelCreateDto messageChannelCreateDto);
        Task<List<UnansweredMessageListDto>> GetUnansweredMessageListByUserAsync(int userId);
        Task SendMessageAsync(MessageCreateDto participantMessageCreateDto);
        Task<OrderMessagesDto> GetMessagesByOrderAndUserListAsync(Guid orderId, int userId);
        Task<MessageChannelDetailsDto> GetMessageChannelAsync(int userId, int id);
        Task<int> GetCountedUnansweredMessagesAsync(int userId);
    }
}
