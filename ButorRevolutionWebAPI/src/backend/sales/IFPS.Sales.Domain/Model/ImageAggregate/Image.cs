using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class Image : Document
    {        
        public string ThumbnailName { get; set; }

        private Image()
        {

        }

        public Image(string fileName, string extension, string containerName) 
            : base(fileName, extension, containerName)
        {            
        }
    }
}
