using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes2._0.Data;
using NetKubernetes2._0.Data.Usuarios;
using NetKubernetes2._0.Middleware;
using NetKubernetes2._0.Models;
using NetKubernetes2._0.Profiles;
using NetKubernetes2._0.Token;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperconfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new InmueblesProfile());
});

IMapper mapper = mapperconfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var builderSecurity = builder.Services.AddIdentityCore<Usuario>();
var identifyBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);
identifyBuilder.AddEntityFrameworkStores<AppDbContext>();
identifyBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.AddSingleton<ISystemClock , SystemClock>();
builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();
builder.Services.AddScoped<IUsuarioSesion, IUsuarioSesion>();
builder.Services.AddScoped<IUsuarioRepository, IUsuarioRepository>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
   {
       opt.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = key,
           ValidateAudience = false,
           ValidateIssuer = false
       };

   });

builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
 {
     builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
 }));



var app = builder.Build();

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ManagerMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors("corsapp");


app.Run();

using (var ambiente = app.Services.CreateScope())
{
    var services = ambiente.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Usuario>>();
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await LoadDatabase.InsertarData(context,userManager);
    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Ocurrio un error en la migracion");
            
    }
}
