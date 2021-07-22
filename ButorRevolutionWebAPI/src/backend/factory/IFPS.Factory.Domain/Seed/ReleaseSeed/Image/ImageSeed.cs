using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ImageSeed : IEntitySeed<Image>
    {
        public Image[] Entities => new[]
        {
            new Image("worktop.jpg",".jpg","MaterialPictures") {Id = new Guid("515B4AB1-A087-4373-96F1-25E6C058D02C"), ThumbnailName = "worktop_thumbnail.jpg" },
            new Image("test.jpg",".jpg","MaterialTestImages") {Id = new Guid("78731AED-B8B6-4EEF-BACA-2855DCAB25FB"), ThumbnailName = "test_thumbnail.jpg" },
            new Image("qr_test.png",".png","QRCodes") {Id = new Guid("6908447F-6357-49B0-8DE1-5BE18073E346"), ThumbnailName = "qr_test.png" },
            new Image("profile_icon.jpg",".jpg","ProfilePictures") { Id = new Guid("E183B6AE-3126-4A8B-9FB1-AAD00E3EB1A0"), ThumbnailName = "profile_icon.jpg" },
            new Image("photo-placeholder.jpg",".jpg","Content") {Id = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), ThumbnailName = "thumbnail_photo-placeholder.jpg" },
        };
    }
}
