# WebEnablePackage

WebEnablePackage is a C# Blazor application (includes a WebAssembly client) that provides user registration for a job agency.

## Supported Platforms & Targets
- .NET 9 (primary)
- .NET 8 (some projects may target this)
- Windows is fully supported. Docker images are portable to Linux when containers are built on compatible base images.
- Visual Studio 2022 and the dotnet CLI are supported for development.

## Prerequisites
- .NET SDK 9.0 (or appropriate runtime installed)
- Visual Studio 2022 (recommended) or latest VS Code
- Docker Desktop (optional, for container builds)

## Quick Start (Local)
1. Clone the repository:
   git clone <repo-url>
2. From the solution root restore and build:
   dotnet restore
   dotnet build
3. Run the Blazor application:
   - Visual Studio 2022: Open the solution and run the startup project (likely the Server or Client project).
   - dotnet CLI: Run the project directly:
     dotnet run --project <path-to-startup-project>
   Replace `<path-to-startup-project>` with your client or hosted server project path (for hosted Blazor WebAssembly solutions run the Server project).

## Running with Docker
1. Ensure Docker Desktop is running.
2. Publish and build an image (example):
   dotnet publish -c Release -o ./publish
   docker build -t webenablepackage:latest .
3. Run the container:
   docker run -p 5000:80 --env ASPNETCORE_ENVIRONMENT=Production webenablepackage:latest
4. Open http://localhost:5000

(If your solution uses multiple services or a database container, consider adding a docker-compose file to orchestrate them.)

## Configuration
- Connection strings and environment-specific settings are in `appsettings.json` and `appsettings.Development.json`.
- When running in Docker, prefer environment variables for secrets and connection strings.

Example connection string environment variable usage:
ASPNETCORE_ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=..."

## Troubleshooting / Common Issues
- If you see "Cannot access a disposed object. Object name: 'System.Net.Http.HttpClient'." ensure your Blazor components use the IHttpClientFactory or the DI-injected HttpClient provided by Blazor (do not create and dispose HttpClient manually inside components).
- For Blazor WebAssembly, prefer the injected HttpClient registered by the host rather than creating new instances.
- When debugging, watch component lifecycle: avoid disposing shared services in component Dispose methods.

## Contributing
Pull requests are welcome. For major changes, open an issue first.

## TODO
- Add Docker Compose for local multi-container setup
- Allow users to delete records outside of direct database connection (for cleaning up data without accessing the DB directly))
- Improve docs for running hosted WebAssembly vs. standalone
- Review and fix any transient bugs with forms (e.g., JobAgencyForm add/save issues)

## License
Not currently licensed.