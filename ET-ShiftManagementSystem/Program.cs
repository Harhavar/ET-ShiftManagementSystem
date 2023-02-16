using Microsoft.EntityFrameworkCore;
using Servises.ProjectServises;
using Microsoft.EntityFrameworkCore.SqlServer;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ShiftMgtDbContext.Entities;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ET_ShiftManagementSystem.Servises;
using ET_ShiftManagementSystem.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ET_ShiftManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication ",
        Description = "Enter a valid JWT bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] {} }
    });
});

builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddScoped<IProjectServises, ProjectServises>();
builder.Services.AddScoped<IProjectDatailServises, ProjectDatailServises>();
//builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IShiftServices, ShiftServices>();
builder.Services.AddScoped<ICommentServices, CommentServices>();
//builder.Services.AddScoped<ICredentialServices, CredentialServices>();

builder.Services.AddScoped<IEmailServices, EmailServices>();

builder.Services.AddScoped<IorganizationServices, organizationServices>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<ISREDetiles, SREservices>();

builder.Services.AddScoped<ITenateServices, TenateServices>();


builder.Services.AddScoped<IPermissionServises, PermissionServises>();


builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShiftManagementDbContext>().AddDefaultTokenProviders();

//var JwtSetting = System.Configuration.GetSection("Jwt");
//builder.Services.Configure<JwtBearerDefaults>(JwtSetting);


//for entity framework
//builder.Services.AddDbContext<ShiftManagementDbContext>(option =>
//{
//    option.UseSqlServer
//    (builder.Configuration.GetConnectionString("ProjectAPIConnectioString"),
//    sqlServerOptionsAction: sqlOperation =>
//    {
//        sqlOperation.EnableRetryOnFailure();
//    });

//});

var asd = builder.Configuration.GetConnectionString("ProjectAPIConnectioString");
builder.Services.AddDbContext<ShiftManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectAPIConnectioString"), sqlServerOptionsAction: sqlOperation =>
    {
        sqlOperation.EnableRetryOnFailure();
    });

});
builder.Services.AddScoped<ISubscriptionRepo, SubscriptionRepo>();

builder.Services.AddScoped<ITokenHandler, ET_ShiftManagementSystem.Servises.TokenHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDocumentServices, DocumentServices>();

builder.Services.AddScoped<ITaskServices, TaskServices>();

builder.Services.AddScoped<IAlertServices, AlertServices>();

builder.Services.AddScoped<IProjectUserRepository, ProjectUserRepository>();




builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    //reset token time span 1 hr 
    options.TokenLifespan = TimeSpan.FromHours(1);
});

//string domain = $"https://{Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        //options.Authority = builder.Domain;
        //options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });

//public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    app.UseMiddleware<TenantMiddleware>();
//}

//add session in web api 

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
});

//cors (cross origin request service we need when we access from front end application
//for single domine one url in origin , for two or more known domine we need to include , * for any domine (url)
builder.Services.AddCors(p => p.AddPolicy("CorePolicy", build =>
{
    build.WithOrigins("http://20.204.99.128/etapi/", "https://localhost:7259/", "http://127.0.0.1/etapi/")
    .AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

//middleware 
// Configure the HTTP request pipeline.

//session 
app.UseSession();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorePolicy");

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
