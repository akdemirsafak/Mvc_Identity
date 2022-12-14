﻿using IdentityMvc.CustomValidations;
using IdentityMvc.Localizations;
using IdentityMvc.Models;

namespace IdentityMvc.Extensions;

public static class StartupExtensions
{
    public static void AddIdentityWithExtention(this IServiceCollection serviceCollection)
    {
        
        serviceCollection.AddIdentity<AppUser, AppRole>(
            options =>
                {
                    //Default ayarları değiştirmek istersek bu kısımda ayarlamalar yapabiliriz.
                    options.User.RequireUniqueEmail = true; // UserName default olarak unique !
                    options.User.AllowedUserNameCharacters = "qwertyuopasdfghjklizxcvbnm1234567890_"; //sadece küçük harfler, sayılar ve _

                    //Parola ile ilgili düzenlemeler.
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false; //özel karakter zorunluluğunu kaldırdık
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                }).AddUserValidator<UserValidator>()
            .AddPasswordValidator<PasswordValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddEntityFrameworkStores<AppDbContext>();

    }
}