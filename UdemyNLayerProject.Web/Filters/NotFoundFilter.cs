using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyNLayerProject.Web.DTOs;
using UdemyNLayerProject.Core.Services;

namespace UdemyNLayerProject.Web.Filters
{
    public class NotFoundFilter: ActionFilterAttribute
    {
        private readonly ICategoryService _categoryService;
        public NotFoundFilter(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = (int)context.ActionArguments.Values.FirstOrDefault();
            var product = await _categoryService.GetByIdAsync(id);
            if (product != null)
            {
                await next();
            }
            else
            {
                ErrorDto errorDto = new ErrorDto();
                errorDto.Status = 404;
                errorDto.Errors.Add($"Id'si {id} olan kategori DB de bulunamadı");
                context.Result = new RedirectToActionResult("Error","Home",errorDto);
            }
        }
    }
}
