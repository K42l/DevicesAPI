# DevicesAPI

A simple CRUD Web API for managing devices inventory.


## Overview

This project was created for a coding challenge, providing a simple Devices Inventory API. It exposes endpoints to create, read, update, and delete devices.\
Built with a clean architecture approach, using .NET 10, ASP.NET Core WebAPI, Entity Framework Core, and SQLite.\
The API documentation was created using OpenAPI and Scalar.


## Features

- CRUD for devices
- OpenAPI and Scalar for the API documentation
- SQLite for data persistence
- Domain tests using NUnit
- Clean Architecture (Domain, Application, Infrastructure, WebApi)


## Tech Stack

- .NET 10
- ASP.NET Core Web API
- OpenAPI and Scalar
- SQLite
- Entity Framework Core
- NUnit
- Docker


## Architecture

The project follows a simplified Clean Architecture structure:
```
Domain              -> Core business models and rules
Application         -> Interface
Infrastructure      -> EF Repository pattern and DB context
WebApi              -> Controllers, Migrations, Dependency injection and Models for requests.
Tests               -> NUnit domain tests
```
FYI: I know I shoudn't. But I used the domain's model for the Infrasctructure and the WebAPI due to the time I had avaliable.

## Running the Application

Just clone and run:
```
git clone https://github.com/K42l/DevicesAPI.git
cd DevicesAPI/DevicesApi.WebApi
dotnet run
```
When the app starts, if the SQLite file doesn't exists, the database is created and populated with 3 entries.\
The OpenAPI, Scalar and the redirect from root to /docs are only available in the development environment.


## API Documentation

Once the application is running, visit:
```
http://localhost:<port>/
```
You'll be automatically redirected to `/docs`.


## Database

- SQLite
- EF Core handles migrations and schema creation.


## Testing

Run tests with:
```
cd DevicesAPI/DevicesApi.Tests
dotnet test
```
Due to the time I had available, I was only able to create the tests for the domain.


## Docker

```
docker build -t devicesapi .
docker run -p 8080:8080 devicesapi
```


## License

MIT License.