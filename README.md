# psw-be-ra-2024-group-8

An ASP.NET Core (.NET 7, C#) modular backend for the Explorer gamified tourist platform.  
It provides REST APIs for tours, tour checkpoints, encounters, secrets, tourist profiles (XP & level progression), and level‑gated challenge/review flows consumed by the Angular frontend.

This repository is a mirror of the original collaborative backend. The service is composed of domain modules (Tours, Encounters, Stakeholders, Payments, Blog, Games) wired in via startup extensions and project references.

- Language composition: C# (100%)

---

## Overview

The backend manages:
- Tours and ordered checkpoints (creation, progression, execution state).
- Secrets and encounters associated with checkpoints or locations.
- Tourist profiles with XP and levels (including creation and display).
- Challenge creation/review gated by level.
- Integration points for broader platform modules (Stakeholders, Payments, Blog, Games).

---

## Technology and Structure

- Framework: ASP.NET Core Web API (net7.0)
- DI: Built-in .NET dependency injection
- Persistence: EF Core (tools referenced in Explorer.API.csproj), Microsoft.Data.SqlClient
- Auth/CORS: Configured via startup extension classes
- API docs: Swagger (Swashbuckle)
- Test support: Integration test pattern enabled via partial Program class

Project references (from Explorer.API.csproj):

- BuildingBlocks: Core, Infrastructure
- Explorer.API (host)
- Modules
  - Blog: Core, Infrastructure
  - Stakeholders: API, Infrastructure
  - Tours: API, Infrastructure
  - Encounters: API, Infrastructure
  - Games: API, Infrastructure
  - Payments: API, Infrastructure

---

## Application Startup

As configured in Program.cs:
1. Add controllers
2. Configure Swagger (ConfigureSwagger)
3. Configure CORS (ConfigureCors with policy name "_corsPolicy")
4. Configure authentication (ConfigureAuth)
5. Register domain modules (RegisterModules)
6. Environment-specific middleware:
   - Development: DeveloperExceptionPage, Swagger, SwaggerUI
   - Production: Exception handler (/error), HSTS
7. Routing, CORS, HTTPS redirection, authorization
8. Map controllers
9. Scoped data seeding: EncountersContext.SeedData(IUserRepository)
10. Run

Program exposes a partial Program class to support integration tests.

---

## Core Domains

- Tours and Tour Checkpoints: aggregate root and services for execution (start, finish, abandon) and checkpoint actions.
- Encounters: CRUD, aggregate with validation, controller/services implemented.
- Secrets: discoverable content at checkpoints (backend persistence; front-end discovery).
- Tourist Profiles: XP/level state (class extends users; seeded at startup).
- Challenges: level-gated creation and review (tourist level ≥ threshold).
- Stakeholders, Payments, Blog, Games: platform modules referenced by the host.

---

