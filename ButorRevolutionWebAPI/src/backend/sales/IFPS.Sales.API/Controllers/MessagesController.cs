using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : IFPSControllerBase
    {
        private const string OPNAME = "Messages";

        private readonly IMessageAppService messageAppService;

        public MessagesController(IMessageAppService messageAppService)
        {
            this.messageAppService = messageAppService;
        }

        // GET unanswered messages by user
        [HttpGet("unanswered")]
        [Authorize(Policy = "GetMessages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<UnansweredMessageListDto>> GetUnansweredMessageList()
        {
            return messageAppService.GetUnansweredMessageListByUserAsync(GetCallerId());
        }

        // POST new message channel
        [HttpPost("channels")]
        [Authorize(Policy = "UpdateMessages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateMessageChannel([FromBody]MessageChannelCreateDto messageChannelCreateDto)
        {
            return messageAppService.CreateMessageChannelAsync(messageChannelCreateDto);
        }

        // POST new message channel
        [HttpPost]
        [Authorize(Policy = "UpdateMessages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SendMessage([FromBody]MessageCreateDto messageCreateDto)
        {
            return messageAppService.SendMessageAsync(messageCreateDto);
        }

        // GET messages by messagechannel
        [HttpGet("channels/{messageChannelId}")]
        [Authorize(Policy = "GetMessages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<MessageChannelDetailsDto> GetMessageChannelList(int messageChannelId)
        {
            return messageAppService.GetMessageChannelAsync(GetCallerId(), messageChannelId);
        }

        // GET messages by user and order
        [HttpGet("{orderId}")]
        //TODO: policy?
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<OrderMessagesDto> GetMessagesByOrderAndUserList(Guid orderId)
        {
            return messageAppService.GetMessagesByOrderAndUserListAsync(orderId, GetCallerId());
        }

        // GET count of unanswered messages by user
        [HttpGet("unanswered/counted")]
        [Authorize(Policy = "GetMessages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> GetCountedUnansweredMessages()
        {
            return messageAppService.GetCountedUnansweredMessagesAsync(GetCallerId());
        }
    }
}