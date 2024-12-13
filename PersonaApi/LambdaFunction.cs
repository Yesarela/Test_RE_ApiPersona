using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;

public class LambdaFunction
{
    public void Init(IHostBuilder builder)
    {
        builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                // Configuración de la conexión a la BD
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        new MySqlServerVersion(new Version(8, 0, 31))
                    )
                );

                // Registrar MediatR
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LambdaFunction).Assembly));

                // Configuración de Swagger
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();

                // Agregar controladores
                services.AddControllers();
            });

            webBuilder.Configure(app =>
            {
                var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                // Middleware
                if (env.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseAuthorization();

                // Mapear controladores en el contexto de enrutamiento
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        });
    }
}
