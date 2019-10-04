using System.Globalization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Signicat.Validator.IdfyPades.Infrastructure.Swagger
{
    public class SummaryOperationNameFilter : IOperationFilter
    {
        private readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var val = operation.Summary ?? operation.OperationId;
            val = _textInfo.ToTitleCase(val);
            operation.OperationId = val.Replace(" ", "");
        }
    }
}