using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Signatures;
using Serilog;

namespace Signicat.Validator.IdfyPades.ValidatorService
{
    public class PDFValidator
    {
        private const string signatureFieldName = "Signature1";

        public Task<PDFValidationResponse> Validate(byte[] pdfBytes)
        {
            try
            {
                if (pdfBytes == null || pdfBytes.Length <= 10)
                    return Task.FromResult(new PDFValidationResponse {Message = "Not valid PDF"});

                using (var ms = new MemoryStream(pdfBytes))
                {
                    using (var reader = new PdfReader(ms))
                    {
                        var pdfDoc = new PdfDocument(reader);
                        var signatureUtil = new SignatureUtil(pdfDoc);
                        var response = new PDFValidationResponse
                        {
                            NumberOfPages = pdfDoc.GetNumberOfPages(),
                            Attachments = new List<PDFAttachment>()
                        };

                        if (signatureUtil.DoesSignatureFieldExist(signatureFieldName))
                        {
                            var certificate =
                                signatureUtil.ReadSignatureData(signatureFieldName).GetSigningCertificate();

                            var signatureData = signatureUtil.ReadSignatureData(signatureFieldName);


                            response.SignatureValid = signatureData.VerifySignatureIntegrityAndAuthenticity();
                            response.TimestampValid = signatureData.VerifyTimestampImprint();
                            response.Signed = signatureData.GetSignDate();

                            response.SigningCertificate =
                                new X509Certificate2(certificate.CertificateStructure.GetEncoded("base64"));
                        }
                        else
                        {
                            response.Message = "No signature field found";
                        }

                        var root = pdfDoc.GetCatalog().GetPdfObject();
                        var documentnames = root.GetAsDictionary(PdfName.Names);
                        var embeddedfiles = documentnames.GetAsDictionary(PdfName.EmbeddedFiles);
                        var filespecs = embeddedfiles.GetAsArray(PdfName.Names);

                        try
                        {
                            for (var i = 0; i < filespecs.Size();)
                            {
                                filespecs.GetAsString(i++);
                                var filespec = filespecs.GetAsDictionary(i++);
                                var refs = filespec.GetAsDictionary(PdfName.EF);
                                foreach (var key in refs.KeySet())
                                    response.Attachments.Add(new PDFAttachment
                                    {
                                        Data = refs.GetAsStream(key).GetBytes(),
                                        Name = filespec.GetAsString(key).ToString()
                                    });
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Logger.Error(e, "Error getting attachments from PDF");
                        }

                        response.NumberOfAttachments = response.Attachments.Count;

                        return Task.FromResult(response);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Error parsing/validating PDF");
                throw;
            }
        }
    }
}