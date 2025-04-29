# 🧩 Piece of Iceland – API

**Secure backend API for managing virtual parcels, users, and transactions in the Piece of Iceland project.**  
Built with .NET 8 (LTS), MongoDB, Swagger UI, and JWT authentication.

---

## 🚀 Tech Stack

- .NET 8 LTS
- ASP.NET Core Web API
- MongoDB (via MongoDB.Driver)
- JWT Authentication (Bearer tokens)
- Swagger (OpenAPI 3) + Swagger UI
- CORS for frontend integration
- DotNetEnv for configuration

---

## 🔐 Authentication

### `POST /api/auth/register`

Create a new user with email, username and password (hashed before saving).

### `POST /api/auth/login`

Validates credentials and returns a short-lived JWT token.

### `POST /api/auth/refresh`

Refreshes the JWT token for an active session (requires valid token in Authorization header).

### `GET /api/auth/me`

Returns authenticated user's basic information.

---

## 🔒 Securing Endpoints

Use `[Authorize]` on any controller or action to restrict it to authenticated users.  
JWT token must be sent in the `Authorization: Bearer <token>` header.

---

## 🔧 Environment Configuration (`env/`)

Environment variables are loaded from .env using DotNetEnv.
📁 Example: env/.env

```json
JWT__Key=your-secret-jwt-key-at-least-32-characters
JWT__Issuer=PieceOfIceland
MONGODB__ConnectionString=mongodb://localhost:27017
MONGODB__DatabaseName=PieceOfIcelandDb
```

---

## 📦 Installation

Build and run::

```bash
dotnet build
dotnet run
```

Then open Swagger::

```bash
http://localhost:5135/swagger
```
