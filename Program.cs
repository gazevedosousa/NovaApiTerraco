

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TerracoDaCida.DTO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        List<SecurityKey> securityKeys = new List<SecurityKey>();

        var publicJwk = new JsonWebKey
        {
            KeyId = builder.Configuration["Tkn:keys:kid"],
            Alg = builder.Configuration["Tkn:keys:alg"],
            E = builder.Configuration["Tkn:keys:e"],
            N = builder.Configuration["Tkn:keys:n"],
            Kty = builder.Configuration["Tkn:keys:kty"],
            Use = builder.Configuration["Tkn:keys:use"],
        };
        securityKeys.Add(publicJwk);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = builder.Configuration.GetSection("Tkn:issuers").AsEnumerable().Select(s => s.Value),
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            IssuerSigningKey = publicJwk,
            ClockSkew = TimeSpan.FromSeconds(20)
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                var error = new MensagemDTO { Mensagem = "Token Inválido", Codigo = 401 };
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(error, new JsonSerializerOptions { IgnoreNullValues = true })));
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback = 
        (httpRequestMessage, cert, cetChain, policyErrors) =>
        {
            return true;
        }
});

//Serviços
//TODO
//Contexto de Banco de Dados
//TODO   

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
     );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Terraço da Cida", Version = "v1" });
        //var xmlFile = $"SwaggerAnnotations.xml";
        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //c.IncludeXmlComments(xmlPath);
    }
 );

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Terraço da Cida");
});

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandler = (async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var mensagem = exceptionHandlerPathFeature?.Error?.Message;
        string ex = null;
        string[] listaMensagem = mensagem == null ? new string[0] : mensagem.Split("|");
        int codigo = 500;
        if (listaMensagem.Length == 2)
        {
            codigo = Convert.ToInt32(listaMensagem[0]);
            mensagem = listaMensagem[1];
        }
        else
        {
            if (app.Environment.IsDevelopment())
            {
                ex = exceptionHandlerPathFeature?.Error?.ToString();
            }
            mensagem = "Erro Interno";
        }
        var error = new MensagemDTO { Mensagem = mensagem, Codigo = codigo, Exception = ex };
        context.Response.StatusCode = codigo;
        context.Response.ContentType = "application/json";
        context.Response.WriteAsync(Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(error, new JsonSerializerOptions { IgnoreNullValues = true })));
    }),
    AllowStatusCode404Response = true
});

app.UseCors("AllowAny");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
