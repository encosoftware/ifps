using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed
{
    public class DocumentSeed : IEntitySeed<Document>
    {
        public Document[] Entities => new[]
        {
            Document.FromSeedData("de464091-4c2e-45d7-b598-8c5c5f151d42.png" ,".png" ,"OrderDocuments","MSZ0001-2019_Render_1.png"                    ,FileExtensionTypeEnum.Picture    ,1 ,1,11,new Guid("1d1d28ef-5cec-4dcc-b1f1-17d8e2454762")),
            Document.FromSeedData("96c5f935-06e1-4a4a-b2aa-17b51bd410fa.png" ,".png" ,"OrderDocuments","MSZ0001-2019_Render_2.png"                    ,FileExtensionTypeEnum.Picture    ,1 ,1,11,new Guid("2790a5b4-6cc5-424d-9bd6-5558f5ac7001")),
            Document.FromSeedData("d48d8e3d-1da1-470c-863f-d0bb67bfd6a1.pdf" ,".pdf" ,"OrderDocuments","MSZ0001-2019_Quotation_1.pdf"                 ,FileExtensionTypeEnum.Pdf        ,2 ,1, 2,new Guid("bf189eb8-3da7-4f12-b0f0-2eba2340f7a2")),
            Document.FromSeedData("4292d306-7631-4251-b5a8-d6d80c8fb408.pdf" ,".pdf" ,"OrderDocuments","MSZ0001-2019_Contract_1.pdf"                  ,FileExtensionTypeEnum.Pdf        ,3 ,1, 3,new Guid("9dde686c-a1b5-4dfa-adc0-1f8eac43a71e")),
            Document.FromSeedData("42b2de8d-555f-4fd3-9040-32ccb3c5d096.png" ,".png" ,"OrderDocuments","MSZ0001-2019_TechnicalDrawing_1.png"          ,FileExtensionTypeEnum.Picture    ,5 ,1, 4,new Guid("472d0440-d426-410a-aa15-43b1855c49fb")),
            Document.FromSeedData("57b03da2-7222-4f5c-b768-367b9d421945.docx",".docx","OrderDocuments","MSZ0001-2019_CertificateForInstallment_1.docx",FileExtensionTypeEnum.Word       ,12,1, 5,new Guid("acc372e2-e66d-4c19-8177-50c22d50c359")),
        };
        //public Document[] Entities => new Document[] { };
    }
}
