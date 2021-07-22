using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed
{
    public class ImageSeed : IEntitySeed<Image>
    {
        public Image[] Entities => new[]
        {
            new Image("test.jpg",".jpg","MaterialPictures") {Id = new Guid("515B4AB1-A087-4373-96F1-25E6C058D02C"), ThumbnailName = "test_thumbnail.jpg" },
            new Image("photo-placeholder.jpg",".jpg","Content") {Id = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),ThumbnailName = "thumbnail_photo-placeholder.jpg" },
            new Image("zanussi_ZDF22002XA.jpg",".jpg","Content") {Id = new Guid("525955CB-F710-401B-BB30-3318E1CEA414"), ThumbnailName = "thumbnail_zanussi_ZDF22002XA.jpg" },
            new Image("worktop-board-placeholder.jpg",".jpg","Content") {Id = new Guid("15619300-FCF1-4B8C-8A24-3C5A2FD3E513"),ThumbnailName = "thumbnail_worktop-board-placeholder.jpg" },
            new Image("decor-board-placeholder.jpg",".jpg","Content") {Id = new Guid("C1882142-743E-41E5-A2C2-D62BA7A7CBA4"), ThumbnailName = "thumbnail_decor-board-placeholder.jpg" },
            new Image("accessory-placeholder.jpg",".jpg","Content") {Id = new Guid("D647832C-02A2-475E-A154-FB60B5F45D6E"),ThumbnailName = "thumbnail_accessory-placeholder.jpg" },
            new Image("foil-placeholder.jpg",".jpg","Content") {Id = new Guid("07A316ED-48DF-4DCC-919F-170224574CD2"),ThumbnailName = "thumbnail_foil-placeholder.jpg" },
            new Image("furnitureunit-placeholder.png",".png","Content") {Id = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0369"), ThumbnailName = "furnitureunit-placeholder.png" },
            new Image("bathroom-placeholder.jpg",".jpg","Content") {Id = new Guid("3341a5ad-98b0-4e55-8ebe-f34c2439ed15"),ThumbnailName = "bathroom-placeholder.jpg" },
            new Image("kitchen-placeholder.jpg",".jpg","Content") {Id = new Guid("12d668ca-cec4-43fb-9f7b-945fa92bbaac"),ThumbnailName = "kitchen-placeholder.jpg" },
            new Image("livingroom-placeholder.jpg",".jpg","Content") {Id = new Guid("703d5ae3-c1bc-43dd-bc46-9dd549ce2a7e"),ThumbnailName = "livingroom-placeholder.jpg" },
            new Image("office-placeholder.jpg",".jpg","Content") {Id = new Guid("da4532c2-43fd-420f-b93a-c11afc44e9e5"),ThumbnailName = "office-placeholder.jpg" },
            new Image("kitchen_island.jpg",".jpg","Content") {Id = new Guid("7cf3d233-da47-4f58-978e-bab8b9c4b118"),ThumbnailName = "kitchen_island.jpg" },
            new Image("kitchen_handles.jpg",".jpg","Content") {Id = new Guid("95537a5a-11d7-4cf7-8c05-73e4a34f3097"),ThumbnailName = "kitchen_handles.jpg" },
            new Image("kitchen_bottom.jpg",".jpg","Content") {Id = new Guid("371fcae6-5196-4c35-818b-c9f5fafb09c9"),ThumbnailName = "kitchen_bottom.jpg" },
            new Image("kitchen_lights.jpg",".jpg","Content") {Id = new Guid("bc71cbf8-edb7-4ec4-8675-108e146ff40b"),ThumbnailName = "kitchen_lights.jpg" },
            new Image("kitchen_tall.jpg",".jpg","Content") {Id = new Guid("a07c074f-38ca-428e-ae53-7780b7012713"),ThumbnailName = "kitchen_tall.jpg" },
            new Image("kitchen_top.jpg",".jpg","Content") {Id = new Guid("556ca249-26bd-42ea-b41e-c68e216282ed"),ThumbnailName = "kitchen_top.jpg" },
        };
    }
}
