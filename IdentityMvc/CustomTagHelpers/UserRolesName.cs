using IdentityMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityMvc.CustomTagHelpers;

[HtmlTargetElement("td", Attributes = "user-roles")]
public class UserRolesName : TagHelper
{
    public UserRolesName(UserManager<AppUser> userManager)
    {
        UserManager = userManager;
    }

    public UserManager<AppUser> UserManager { get; set; }

    [HtmlAttributeName("user-roles")] public string UserId { get; set; }


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        //Bu method string veya html ifadeyi yukarıda htmltargetelement de belirttiğimiz şartlara uyan(html sayfasında td taginde  user-roles="" olan kısıma) yazar.

        var user = await UserManager.FindByIdAsync(UserId);
        var userRoles = await UserManager.GetRolesAsync(user);
        var html = string.Empty;
        userRoles.ToList().ForEach(x => { html += $"<span class='badge badge-info text-dark'> {x} </span>"; });
        output.Content.SetHtmlContent(html);
        //return base.ProcessAsync(context, output);
    }
}