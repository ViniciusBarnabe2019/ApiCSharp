using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

//Builder Recebe um Objeto de Aplicação Web
var builder = WebApplication.CreateBuilder(args);

//Adiciona nossas Controlles na API
builder.Services.AddControllers();

//Coleta Infos dos EndPoints como Metodos, Respontas, etc
//(Usado em Conjunto com Swagger)
builder.Services.AddEndpointsApiExplorer();

//Adiciona o Swagger (Analisador de Controllers/Endpoints) a nossa API
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    //bloco de codigo da função lambda
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters { 
        ValidateIssuer = false, 
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, 
        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => {
            if(kid == "1234"){
                var listaChaves = new List<SecurityKey> { new SymmetricSecurityKey(Convert.FromBase64String("aGVsbG8gd29ybGQgdGhpcyBpcyBhIHNlY3JldCBrZXk=")) };
                return listaChaves;
            }
            return null;
        }
    };
});

//Cria nosso Web App já Configurado
var app = builder.Build();

//Se estiver em Modo de Desenvolvimento/Criação/Testes
if(app.Environment.IsDevelopment()){
    //Habilite a Geraçao de Documentaçao 
    app.UseSwagger();
    //Crie uma GUI que Exiba a Documentação
    app.UseSwaggerUI();
}

//Redireciona todas Solicitações para HTTPS
//app.UseHttpsRedirection();

//AcessoMiddleware na 1º Posição do Pipeline
app.UseMiddleware<AcessoMiddleware>();

//Obriga o Uso de Autenticação pra Consumir a API
app.UseAuthentication();

//Obriga o Uso de Autorização pra Consumir a API
app.UseAuthorization();

//Prepara os Controllers da API pra Receberem e Responderem Solicitações
app.MapControllers();

app.Run();
