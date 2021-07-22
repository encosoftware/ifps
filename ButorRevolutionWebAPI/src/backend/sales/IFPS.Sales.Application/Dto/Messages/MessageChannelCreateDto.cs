using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class MessageChannelCreateDto
    {
        public Guid OrderId { get; set; }
        public List<int> ParticipantIds { get; set; }

        public MessageChannel CreateModelObject()
        {
            return new MessageChannel(OrderId);
        }
    }
}
