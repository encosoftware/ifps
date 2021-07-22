using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.API.Infrastructure.Swagger
{
    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        private readonly List<string> ParamsToRemove = new List<string>() { "ContentType", "ContentDisposition", "Headers", "Length", "Name", "FileName" };
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.TryGetMethodInfo(out var methodInfo) && methodInfo.CustomAttributes.Any(x => x.AttributeType.Name == "SwaggerFileUploadAttribute"))
            {
                var removable = operation.Parameters.Where(x => ParamsToRemove.Contains(x.Name)).ToList();
                foreach (var param in removable)
                {
                    operation.Parameters.Remove(param);
                }

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "file",
                    In = "formData",
                    Description = null,
                    Required = removable.Any(x => x.Required),
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
