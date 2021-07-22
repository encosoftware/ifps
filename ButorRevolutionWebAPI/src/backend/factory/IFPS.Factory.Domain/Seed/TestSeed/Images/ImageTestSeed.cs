using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class ImageTestSeed : IEntitySeed<Image>
    {
        public Image[] Entities => new[]
        {
            new Image("test.jpg",".jpg","MaterialTestImages") {Id = new Guid("2154e885-c52f-4e70-9408-f919db51fdae"), ThumbnailName = "test_thumbnail.jpg" },
            new Image("qr_test.png",".png","QRCodes") {Id = new Guid("6908447f-6357-49b0-8de1-5be18073e346"), ThumbnailName = "qr_test.png" },
            new Image("profile_icon.jpg",".jpg","ProfilePictures") { Id = new Guid("E183B6AE-3126-4A8B-9FB1-AAD00E3EB1A0"), ThumbnailName = "profile_icon.jpg" },
            new Image("photo-placeholder.jpg",".jpg","Content") {Id = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"), ThumbnailName = "thumbnail_photo-placeholder.jpg" }
        };
    }
}