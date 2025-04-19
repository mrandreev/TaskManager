# Task Management System

This project is a **Task Management System** built with modern architectural practices: **Clean Architecture**, **Domain-Driven Design (DDD)**, **CQRS**, and asynchronous messaging using **RabbitMQ**. It is designed to demonstrate **SOLID** principles, **MediatR** integration, and event-driven workflows in a real-world **.NET 8** application.

---

## 🚀 Features

- ✅ Task creation, status updates, and listing via Web API
- ✅ Domain events with clean domain models
- ✅ CQRS structure with separate Commands, Queries, and Notifications
- ✅ Domain events and dispatching using MediatR
- ✅ RabbitMQ-based message bus and background listener for handling events
- ✅ PostgreSQL via EF Core (code-first)
- ✅ Containerized with Docker

---

## ▶️ How to Run

You can run the application in two ways:

#### 🐳 Option 1: Run via Docker (Recommended)

This will start:
- ASP.NET Core Web API
- RabbitMQ (with management UI)
- PostgreSQL database

```bash
docker-compose up --build
```

After startup:
- Swagger UI → http://localhost:8080/swagger
- RabbitMQ UI → http://localhost:15672 (guest / guest)
- PostgreSQL → localhost:5432 (postgres / postgres / taskmanagerdb)

#### 💻 Option 2: Run Locally via `dotnet run`

1. Start RabbitMQ and PostgreSQL manually (or via Docker):
2. Run the API:

```bash
dotnet run --project src/TaskManager.Api
```

Make sure your `appsettings.json` or user secrets contain:

```json
"RabbitMQ": {
  "HostName": "localhost",
  "Port": 5672,
  "UserName": "guest",
  "Password": "guest",
  "VirtualHost": "/"
}
```

## 🧱 Architecture Overview

- **TaskManager.Api**            → Web API, Swagger, and DI wiring
- **TaskManager.Application**    → CQRS, domain events, messaging contracts, MediatR handlers
- **TaskManager.Domain**         → Domain entities and logic
- **TaskManager.Infrastructure** → RabbitMQ message bus and background message listeners
- **TaskManager.Persistence**    → EF Core context and repository implementations
- **TaskManager.Core**           → Shared module with base models, exceptions, and validation contracts

## ⚠️ Trade-offs & Limitations

- No retry policies handling for failed messages
- Queue names are based on message type name, which may reduce flexibility
- Message processing is sequential by default; parallel handling requires additional app logic and RabbitMQ configurations