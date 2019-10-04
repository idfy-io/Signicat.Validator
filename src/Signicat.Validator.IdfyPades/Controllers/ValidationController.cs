using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Signicat.Validator.IdfyPades.Infrastructure.Swagger.Examples;
using Signicat.Validator.IdfyPades.Models;
using Signicat.Validator.IdfyPades.ValidatorService;

namespace Signicat.Validator.IdfyPades.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly PDFValidator PdfValidator;

        public ValidationController(PDFValidator pdfValidator)
        {
            PdfValidator = pdfValidator;
        }

        /// <summary>
        /// Validate Pades file
        /// </summary>
        /// <remarks>
        /// Validate a PAdES file created by Signicat, returns certificate, attachments and other metadata.
        /// </remarks>
        /// <param name="files">Http file upload</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ValidationResponse),200)]
        [Produces("application/json")]
        [Swashbuckle.AspNetCore.Examples.SwaggerResponseExample(200,typeof(ValidationResponseExample))]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            if ( Request.Form?.Files!=null && Request.Form.Files.Any() && Request.Form?.Files[0]?.Length > 0)
            {
                var file = Request.Form.Files[0];
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    var response = await PdfValidator.Validate(ms.ToArray());

                    return Ok(response);
                }                
            }
            else
            {
                return BadRequest();
            }

        }


    }
}
