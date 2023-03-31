using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MvcLab4.Entities;
using MvcLab4.Identity;
using MvcLab4.Repository;
using MvcLab4.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region LessonOne

// 1. dersteki ilk uygulamada bu context olucak
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddHttpContextAccessor();

#endregion

#region LessonTwo

// 2. dersteki ilk uygulamada bu context olucak

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Lesson3

// 3. Uygulama ile alakalý kýsým

// Uygulamamýza User ve Role sýnýflarýný tanýttýk
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
  opt.SignIn.RequireConfirmedEmail = false; // email confirm etmeden login etsin
  opt.User.RequireUniqueEmail = true; // email unique olmalý
  //opt.Password.RequireDigit = false;
  //opt.Password.RequireLowercase = false;
  opt.Password.RequireDigit = true; // parolada numeric alan olamlý gibi ayarlarý yapabiliyoruz.
  
}).AddEntityFrameworkStores<AppIdentityDbContext>();


// Cookie Authentication Yapacaðýmýzý söyledik
// Authentication Scheme CookieAuthentication belirledik
// kimlik doðrumala iþlemi yapýcaz.
builder.Services.AddAuthentication()
     .AddCookie();


//// Authentication Cookie ayarlarýný yaptýk
builder.Services.ConfigureApplicationCookie(option => //cookie burada yaratýlýr.
{
  option.Cookie.Name = "UserLoginCookie";
  option.LoginPath = "/Auth/Login"; // login deðilsek buraya yönlen
  option.AccessDeniedPath = "/Auth/AccessDenied"; // yetkimiz yoksa yönlen
});

// uygulamaya yetkilendirme servisleri ekle
builder.Services.AddAuthorization(opt =>
{
  // manager veya admin roline 
  opt.AddPolicy("AdminOrManager", policy => policy.RequireRole("Manager", "Admin")); // ya manager yada admin olmalý
  // bizim oturum açan kullanýcýnýn mail uzantýsý neominal.com olmalý
  opt.AddPolicy("DomainCheck", policy => policy.AddRequirements(new SpesificDomainRequirement("neominal.com")));

  opt.AddPolicy("HasLinkedInProfile", policy => policy.RequireClaim("LinkedInProfile"));

});

// DomainRequirementHandler servisi sisteme tanýttýk
// Not AuthorizationHandler servicleri sisteme transient olarak tanýtýyoruz. Scope ve Singleton servisler ile hata verip çalýþamayacaðýný söylüyor.
builder.Services.AddTransient<IAuthorizationHandler, DomainRequirementHandler>();

#endregion


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


#region Lesson3

// not 3. örnek için bunu eklemeyi unutmayalým
// yoksa login olamýyoruz. cookie oluþuyor ama [Authorize] attribute çalýþmýyor
app.UseAuthentication();
// bu servis olmadan yetkilendirme çalýþmaz
app.UseAuthorization();

#endregion



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
