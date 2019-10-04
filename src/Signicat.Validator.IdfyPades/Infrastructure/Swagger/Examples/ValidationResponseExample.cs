using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Signicat.Validator.IdfyPades.Models;
using Swashbuckle.AspNetCore.Examples;

namespace Signicat.Validator.IdfyPades.Infrastructure.Swagger.Examples
{
    public class ValidationResponseExample: IExamplesProvider
    {
        public object GetExamples()
        {
            return new ValidationResponse()
            {
                NumberOfAttachments = 2,
                NumberOfPages = 3,
                SignatureValid = true,
                TimestampValid = true,
                Signed = DateTime.Now.AddMinutes(-60),
                SigningCertificate = new X509Certificate2(Convert.FromBase64String(
                    @"MIIEtDCCA5ygAwIBAgILDK0BAIiuarKYNzQwDQYJKoZIhvcNAQELBQAwSzELMAkGA1UEBhMCTk8xHTAbBgNVBAoMFEJ1eXBhc3MgQVMtOTgzMTYzMzI3MR0wGwYDVQQDDBRCdXlwYXNzIENsYXNzIDMgQ0EgMzAeFw0xODAzMTMxNDM3NThaFw0yMTAzMTMyMjU5MDBaMEgxCzAJBgNVBAYTAk5PMRYwFAYDVQQKDA1JREZZIE5PUkdFIEFTMQ0wCwYDVQQDDARJZGZ5MRIwEAYDVQQFEwk5OTgzMDMxNjgwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCu9GxWQi3gDfvqYRPKGn+3gO9sv5XFqQegG1RWe8Zg+/g0KNYixsRsJjqZWYNGnF/CFhAuOMuA1v30HpO5RepEL/4lE/I4i5ZSIpF9l3WDl/YjzCbqWRX3UHHXlZZUw3GvbjCitEbUI/ZeId95lb/VRYpLsqFpvZiuT9puOEGPVbPWHjWxbrYUsbt6b1/sc7Q8s4KhhG80vNRirlL9McE0SBwVzsyjWlNuqi+lYOdH6dQlZn8vYOcYg3WJGNBU4d71wrmwKgxJUSiXZBZmHtYGTO5d4UuESY75wcYvmijT1byup7vA6+OHgoQBS7Fw9YZ/Ys/hXabIhQMZFgQoxvQdAgMBAAGjggGaMIIBljAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFMzD+Ae3nG16TvWnKx0F+bNHHJHRMB0GA1UdDgQWBBRsOGlxmrAAW27Q29MFtoNvHx3CHDAOBgNVHQ8BAf8EBAMCBkAwFQYDVR0gBA4wDDAKBghghEIBGgEDBTCBpQYDVR0fBIGdMIGaMC+gLaArhilodHRwOi8vY3JsLmJ1eXBhc3Mubm8vY3JsL0JQQ2xhc3MzQ0EzLmNybDBnoGWgY4ZhbGRhcDovL2xkYXAuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBDQSUyMDM/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdDB6BggrBgEFBQcBAQRuMGwwMwYIKwYBBQUHMAGGJ2h0dHA6Ly9vY3NwLmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM0NBMzA1BggrBgEFBQcwAoYpaHR0cDovL2NydC5idXlwYXNzLm5vL2NydC9CUENsYXNzM0NBMy5jZXIwDQYJKoZIhvcNAQELBQADggEBAAYFOsNh0ji7zXFqjGoRYryAMAhEFIZ3e7N1c6g4cmIahU8TbtjkVPt/UblRx/lHAF2f+VZk4+t090Ot9i86M5QrcQKlr26F1Q8NEoUqY9ahAO0J86DL5ziTpqMzbESynXIhq+FXJkWOBXQ8ZN9JHupaUdACvFhltLSoDckV+Ie2Hm2RYFGFuLbVlWJjwIqZG+HCJ6xQQauejdVkoyYIkhHt3aykERw4sM+BSagTTXyq1uitGqVgC+jqFvnYpjduQkaxS2jkFJHDaBrCE15jSI+QXmq0q6nn5htOwyNd6WB5nSPSsqaMApP6RzB4MPsc76Wf4o2lbIHglAIa0uX4CXY=")),
                Attachments = new List<PDFAttachment>()
                {
                    new PDFAttachment()
                    {
                        Name = "ElektroniskSignatur.xml",
                        Data = Encoding.UTF8.GetBytes(
                            "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjxTaWduZWREb2N1b....."),
                    },
                    new PDFAttachment()
                    {
                        Name = "SignaturDetaljer-NO.pdf",
                        Data = Encoding.UTF8.GetBytes(
                            "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjxTaWduZWREb2N1b....."),
                    },
                }
            };
        }
    }
}