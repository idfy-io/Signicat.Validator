using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Signicat.Validator.IdfyPades.Infrastructure.Swagger
{
    public static class SwaggerHelpers
    {
        /// <summary>
        /// Custom strategy for assigning a default "tag" to Swagger actions.
        /// Will use the custom value provided by <see cref="OperationGroupAttribute"/> if present, otherwise uses the controller name.
        /// </summary>
        /// <param name="apiDesc"></param>
        /// <returns></returns>
        public static IList<string> CustomOrDefaultOperationGroup(ApiDescription apiDesc)
        {
            var defaultOperationGroup = (apiDesc.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;

            if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                return new List<string>() { defaultOperationGroup };

            var customOperationGroup = methodInfo.DeclaringType.GetCustomAttributes(true)
                .OfType<OperationGroupAttribute>().FirstOrDefault();

            return new List<string>()
                {customOperationGroup != null ? customOperationGroup.Name : defaultOperationGroup};
        }
    }
}