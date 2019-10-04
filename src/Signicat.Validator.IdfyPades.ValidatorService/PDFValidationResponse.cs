using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Signicat.Validator.IdfyPades.ValidatorService
{
    public class PDFValidationResponse
    {
        public DateTime? Signed { get; set; }

        public X509Certificate2 SigningCertificate { get; set; }

        public int NumberOfAttachments { get; set; }

        public List<PDFAttachment> Attachments { get; set; }
        public bool SignatureValid { get; set; }
        public bool TimestampValid { get; set; }
        public int NumberOfPages { get; set; }
        public string Message { get; set; }
    }
}