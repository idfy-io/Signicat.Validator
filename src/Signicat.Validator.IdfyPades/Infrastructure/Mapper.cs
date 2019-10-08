using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
                Attachments = response.Attachments?.Select(x=>new Signicat.Validator.IdfyPades.Models.PDFAttachment()
                {
                    Data = x.Data,
                    Name = x.ToString(),
                }).ToList(),
                Signed = response.Signed,
                SigningCertificate =response.SigningCertificate==null ? null:  new CertificateDTO()
                {
                    FriendlyName = response.SigningCertificate?.FriendlyName,
                    IssuerName = response.SigningCertificate.IssuerName,
                    Issuer = response.SigningCertificate.Issuer,
                    NotAfter = response.SigningCertificate.NotAfter,
                    NotBefore = response.SigningCertificate.NotBefore,
                    SerialNumber = response.SigningCertificate.SerialNumber,
                    SignatureAlgorithm = response.SigningCertificate.SignatureAlgorithm,
                    Subject = response.SigningCertificate.Subject,
                    SubjectName = response.SigningCertificate.SubjectName,
                    PemFormated = response.SigningCertificate.ExportToPEM(),

                },
                NumberOfAttachments = response.NumberOfAttachments,
                Message = response.Message,
                TimestampValid = response.TimestampValid,
                SignatureValid = response.SignatureValid,
                NumberOfPages = response.NumberOfPages,
                
                
            };
        }
    }

    public static class CertificateHelper
    {
        /// <summary>
        /// Export a certificate to a PEM format string
        /// </summary>
        /// <param name="cert">The certificate to export</param>
        /// <returns>A PEM encoded string</returns>
        public static string ExportToPEM(this X509Certificate2 cert)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }
    }
}