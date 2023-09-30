using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
//Forma de autenticação.
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "JwtBearer";
    option.DefaultChallengeScheme = "JwtBearer";
})
// Parâmetros de validação do token.
.AddJwtBearer("JwtBearer", option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        //validar quem está solicitando.
        ValidateIssuer = true,
        //validar quem está recebendo.
        ValidateAudience = true,
        //Define se o tempo de expiração será validado.
        ValidateLifetime = true,
        //Criptografia e validação da chave autenticação.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        //valida o tempo de expiração do token.
        ClockSkew = TimeSpan.FromSeconds(30),
        //nome do issuer, da origem.
        ValidIssuer = "exoapi.webapi",
        //nome do audience, para o destino.
        ValidAudience = "exoapi.webapi"
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();
//habilita a autenticação.
app.UseAuthentication();

//habilita a autorização.
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
