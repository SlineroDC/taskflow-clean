# Documento de Diseño de Software - TaskFlow
## 1. Introducción del Sistema
El presente documento describe el diseño arquitectónico e ingenieril para TaskFlow, una plataforma integral de gestión de proyectos y tareas. El objetivo principal es construir una aplicación monolítica estructurada bajo los principios de Arquitectura Limpia (Clean Architecture). La solución está diseñada para operar inicialmente a través de una interfaz web nativa, pero expone simultáneamente endpoints RESTful para integraciones futuras, garantizando que ambas vías de acceso consuman exactamente la misma lógica de negocio centralizada.

## 2. Requisitos del Sistema

Requisitos Funcionales:

- Gestión de Proyectos: Creación, lectura, actualización, eliminación (CRUD), cambio de estado (activar/completar) y generación de resumen (/summary).

- Gestión de Tareas: Creación, eliminación, cambio de estado (completar) y reordenamiento de prioridad.

- Reglas de Negocio Estrictas: Un proyecto requiere al menos una tarea para ser activado; todas las tareas deben estar completadas para completar el proyecto; el orden de las tareas debe ser único por proyecto y no generar duplicados al reordenar.

Requisitos No Funcionales:

- Desarrollado en .NET 10.

- Interfaz visual mediante ASP.NET Core MVC con vistas Razor.

- Exposición de API RESTful.

- Base de datos relacional (PostgreSQL) utilizando Entity Framework Core.

- Cobertura mínima de 5 pruebas unitarias sobre la capa de aplicación/dominio.

3. Arquitectura del Software
Se implementará una Arquitectura Limpia (Clean Architecture). Esta decisión arquitectónica se toma para cumplir la restricción obligatoria del proyecto: "La aplicación web NO debe consumir la API vía HTTP".
El flujo de dependencia fluirá hacia adentro:

1. Capa de Presentación (TaskFlow.Presentation.WebMVC y TaskFlow.Presentation.WebApi) dependen de la Capa de Aplicación. Ambas instancian los mismos servicios a través de Inyección de Dependencias.

2. Capa de Infraestructura (TaskFlow.Infrastructure) depende de la Capa de Aplicación (implementando sus interfaces) y de la Capa de Dominio.

3. Capa de Aplicación (TaskFlow.Application) depende únicamente del Dominio.

4. Capa de Dominio (TaskFlow.Domain) es el núcleo central y no tiene dependencias externas.

## 4. Descripción de Módulos

TaskFlow.Domain: Contiene las entidades principales (Project, TaskItem) y las reglas de validación inherentes a los datos puros.

TaskFlow.Application: Orquesta los casos de uso. Aquí se validarán las reglas de negocio críticas definidas en los requerimientos.

TaskFlow.Infrastructure: Gestiona la persistencia de datos con Entity Framework Core y la configuración del contexto de la base de datos.

TaskFlow.Presentation.WebMVC: Provee la interfaz de usuario interactiva manejando peticiones HTTP tradicionales que devuelven vistas HTML.

TaskFlow.Presentation.WebApi: Provee las mismas funcionalidades de negocio mediante respuestas en formato JSON para clientes externos.

5. Justificación Técnica

Patrón Arquitectónico: La separación en capas garantiza que las reglas de negocio sean agnósticas a la interfaz. Esto evita la duplicación de código y asegura que un error de lógica se corrija en un solo lugar, beneficiando tanto a la web como a la API.

.NET 10 & C#: Proporciona un entorno robusto, fuertemente tipado y de alto rendimiento.

Inyección de Dependencias (DI): Es el pilar fundamental para desacoplar los controladores de la lógica de negocio, facilitando el mantenimiento y la implementación de las pruebas unitarias requeridas mediante el aislamiento de componentes.