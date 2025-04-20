# 🧩 Piece of Iceland – API

**Secure backend API for managing virtual parcels, users, and transactions in the Piece of Iceland project.**
Built with .NET 8 (LTS), MongoDB, Swagger, and JWT authentication.

---

## 🚀 Tech Stack

- .NET 8 LTS
- ASP.NET Core Web API
- MongoDB (via MongoDB.Driver)
- JWT Authentication (Bearer tokens)
- Swagger (OpenAPI 3) + UI
- RESTful structure (Users, Parcels, Transactions)

---

## 🔐 Authentication

### `POST /api/auth/register`

Create new user with hashed password.

### `POST /api/auth/login`

Returns JWT token for valid credentials.

### `GET /api/auth/me`

Returns current user info (protected, needs Bearer token).

---

## 🔒 Securing Endpoints

Use `[Authorize]` on any controller or action to restrict it to authenticated users with valid JWT.

---

## 🔧 Environment Configuration (`env/`)

The project uses [DotNetEnv](https://github.com/tonerdo/dotnet-env) to load environment variables from `.env` files.

---

## 📦 Installation

```bash
dotnet build
dotnet run
```

and open:

```bash
http://localhost:5135/swagger
```
