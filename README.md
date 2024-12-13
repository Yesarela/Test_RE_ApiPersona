# Persona API

## Descripcin

API REST desarrollada en **.NET 8** con CRUD para gestionar un catálogo de personas.

## Características

- **Base de datos:** MySQL
- **ORM:** Entity Framework Core
- **Documentación:** Swagger
- **Patrones:** CQRS y Mediator
- **Despliegue:** AWS Lambda ó Docker
- **Pruebas TDD:** Se incluye pruebas dentro del proyecto

## Requisitos

- Visual Studio 2022
- .NET 8 SDK
- MySQL
- Docker

## Ejecución del Proyecto

La ejecución del proyecto se realiza de manera más simple al abrir la solución `PersonaApi.sln` en Visual Studio 2022. Aunque de igual forma se detalla el paso a paso si se desea realizar desde terminal.

### 1. Configura la cadena de conexión en `appsettings.json` dentro del proyecto `PersonaApi`.

#### 1.1 Ejecución de Base de Datos:

En caso tal de querer crear la Base de Datos en una nueva BD, se incluye el script de la misma:

<pre style="max-height: 300px; overflow-y: scroll;">
CREATE DATABASE db_persona;
USE db_persona;

/*TABLA*/
CREATE TABLE cat_persona(
id_persona					INT AUTO_INCREMENT PRIMARY KEY,
primer_nombre				VARCHAR(200) NOT NULL,
segundo_nombre				VARCHAR(200) NOT NULL,
primer_apellido				VARCHAR(200) NOT NULL,
segundo_apellido			VARCHAR(200) NOT NULL,
tipo_documento				VARCHAR(40) NOT NULL,
documento					VARCHAR(50) NOT NULL,
fecha_nacimiento			DATE NOT NULL,
genero						VARCHAR(40) NOT NULL,
`status`					BIT NOT NULL DEFAULT 1
); END

 /*CRUD PERSONA*/

 -- Insertar
DELIMITER $$
CREATE PROCEDURE sp_agregar_persona(IN _1 VARCHAR(200), IN _2 VARCHAR(200), IN _3 VARCHAR(200), IN _4 VARCHAR(200), IN _5 VARCHAR(40), IN _6 VARCHAR(50), IN _7 DATE, IN _8 VARCHAR(40))
BEGIN
	INSERT INTO cat_persona(primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, tipo_documento, documento, fecha_nacimiento, genero)
		VALUES (_1, _2, _3, _4, _5, _6, _7, _8);

    SELECT * FROM cat_persona WHERE id_persona = LAST_INSERT_ID();
END;
$$

-- Actualizar
DELIMITER $$
CREATE PROCEDURE sp_actualizar_persona(IN _1 INT, IN _2 VARCHAR(200), IN _3 VARCHAR(200), IN _4 VARCHAR(200), IN _5 VARCHAR(200), IN _6 VARCHAR(40), IN _7 VARCHAR(50), IN _8 DATE, IN _9 VARCHAR(40))
BEGIN
	UPDATE cat_persona SET
		primer_nombre = _2,
        segundo_nombre = _3,
        primer_apellido = _4,
        segundo_apellido = _5,
        tipo_documento = _6,
        documento = _7,
        fecha_nacimiento = _8,
        genero = _9
    WHERE  id_persona = _1;

    SELECT * FROM cat_persona WHERE id_persona = _1;
END;
$$

-- Eliminar
DELIMITER $$
CREATE PROCEDURE sp_eliminar_persona(IN _1 INT)
BEGIN
	UPDATE cat_persona SET
		`status` = 0
    WHERE  id_persona = _1;

    SELECT * FROM cat_persona WHERE id_persona = _1;
END;
$$

-- Seleccionar
DELIMITER $$
CREATE PROCEDURE sp_listar_persona(IN _1 INT)
BEGIN
	IF (_1 IS NULL)
    THEN
		SELECT * FROM cat_persona WHERE `status` = 1;
    ELSE
		SELECT * FROM cat_persona WHERE id_persona = _1;
	END IF;
END;
$$

-- Init Data / Test CRUD
CALL sp_agregar_persona ('Cliente', 'Cliente', 'Cliente', 'Cliente', 'Natural','Test', '1990/01/30', 'Otro');
CALL sp_agregar_persona ('Yesarela', 'de Jesús', 'Fariña', 'López', 'Natural','010-01011990', '1990/01/30', 'Femenino');
CALL sp_actualizar_persona(2, 'Yesarela', 'de Jesús', 'Fariña', 'López', 'Natural','010-01011990-0101A', '1990/01/30', 'Femenino');
-- CALL sp_eliminar_persona (1);
CALL sp_listar_persona (null);

</pre>


### 2. Restaura los paquetes:

```
dotnet restore
```

### 3. Construir el proyecto:

```
dotnet build
```

### 4. Ejecución del proyecto:

```
cd PersonaApi           # nos dirigimos al proyecto principal
dotnet run              # Ejecutamos el proyecto
```

### 5. Dockerizar solución:

El proyecto cuenta con una receta Docker la cual permitirá crear una imagen para ejecución, para ello debemos ejecutar lo siguiente en caso tal de desear usar la solución como Docker:

```
docker build -t PersonaApi .                       # Construimos la imagen
docker run -d -p 8080:80 PersonaApi                # Ejecutamos la imagen con el tag que definimos
```

### 6. Curls:

#### 6.1 Crear Persona

```
curl --location 'https://localhost:7262/api/Personas' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data '{
  "primer_nombre": "Test nombre 1",
  "segundo_nombre": "Test nombre 2",
  "primer_apellido": "Test apellido 1",
  "segundo_apellido": "Test apellido 2",
  "tipo_documento": "Cedula",
  "documento": "000148450",
  "fecha_nacimiento": "1990-12-12",
  "genero": "Masculino"
}'
```

#### 6.2 Actualizar Persona

```
curl -X 'PUT' \
  'https://localhost:7262/api/Personas/1' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "id_persona": 1,
  "primer_nombre": "string",
  "segundo_nombre": "string",
  "primer_apellido": "string",
  "segundo_apellido": "string",
  "tipo_documento": "string",
  "documento": "string",
  "fecha_nacimiento": "2024-12-13",
  "genero": "string",
  "status": true
}'
```

#### 6.3 Listar Persona por:

##### ID

```
curl -X 'GET' \
  'https://localhost:7262/api/Personas/1' \
  -H 'accept: text/plain'
```

#### Obtener todas

```
curl -X 'GET' \
  'https://localhost:7262/api/Personas/' \
  -H 'accept: text/plain'
```

#### 6.4 Eliminar Persona

```
curl -X 'DELETE' \
  'https://localhost:7262/api/Personas/1' \
  -H 'accept: */*'
```