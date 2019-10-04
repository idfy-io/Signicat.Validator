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
        [Ignore("Requires local file")]
        public async Task Test1()
        {
            string path = @"C:\Users\rune.synnevaag\Downloads\Visena-Merge-Fields-Reference-07.12.2015_pades.pdf";
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

            Assert.Pass();
        }
    }
}