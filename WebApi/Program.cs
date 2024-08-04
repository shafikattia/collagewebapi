using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Models;
using WebApi.Sevices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ItiDbContext>(options => options.
UseSqlServer(@"server = DESKTOP-DR5LS6Q ;
 Initial catalog = DbWebApi;
Integrated security = true ;
 Encrypt = True; Trust Server Certificate = True"));

builder.Services.AddCors(CorsOptions => {
    CorsOptions.AddPolicy("MyFirstPolicty", CorsPolicyBuilder =>
{
    CorsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ItiDbContext>();


builder.Services.AddAuthentication(Options=>
{
    Options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme    =JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultScheme             =JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options=>
{
    Options.SaveToken = true;
    Options.RequireHttpsMetadata = false;
    var jwtSettings = builder.Configuration.GetSection("JWT");
    Options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidateAudience=true,
        ValidAudience= jwtSettings["ValidAudience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),

};
}
    );

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "colloge", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});



builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}
app.UseStaticFiles(); //html pages , images
app.UseCors("MyFirstPolicty");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
