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
//   ------------- BU KISMI STARTUP EXTENSIONS' a taşıdık. --------

// builder.Services.AddIdentity<AppUser, AppRole>(
//     
//     options =>
//     {
//    
//         //Default ayarları değiştirmek istersek bu kısımda ayarlamalar yapabiliriz.
//         options.User.RequireUniqueEmail = true; // UserName default olarak unique !
//         options.User.AllowedUserNameCharacters = "qwertyuopasdfghjklizxcvbnm1234567890_"; //sadece küçük harfler, sayılar ve _
//         
//         //Parola ile ilgili düzenlemeler.
//         options.Password.RequiredLength = 6;
//         options.Password.RequireNonAlphanumeric = false; //özel karakter zorunluluğunu kaldırdık
//         options.Password.RequireLowercase = true;
//         options.Password.RequireUppercase = false;
//         options.Password.RequireDigit = false;
//     }
//     
// ).AddEntityFrameworkStores<AppDbContext>();

///////////----Identity End//////////////


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

app.UseAuthorization();

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
        name : "areas",
        pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}");

///// NEW AREA ADDED END
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();