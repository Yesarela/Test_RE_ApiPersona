using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Controlador para gestionar las operaciones relacionadas con las personas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PersonasController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Crea una nueva persona en la base de datos.
    /// Utiliza un procedimiento almacenado para agregar una persona.
    /// </summary>
    /// <param name="persona">El objeto de la persona a agregar.</param>
    /// <returns>Una respuesta HTTP 201 (Creado) si la persona es creada correctamente.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Persona), 201)]  // Respuesta en caso de creación exitosa
    [ProducesResponseType(400)]  // Respuesta si hay un error en la solicitud
    public async Task<IActionResult> Create([FromBody] Persona persona)
    {
        // Llama al método para agregar la persona a la base de datos usando SP.
        var result = await _context.AgregarPersonaAsync(persona);

        // Devuelve una respuesta HTTP 201 (Creado), incluyendo la URL para acceder a la persona recién creada.
        return CreatedAtAction(nameof(GetAll), new { id = result.id_persona }, result);
    }

    /// <summary>
    /// Obtiene una lista de personas. Si se proporciona un ID, devuelve una sola persona, 
    /// de lo contrario, devuelve todas las personas.
    /// </summary>
    /// <param name="id">El ID de la persona a buscar. Si es nulo, devuelve todas las personas.</param>
    /// <returns>Una lista de personas si el ID es nulo o la persona correspondiente al ID proporcionado.</returns>
    [HttpGet("{id?}")]
    [ProducesResponseType(typeof(IEnumerable<Persona>), 200)]  // Documenta la respuesta de tipo lista de personas
    [ProducesResponseType(404)]  // Respuesta en caso de que no se encuentre la persona
    public async Task<ActionResult<IEnumerable<Persona>>> GetAll([FromRoute] int? id = null)
    {
        // Llama al procedimiento almacenado para obtener las personas o una persona por ID.
        var result = await _context.ListarPersonasAsync(id);

        // Devuelve la lista de personas o la persona por ID.
        return Ok(result);
    }

    /// <summary>
    /// Actualiza los datos de una persona en la base de datos.
    /// Utiliza un procedimiento almacenado para realizar la actualización.
    /// </summary>
    /// <param name="id">El ID de la persona a actualizar.</param>
    /// <param name="persona">El objeto con los datos actualizados de la persona.</param>
    /// <returns>Una respuesta HTTP 200 (OK) si la actualización es exitosa.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]  // Respuesta en caso de que la actualización sea exitosa
    [ProducesResponseType(400)]  // Respuesta en caso de que el ID no coincida con el objeto persona
    [ProducesResponseType(404)]  // Respuesta si no se encuentra la persona
    public async Task<IActionResult> Update(int id, [FromBody] Persona persona)
    {
        // Verifica si el ID de la persona en la ruta coincide con el ID del objeto persona.
        if (id != persona.id_persona)
        {
            return BadRequest("El ID de la persona no coincide con el ID proporcionado.");
        }

        // Llama al método para actualizar la persona en la base de datos usando SP.
        var result = await _context.ActualizarPersonaAsync(persona);

        // Si la persona no existe, devuelve un 404 (No encontrado).
        if (result == null)
        {
            return NotFound("Persona no encontrada.");
        }

        // Devuelve una respuesta HTTP 200 (OK) con el resultado de la actualización.
        return Ok(result);
    }

    /// <summary>
    /// Elimina una persona de la base de datos utilizando el ID proporcionado.
    /// </summary>
    /// <param name="id">El ID de la persona a eliminar.</param>
    /// <returns>Una respuesta HTTP 200 (OK) si la eliminación es exitosa.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]  // Respuesta en caso de que la eliminación sea exitosa
    [ProducesResponseType(404)]  // Respuesta si no se encuentra la persona a eliminar
    public async Task<IActionResult> Delete(int id)
    {
        // Llama al método para eliminar la persona de la base de datos usando SP.
        var result = await _context.EliminarPersonaAsync(id);

        // Si no se encontró el resultado, retorna un 404 (No encontrado).
        if (result == null)
        {
            return NotFound("Persona no encontrada.");
        }

        // Devuelve una respuesta HTTP 200 (OK) con el resultado de la eliminación.
        return Ok(result);
    }
}
