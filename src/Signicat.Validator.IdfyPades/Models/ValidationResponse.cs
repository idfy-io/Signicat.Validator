using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Signicat.Validator.IdfyPades.Infrastructure.Extensions;


namespace Signicat.Validator.IdfyPades.Models
{
    public class ValidationResponse
    {
        /// <summary>
        /// The date and time the PDF is signed if signed
        /// </summary>
        public DateTime? Signed { get; set; }

        /// <summary>
        /// The public part of the certificate that signed the document
        /// </summary>
        public CertificateDTO SigningCertificate { get; set; }

        /// <summary>
        /// Number of attachments in the PAdES
        /// </summary>
        public int NumberOfAttachments { get; set; }

        /// <summary>
        /// List of attachments filename and content
        /// </summary>
        public List<PDFAttachment> Attachments { get; set; }

        /// <summary>
        /// Is the digital signature valid
        /// </summary>
        public bool SignatureValid { get; set; }

        /// <summary>
        /// Is there a timestamp and is it valid
        /// </summary>
        public bool TimestampValid { get; set; }

        /// <summary>
        /// Number of pages in the PDF
        /// </summary>
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Error message 
        /// </summary>
        public string Message { get; set; }
    }

    public class PDFAttachment
    {
        /// <summary>
        /// Attachment file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attachment file content as base64
        /// </summary>
        public byte[] Data { get; set; }
    }

    public class CertificateDTO
    {
        /// <summary>
        /// The certificate in PEM format
        /// </summary>
        public string PemFormated { get; set; }


        /// <summary>Gets the name of the certificate authority that issued the X.509v3 certificate.</summary>
        /// <returns>The name of the certificate authority that issued the X.509v3 certificate.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
        public string Issuer { get; set; }

        /// <summary>Gets the subject distinguished name from the certificate.</summary>
        /// <returns>The subject distinguished name from the certificate.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
        public string Subject { get; set; }

        /// <summary>Gets or sets the associated alias for a certificate.</summary>
        /// <returns>The certificate&amp;#39;s friendly name.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable.</exception>
        public string FriendlyName { get; set; }

        /// <summary>Gets the distinguished name of the certificate issuer.</summary>
        /// <returns>An <see cref="System.Security.Cryptography.X509Certificates.X500DistinguishedName"></see> object that contains the name of the certificate issuer.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
        public X500DistinguishedName IssuerName { get; set; }

        /// <summary>Gets the date in local time after which a certificate is no longer valid.</summary>
        /// <returns>A <see cref="System.DateTime"></see> object that represents the expiration date for the certificate.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable.</exception>
        public DateTime NotAfter { get; set; }

        /// <summary>Gets the date in local time on which a certificate becomes valid.</summary>
        /// <returns>A <see cref="System.DateTime"></see> object that represents the effective date of the certificate.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable.</exception>
        public DateTime NotBefore { get; set; }

        /// <summary>Gets the serial number of a certificate.</summary>
        /// <returns>The serial number of the certificate.</returns>
        public string SerialNumber { get; set; }

        /// <summary>Gets the algorithm used to create the signature of a certificate.</summary>
        /// <returns>Returns the object identifier (<see cref="System.Security.Cryptography.Oid"></see>) of the signature algorithm.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable.</exception>
        public Oid SignatureAlgorithm { get; set; }

        /// <summary>Gets the subject distinguished name from a certificate.</summary>
        /// <returns>An <see cref="System.Security.Cryptography.X509Certificates.X500DistinguishedName"></see> object that represents the name of the certificate subject.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
        public X500DistinguishedName SubjectName { get; set; }


        /// <summary>Gets the thumbprint of a certificate.</summary>
        /// <returns>The thumbprint of the certificate.</returns>
        public string Thumbprint { get; }

        /// <summary>Gets the X.509 format version of a certificate.</summary>
        /// <returns>The certificate format.</returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable.</exception>
        public int Version { get; }

    }


    public class SignicatError
    {
        public SignicatError(SignicatErrorCode errorCode)
        {
            Code = $"ID-{(int)errorCode}";
            Message = errorCode.GetDescription();
        }

        public string Code { get; }

        public string Message { get; }
    }

    public enum SignicatErrorCode
    {
        [Description("The PDF was not valid")]
        NotValidPDF = 1000,

        

        [Description("Something went wrong. Please contact us for assistance if the problem persists.")]
        UnexpectedError = 1500,
    }
}
