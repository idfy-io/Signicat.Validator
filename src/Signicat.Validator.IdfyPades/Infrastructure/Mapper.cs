using System.Linq;
using Signicat.Validator.IdfyPades.Models;
using Signicat.Validator.IdfyPades.ValidatorService;

namespace Signicat.Validator.IdfyPades.Infrastructure
{
    public static class Mapper
    {
        public static ValidationResponse Map(this PDFValidationResponse response)
        {
            return new ValidationResponse()
            {
                Attachments = response.Attachments!=null ? response.Attachments.Select(x=>new Signicat.Validator.IdfyPades.Models.PDFAttachment()
                {
                    Data = x.Data,
                    Name = x.ToString(),
                }).ToList():null,
                Signed = response.Signed,
                SigningCertificate = response.SigningCertificate,
                NumberOfAttachments = response.NumberOfAttachments,
                Message = response.Message,
                TimestampValid = response.TimestampValid,
                SignatureValid = response.SignatureValid,
                NumberOfPages = response.NumberOfPages,
                
            };
        }
    }
}