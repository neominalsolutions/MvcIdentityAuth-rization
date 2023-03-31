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

// 3. Uygulama ile alakal� k�s�m

// Uygulamam�za User ve Role s�n�flar�n� tan�tt�k
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
  opt.SignIn.RequireConfirmedEmail = false; // email confirm etmeden login etsin
  opt.User.RequireUniqueEmail = true; // email unique olmal�
  //opt.Password.RequireDigit = false;
  //opt.Password.RequireLowercase = false;
  opt.Password.RequireDigit = true; // parolada numeric alan olaml� gibi ayarlar� yapabiliyoruz.
  
}).AddEntityFrameworkStores<AppIdentityDbContext>();


// Cookie Authentication Yapaca��m�z� s�yledik
// Authentication Scheme CookieAuthentication belirledik
// kimlik do�rumala i�lemi yap�caz.
builder.Services.AddAuthentication()
     .AddCookie();


//// Authentication Cookie ayarlar�n� yapt�k
builder.Services.ConfigureApplicationCookie(option => //cookie burada yarat�l�r.
{
  option.Cookie.Name = "UserLoginCookie";
  option.LoginPath = "/Auth/Login"; // login de�ilsek buraya y�nlen
  option.AccessDeniedPath = "/Auth/AccessDenied"; // yetkimiz yoksa y�nlen
});

// uygulamaya yetkilendirme servisleri ekle
builder.Services.AddAuthorization(opt =>
{
  // manager veya admin roline 
  opt.AddPolicy("AdminOrManager", policy => policy.RequireRole("Manager", "Admin")); // ya manager yada admin olmal�
  // bizim oturum a�an kullan�c�n�n mail uzant�s� neominal.com olmal�
  opt.AddPolicy("DomainCheck", policy => policy.AddRequirements(new SpesificDomainRequirement("neominal.com")));

  opt.AddPolicy("HasLinkedInProfile", policy => policy.RequireClaim("LinkedInProfile"));

});

// DomainRequirementHandler servisi sisteme tan�tt�k
// Not AuthorizationHandler servicleri sisteme transient olarak tan�t�yoruz. Scope ve Singleton servisler ile hata verip �al��amayaca��n� s�yl�yor.
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

// not 3. �rnek i�in bunu eklemeyi unutmayal�m
// yoksa login olam�yoruz. cookie olu�uyor ama [Authorize] attribute �al��m�yor
app.UseAuthentication();
// bu servis olmadan yetkilendirme �al��maz
app.UseAuthorization();

#endregion



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
