using exoModelsProjet.Models;
using LearnPlay.repository;

namespace Learn_Play
{
    internal class Program
    {
        static void Main(string[] args)
        {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen();

            // DI (pour ne pas faire les implementations  non utilse car je ne les injecte pas
            builder.Services.AddScoped<Utilisateurs, UtilisateursRepo>();
            builder.Services.AddScoped<Profils, ProfilsRepo>();

            //  Services pour rattacher au front (CORS) 1/2
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("GestionTachesFront", policy =>
                    policy
                        .WithOrigins("http://localhost:5510") // si tu sers ton HTML sur ce port
                        .AllowAnyOrigin()   // en dev: simple et efficace mais attention en prod
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });


            var app = builder.Build();

            // Configure Swagger.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionTaches API v1");
                c.RoutePrefix = "swagger";  //c.RoutePrefix = string.Empty = ouvres directement https://localhost:PORT/
            });

            // Pour rattacher au front (CORS) 2/2
            app.UseCors("GestionTachesFront");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
