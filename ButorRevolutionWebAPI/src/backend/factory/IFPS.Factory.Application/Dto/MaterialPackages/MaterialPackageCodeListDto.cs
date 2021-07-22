using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialPackageCodeListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public MaterialPackageCodeListDto(MaterialPackage materialPackage)
        {
            Id = materialPackage.Id;
            Code = materialPackage.PackageCode;
        }
    }
}
