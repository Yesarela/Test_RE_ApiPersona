using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PersonaApi;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PersonaApi.Tests
{
    public class PersonaControllerTests
    {
        private readonly PersonasController _controller;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<DbSet<Persona>> _mockDbSet;

        public PersonaControllerTests()
        {
            // Crear un mock para el contexto de base de datos
            _mockContext = new Mock<ApplicationDbContext>();
            _mockDbSet = new Mock<DbSet<Persona>>();

            // Configurar el mock para que devuelva un conjunto de datos simulado
            _mockContext.Setup(m => m.Personas).Returns(_mockDbSet.Object);

            // Inicializar el controlador con el contexto mockeado
            _controller = new PersonasController(_mockContext.Object);
        }

        // Test para obtener todas las personas
        [Fact]
        public async Task GetPersonas_ReturnsOkResult_WithListOfPersonas()
        {
            // Arrange
            var personas = new List<Persona>
    {
        new Persona { id_persona = 1, primer_nombre = "Cliente", segundo_nombre = "Cliente", primer_apellido = "Cliente", segundo_apellido = "Cliente", tipo_documento = "Natural", documento = "Test", fecha_nacimiento = System.DateTime.Now, genero = "Cliente", status = true }
    };

            _mockDbSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(personas);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Persona>>>();

            // Verificar si el resultado es de tipo OkObjectResult
            var okResult = result.Result as OkObjectResult;  // Aquí verificamos el tipo exacto
            okResult.Should().NotBeNull();

            // Ahora podemos acceder al valor dentro de OkObjectResult
            var value = okResult.Value as IEnumerable<Persona>;
            value.Should().BeEquivalentTo(personas);
        }

        // Test para crear una nueva persona
        [Fact]
        public async Task CreatePersona_ReturnsCreatedAtActionResult_WithNewPersona()
        {
            // Arrange
            var persona = new Persona
            {
                primer_nombre = "Carlos",
                segundo_nombre = "Luis",
                primer_apellido = "Perez",
                segundo_apellido = "Lopez",
                tipo_documento = "DNI",
                documento = "12345678",
                genero = "Masculino"
            };

            _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Persona>(), default))
                .Returns((Persona p, System.Threading.CancellationToken ct) =>
                    new ValueTask<EntityEntry<Persona>>(Mock.Of<EntityEntry<Persona>>()));

            // Act
            var result = await _controller.Create(persona);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();  // Verificamos que el resultado sea del tipo esperado

            var createdAtActionResult = result as CreatedAtActionResult;  // Hacemos un cast a CreatedAtActionResult
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ActionName.Should().Be("GetPersonas");  // Verificamos que el nombre de la acción sea el esperado
        }

        // Test para actualizar una persona existente
        [Fact]
        public async Task UpdatePersona_ReturnsNoContent_WhenPersonaExists()
        {
            // Arrange
            var persona = new Persona { id_persona = 1, primer_nombre = "Carlos", segundo_nombre = "Luis" };

            _mockContext.Setup(m => m.Personas.Update(It.IsAny<Persona>())).Returns(new Mock<EntityEntry<Persona>>().Object);

            // Act
            var result = await _controller.Update(1, persona);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        // Test para eliminar una persona existente
        [Fact]
        public async Task DeletePersona_ReturnsOkResult_WhenPersonaExists()
        {
            // Arrange
            var persona = new Persona { id_persona = 1, primer_nombre = "Carlos", segundo_nombre = "Luis" };

            _mockContext.Setup(m => m.Personas.Remove(It.IsAny<Persona>())).Returns(new Mock<EntityEntry<Persona>>().Object);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
