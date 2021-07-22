using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IFPS.Factory.Domain.FileHandling
{
    public class FileContainerProvider
    {
        public static class Containers
        {
            public const string OrderDocuments = "OrderDocuments";
            public const string ProfilePictures = "ProfilePictures";
            public const string MaterialsPictures = "MaterialPictures";
            public const string MaterialTestImages = "MaterialTestImages";
            public const string FurnitureUnitPictures = "FurniturePictures";
            public const string QRCodes = "QRCodes";
            public const string Content = "Content";
            public const string Temp = "Temp";
            public const string FurnitureUnits = "FurnitureUnits";
            public const string MaterialPackages = "MaterialPackages";
            public const string XxlData = "XxlData";
            public const string Optimizer = "Optimizer";
        }


        public static IReadOnlyDictionary<string, (string[] Extensions, bool IsPublic)> ContainerSettings { get; } =
            new ReadOnlyDictionary<string, (string[] Extensions, bool IsPublic)>(
                new Dictionary<string, (string[] Extensions, bool IsPublic)>
                {
                    [Containers.OrderDocuments] = (new string[] { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".pdf" }, true),
                    [Containers.ProfilePictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.MaterialsPictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.MaterialTestImages] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.FurnitureUnitPictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.QRCodes] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.Content] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.FurnitureUnits] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.MaterialPackages] = (new string[] { ".csv" }, true),
                    [Containers.Temp] = (new string[] { ".jpg", ".jpeg", ".png", ".zip", ".docx", ".csv", ".doc" }, true),                    
                    [Containers.XxlData] = (new string[] { ".zip" }, true),                   
                    [Containers.Optimizer] = (new string[] { ".json", ".zip", ".html", ".css" }, true),
                });
    }
}
