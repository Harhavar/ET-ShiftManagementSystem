using ShiftMgtDbContext.Data;
using Microsoft.EntityFrameworkCore;
using Servises.ProjectServises;
using Microsoft.EntityFrameworkCore.SqlServer;
using ShiftManagementServises.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ShiftMgtDbContext.Entities;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

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

builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShiftManagementDbContext>().AddDefaultTokenProviders();

//for Authentication 
//builder.Services.AddAuthentication(option =>
//       {
//           option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//           option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//           option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//       }).AddJwtBearer(options =>
//       {
//           options.TokenValidationParameters = new TokenValidationParameters
//           {
//               ValidateIssuer = true,
//               ValidateAudience = true,
//               ValidateLifetime = true,
//               ValidateIssuerSigningKey = true,
//               ValidIssuer = builder.Configuration["Jwt:Issuer"],
//               ValidAudience = builder.Configuration["Jwt:Audience"],
//               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//           };
//       });





//var JwtSetting = System.Configuration.GetSection("Jwt");
//builder.Services.Configure<JwtBearerDefaults>(JwtSetting);


//for entity framework
builder.Services.AddDbContext<ShiftManagementDbContext>(option => option.UseSqlServer
(builder.Configuration.GetConnectionString("ProjectAPIConnectioString")));



builder.Services.AddScoped<ITokenHandler, ShiftManagementServises.Servises.TokenHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateIssuerSigningKey = true,
    ValidateLifetime = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
