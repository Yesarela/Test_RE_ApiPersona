using System.ComponentModel.DataAnnotations;

public class Persona
{
    [Key]
    public int id_persona { get; set; }

    [Required]
    [MaxLength(200)]
    public string primer_nombre { get; set; }

    [Required]
    [MaxLength(200)]
    public string segundo_nombre { get; set; }

    [Required]
    [MaxLength(200)]
    public string primer_apellido { get; set; }

    [Required]
    [MaxLength(200)]
    public string segundo_apellido { get; set; }

    [Required]
    public string tipo_documento { get; set; } // Cédula, RUC, Pasaporte

    [Required]
    [MaxLength(50)]
    public string documento { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime fecha_nacimiento { get; set; }

    [Required]
    public string genero { get; set; } // Masculino, Femenino

    public bool status { get; set; } = true;
}