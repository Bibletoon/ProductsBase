using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductsBase.Domain.Common.Extensions;

namespace ProductsBase.Api.Middlewares.Filters
{
    public class ModelStateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> error = context.ModelState.GetErrorMessages();
                context.Result = new BadRequestObjectResult(error);
            }
            else
            {
                await next();
            }
        }
    }
}