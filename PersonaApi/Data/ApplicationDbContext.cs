using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Persona> Personas { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Método para agregar persona
    public async Task<Persona> AgregarPersonaAsync(Persona persona)
    {
        var result = await Personas
            .FromSqlRaw("CALL sp_agregar_persona({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                persona.primer_nombre,
                persona.segundo_nombre,
                persona.primer_apellido,
                persona.segundo_apellido,
                persona.tipo_documento,
                persona.documento,
                persona.fecha_nacimiento,
                persona.genero)
            .ToListAsync();

        return result.FirstOrDefault();
    }


    // Método para actualizar persona
    public async Task<Persona> ActualizarPersonaAsync(Persona persona)
    {
        var result = await Personas
            .FromSqlRaw("CALL sp_actualizar_persona({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                persona.id_persona,
                persona.primer_nombre,
                persona.segundo_nombre,
                persona.primer_apellido,
                persona.segundo_apellido,
                persona.tipo_documento,
                persona.documento,
                persona.fecha_nacimiento,
                persona.genero)
            .ToListAsync();

        return result.FirstOrDefault();
    }


    // Método para eliminar persona
    public async Task<Persona> EliminarPersonaAsync(int id)
    {
        var result = await Personas
            .FromSqlRaw("CALL sp_eliminar_persona({0})", id)
            .ToListAsync();

        return result.FirstOrDefault();
    }

    // Método para listar persona
    public async Task<List<Persona>> ListarPersonasAsync(int? id)
    {
        return await Personas
            .FromSqlRaw("CALL sp_listar_persona({0})", id.HasValue ? id.Value : DBNull.Value)
            .ToListAsync();
    }

}