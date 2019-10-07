using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Signicat.Validator.IdfyPades.ValidatorService;

namespace Tests
{
    public class PDFValidatorTest

    {
        private PDFValidator Validator;
        [SetUp]
        public void Setup()
        {
            Validator=new PDFValidator();
        }

        [Test]        
        public async Task Test_valid_PDF_is_signed_and_valid()
        {
            string path = @"test-valid-pades.pdf";
            var response = await Validator.Validate(System.IO.File.ReadAllBytes(path));

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Signed);
            Console.WriteLine("Signed time: {0}",response.Signed);
            Console.WriteLine("Signature valid: {0}", response.SignatureValid);
            Console.WriteLine("Timestamp valid: {0}", response.TimestampValid);
            Console.WriteLine(response.SigningCertificate.ToString());

            foreach (var responseAttachment in response.Attachments)
            {
                Console.WriteLine(responseAttachment.Name);
                File.WriteAllBytes(responseAttachment.Name,responseAttachment.Data);
            }

            Assert.IsTrue(response.SignatureValid);
            Assert.IsTrue(response.TimestampValid);
            Assert.IsNotNull(response.Signed);
            Assert.IsNotNull(response.SigningCertificate);
            Assert.IsNotNull(response.Attachments);
            
            Assert.AreEqual(2,response.NumberOfAttachments);

            Assert.Pass();
        }
    }
}