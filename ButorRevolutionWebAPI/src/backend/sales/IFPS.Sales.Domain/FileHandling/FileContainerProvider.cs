using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IFPS.Sales.Domain.FileHandling
{
    public class FileContainerProvider
    {
        public static class Containers
        {
            public const string OrderDocuments = "OrderDocuments";
            public const string OrderTests = "OrderTests";
            public const string ProfilePictures = "ProfilePictures";
            public const string MaterialsPictures = "MaterialPictures";
            public const string FurnitureUnitPictures = "FurniturePictures";
            public const string Content = "Content";
            public const string Materials = "Materials";
            public const string FurnitureUnits = "FurnitureUnits";
            public const string CartAnalysis = "CartAnalysis";
        }

        public static IReadOnlyDictionary<string, (string[] Extensions, bool IsPublic)> ContainerSettings { get; } =
            new ReadOnlyDictionary<string, (string[] Extensions, bool IsPublic)>(
                new Dictionary<string, (string[] Extensions, bool IsPublic)>
                {
                    [Containers.OrderDocuments] = (new string[] { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".odt", ".xls", ".xlsx", ".ods", ".pdf" , }, true),
                    [Containers.OrderTests] = (new string[] { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".odt", ".xls", ".xlsx", ".ods", ".pdf" , }, false),
                    [Containers.ProfilePictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.MaterialsPictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.FurnitureUnitPictures] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.Content] = (new string[] { ".jpg", ".jpeg", ".png" }, true),
                    [Containers.Materials] = (new string[] { ".csv" }, true),
                    [Containers.FurnitureUnits] = (new string[] { ".csv", ".png" }, true),
                    [Containers.CartAnalysis] = (new string[] { ".json" }, false),
                });
    }
}
