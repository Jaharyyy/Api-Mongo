    using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI.DATAS;
using WebAPI.DATAS.Security;
using WebAPI.MODEL.Configuration;
using WebAPI.MODEL.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddSingleton<ProductRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//INYECCION DE LOS REPOSITORIOS 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//prueba de codigo
builder.Services.AddCors(options =>
{
    options.AddPolicy("Corspolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


//Configuración de tipos roles de la aplicación
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Empleado", policy => policy.RequireRole("Empleado"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});



//Configuración de la integración y validación de Autorización
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = $@"JWT Authorization header using the Bearer scheme. +
                       \r\n\r\n Enter prefix (Bearer), space, and then your token.
                       Example: 'Bearer 1a234ert789fgcv%4578...'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
    });
});

//Inyectando el modelo JwtConfig y le estamos cargando el objeto JwtConfig del archivo appsettings.json
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var secret = builder.Configuration.GetSection("JwtConfig:Secret").Value;


builder.Services.AddControllers();



//AddAuthentication permite agregar y personalizar los permisos que se requieren para autenticar
builder.Services.AddAuthentication(options =>
{
    //Definir los esquemas de autenticacion estandar/por defecto de la API
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
)
    //Agregar las opciones del Jwt
    .AddJwtBearer(jwt =>
    {

        //Indica que el token debe guardarse una vez que la autenticacion fue verdadera
        jwt.SaveToken = true;

        //Configuracion de parametros de validacion del token
        jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            //Asegurar que la key del token siempre sea validada
            ValidateIssuerSigningKey = true,


            //Asegurar que no hubo manipulacion del token desde que lo emitio su emisor
            ValidateIssuer = false,

            //Validar el destinatario para el cual el token fue creado
            ValidateAudience = false,

            //Valida el tiempo de vida del token
            ValidateLifetime = true,

            //Tiempo de expiracion del token
            RequireExpirationTime = true
        };
    }
 );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterWeb",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("Corspolicy");
//app.UseHttpsRedirection();

app.UseCors("AllowFlutterWeb");

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();






