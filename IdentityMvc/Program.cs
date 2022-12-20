using IdentityMvc.Extensions;
using IdentityMvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//////////-----------Identity Started//////////////
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentityWithExtention();
builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "IdentityServerLogin";
    opt.LoginPath = new PathString("/Home/Login");
    opt.LogoutPath = new PathString("/Member/Logout"); //Efektif olarak isimlendiriğimiz Logout için yazdık. 
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true; //kullanıcı her giriş yaptığında cookie süresini 60 gün uzatır.
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Authentication için. kimliği doğrular
app.UseAuthorization(); //yetkilendirme
//Buradaki sıralama önemli önce kimlik doğrulanmalı sonra yetki.

//// NEW AREA ADDED
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllerRoute(
//         name : "areas",
//         pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//     );
// });

//üstteki .net6 dan önce kullanılan area route'u güncel hali aşağıdaki gibi.
app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

///// NEW AREA ADDED END
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();