﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityMvc.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errorMessages)
    {
        errorMessages.ForEach(x =>
        {
            modelState.AddModelError(string.Empty,x);
        });
    }
}