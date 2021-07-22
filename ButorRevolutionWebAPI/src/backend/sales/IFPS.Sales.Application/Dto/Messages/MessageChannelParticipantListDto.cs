using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class MessageChannelParticipantListDto
    {
        public ImageThumbnailListDto Image { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public MessageChannelParticipantListDto(MessageChannelParticipant messageChannelParticipant)
        {
            Image = new ImageThumbnailListDto(messageChannelParticipant.User.Image);
            Name = messageChannelParticipant.User.CurrentVersion.Name;
            UserId = messageChannelParticipant.User.Id;            
        }

        public MessageChannelParticipantListDto(UserAvatarDbo profile)
        {
            Image = new ImageThumbnailListDto(profile.Image);
            Name = profile.Name;
            UserId = profile.Id;
        }
    }
}
