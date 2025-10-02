using LearnPlay.Interfaces;
using LearnPlay.Metiers;
using LearnPlay.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LearnPlay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();   // <— important
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnPlay API", Version = "v1" });

                // Déclare DateOnly / TimeOnly comme string "date"/"time"
                c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
                c.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = "time" });

                // (Optionnel) XML comments si tu veux les descriptions depuis le code
                // var xml = Path.Combine(AppContext.BaseDirectory, "LearnPlay.xml");
                // c.IncludeXmlComments(xml, includeControllerXmlComments: true);
            });


            // Repos
            builder.Services.AddScoped<IProfilsRepository, ProfilsRepo>();
            builder.Services.AddScoped<IUtilisateursRepository, UtilisateursRepo>();

            // Métier (services)
            builder.Services.AddScoped<IProfilsMt, ProfilsMt>();
            builder.Services.AddScoped<IUtilisateursMt, UtilisateursMt>();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Front", policy =>
                    policy
                        .WithOrigins("http://127.0.0.1:5510", "http://localhost:5511")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                // .AllowCredentials()  // décommente si tu envoies cookies/Authorization cross-site
                );
            });

            var app = builder.Build();

            // Swagger (généralement réservé au dev)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnPlay API v1");
                    c.RoutePrefix = "swagger"; // l’UI sera sur /swagger
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("Front");
            app.UseAuthorization();

            app.MapControllers();

            // SPA fallback: toute route inconnue renvoie index.html
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
