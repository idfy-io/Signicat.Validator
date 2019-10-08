using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Signicat.Validator.IdfyPades.Models.Errors
{
    public class SignicatException : Exception
    {
        public SignicatException(SignicatError error) : base($"{error.Code}: {error.Message}")
        {
            Error = error;
        }

        public SignicatError Error { get; }
    }

    public class ApiErrorObject
    {
        public ApiErrorObject(string message)
        {
            Message = message;
        }

        public ApiErrorObject(string message, string code)
        {
            Message = message;
            Code = code;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }

    public class ApiErrorResult : ObjectResult
    {
        public ApiErrorResult(HttpStatusCode statusCode, string message) : base(new ApiErrorObject(message))
        {
            StatusCode = (int)statusCode;
        }

        public ApiErrorResult(HttpStatusCode statusCode, SignicatError error) : base(new ApiErrorObject(error.Message, error.Code))
        {
            StatusCode = (int)statusCode;
        }
    }
}