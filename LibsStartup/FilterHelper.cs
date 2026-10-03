
using Microsoft.AspNetCore.Mvc;
using ReadMeter.Api.Handlers;


namespace ReadMeter.Api.LibsStartup
{
    public static class FilterHelper
    {
        public static void Register(this MvcOptions options)
        {
            options.Filters.Add(typeof(UnHandledExceptionHandle));
            options.Filters.Add(typeof(HandledExceptionHandle));
            options.Filters.Add(typeof(ValidateModelAttribute));
        }
    }
}
