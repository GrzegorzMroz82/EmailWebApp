using Email.Services.Emails;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Services
{
    public static class Response
    {
        public static Response<T> Fail<T>(EmailResult error, string message = null, T data = default) =>
            new Response<T>(data, message, error);

        public static Response<T> Ok<T>(T data, string message = null) =>
            new Response<T>(data, message, null);
    }

    public class Response<T>
    {
        public Response(T data, string msg, EmailResult? errorCode)
        {
            Data = data;
            Message = msg;
            ErrorCode = errorCode;
        }
        public T Data { get; set; }
        public string Message { get; set; }
        public EmailResult? ErrorCode { get; set; }
    }
}
