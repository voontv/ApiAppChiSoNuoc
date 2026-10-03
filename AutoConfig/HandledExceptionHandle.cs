using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReadMeter.Api.Exceptions;
using System;

namespace ReadMeter.Api.Handlers
{
    public class HandledExceptionHandle : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BaseException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int)exception.StatusCode
                };

                Console.Out.WriteLine(exception.StatusCode + " " + exception.Message);
            }
        }
    }
}