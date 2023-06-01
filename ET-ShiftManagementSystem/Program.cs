using Microsoft.EntityFrameworkCore;
using Servises.ProjectServises;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Services;
using Serilog;

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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ET_ShiftManagementSystem", Version = "v1" , Description ="Shift management system is basicaly ment For Devops engineers ",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "sales@euphoricthought.com",
            Url = new Uri("https://www.euphoricthought.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
        },
    });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "ETSMS.xml");


    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, filePath));

});
//(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
//    var filePath = Path.Combine(System.AppContext.BaseDirectory, "new.xml");


//    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, filePath));
//});
builder.Services.AddHttpLogging(HttpLogging =>
{
    HttpLogging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

var _logger = new LoggerConfiguration().WriteTo.File("C:\\ET-ShiftManagementSystem\\Logs\\APILogs-", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Logging.AddSerilog(_logger);

builder.Services.AddControllers()
    .AddNewtonsoftJson();


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

builder.Services.AddScoped<IProjectServices, ProjectServices>();

builder.Services.AddScoped<IRoleServices , RoleServices>();

builder.Services.AddScoped<IOrganizationRoleServices, OrganizationRoleServices>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<INotesServices, NotesServices>();

builder.Services.AddScoped<ITaskCommentServices , TaskCommentServices>();

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

builder.Services.AddScoped<ITasksServices,  TasksServices>();




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
//builder.Services.AddCors(p => p.AddPolicy("CorePolicy", build =>
//{
//    build.WithOrigins("http://20.204.99.128/etapi/", "http://localhost:5173/")
//    .AllowAnyMethod().AllowAnyHeader();
//}));
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .WithExposedHeaders("Authorization"); // Add this line to allow the Authorization header
//    });
//});
//builder.Services.AddCors(p => p.AddPolicy("CorePolicy", build =>
//{
//    build.WithOrigins("*")
//    .AllowAnyMethod().AllowAnyHeader();
//}));
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        builder =>
//        {
//            builder.WithOrigins("http://20.204.99.128/etapi", "http://localhost:5173")
//                                .AllowAnyHeader()
//                                .AllowAnyMethod();
//        });
//});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorePolicy", builder =>
//    {
//        builder.AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

builder.Services.AddCors(p => p.AddPolicy("CorePolicy", build =>
{
    build.WithOrigins("http://20.204.99.128/etapi", "http://20.204.99.128/SMS", "http://localhost:5173")
    .AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Authorization");
}));
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.WithOrigins("http://20.204.99.128/etapi", "http://20.204.99.128/SMS", "http://localhost:5173")
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .WithExposedHeaders("Authorization"); // Add this line to allow the Authorization header
//    });
//});
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

app.UseHttpLogging();


app.UseRouting();
//app.UseCors();
app.UseCors("CorePolicy");
//app.UseCors(x => x
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .SetIsOriginAllowed(_ => true)
//            .AllowCredentials());
//app.UseHttpsRedirection();
//app.UseCors();
//app.UseCors("CorePolicy");
//app.UseCors(builder =>
//{
//    builder
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader();
//});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
