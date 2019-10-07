using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Signatures;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Signicat.Validator.IdfyPades.ValidatorService
{
    public class PDFValidator
    {
        private const string signatureFieldName = "Signature1";
        public async Task<PDFValidationResponse> Validate(byte[] pdfBytes)
        {

            if(pdfBytes==null || pdfBytes.Length<=10)
                return new PDFValidationResponse(){Message = "Not valid PDF"};

            using (var ms = new MemoryStream(pdfBytes))
            {
                using (var reader = new PdfReader(ms))
                {
                    PdfDocument pdfDoc = new PdfDocument(reader);
                    SignatureUtil signatureUtil = new SignatureUtil(pdfDoc);
                    PDFValidationResponse response = new PDFValidationResponse
                    {
                        NumberOfPages = pdfDoc.GetNumberOfPages(),
                        Attachments = new List<PDFAttachment>()
                    };

                    if (signatureUtil.DoesSignatureFieldExist(signatureFieldName))
                    {
                        X509Certificate certificate =
                            signatureUtil.ReadSignatureData(signatureFieldName).GetSigningCertificate();

                        var signatureData = signatureUtil.ReadSignatureData(signatureFieldName);


                        response.SignatureValid = signatureData.VerifySignatureIntegrityAndAuthenticity();
                        response.TimestampValid = signatureData.VerifyTimestampImprint();
                        response.Signed = signatureData.GetSignDate();

                        response.SigningCertificate = new X509Certificate2(certificate.CertificateStructure.GetEncoded("base64"));
                        
                    }
                    else
                    {
                        response.Message = "No signature field found";
                    }

                    PdfDictionary root = pdfDoc.GetCatalog().GetPdfObject();
                    PdfDictionary documentnames = root.GetAsDictionary(PdfName.Names);
                    PdfDictionary embeddedfiles = documentnames.GetAsDictionary(PdfName.EmbeddedFiles);
                    PdfArray filespecs = embeddedfiles.GetAsArray(PdfName.Names);

                    try
                    {
                        for (int i = 0; i < filespecs.Size();)
                        {
                            filespecs.GetAsString(i++);
                            var filespec = filespecs.GetAsDictionary(i++);
                            var refs = filespec.GetAsDictionary(PdfName.EF);
                            foreach (var key in refs.KeySet())
                            {
                                response.Attachments.Add(new PDFAttachment()
                                {
                                    Data = refs.GetAsStream(key).GetBytes(),
                                    Name = filespec.GetAsString(key).ToString(),
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    response.NumberOfAttachments = response.Attachments.Count;

                    return response;
                }
            }

            
        }

    }
}