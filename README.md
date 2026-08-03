# Cinema Ticket Booking API

A RESTful ASP.NET Core Web API for managing a cinema's movies, auditoriums, showtimes, and ticket bookings — built as a final course project demonstrating layered architecture, dependency injection, API versioning, global exception handling, and EF Core with a real SQL Server database.

## Overview

The API models a simple cinema booking flow: movies get scheduled into showtimes in specific auditoriums, and guest customers (no account or login required) can book and cancel tickets against those showtimes. Booking status (`Pending` / `Confirmed` / `Cancelled`) is controlled entirely by application logic — never directly settable by a client.

**Live demo:** not yet deployed — planned via Render for a future update.

## Features

- Full CRUD for Movies, Auditoriums, and ShowTimes
- Guest booking system — no authentication, just name/email
- **API versioning** on the Movies endpoint: v1 returns a compact shape, v2 returns a detailed one
- **Pagination, filtering, and sorting** on Movies (search by name, filter by genre, sort by name/release date) and **filtering** on Bookings (by customer, showtime, and status)
- **Global exception handling middleware** returning [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) `ProblemDetails` responses
- Business rules enforced at the service layer: duplicate movie names blocked, deletes blocked when active relationships exist, showtimes validated against movie/auditorium existence and cinema availability
- Booking confirmation/cancellation notifications (logged, swappable for real SMTP behind an interface)
- Interactive Swagger documentation with version switching
- Entity Framework Core migrations against a real SQL Server database

## Tech Stack

- **ASP.NET Core 8** — Web API framework
- **Entity Framework Core 9** — ORM, SQL Server provider
- **SQL Server (LocalDB)** — relational database
- **Asp.Versioning.Mvc** — API versioning
- **Swashbuckle.AspNetCore** — Swagger / OpenAPI documentation

## Project Structure

```
CinemaTicketBookingAPI/
├── Controllers/          # HTTP endpoints (5 controllers)
├── Services/
│   └── Interfaces/       # Business logic
├── Repositories/
│   └── Interfaces/       # Data access via EF Core
├── Models/               # EF Core entities
├── DTOs/                 # Request/response contracts
├── Enums/                # BookingStatus
├── Exceptions/           # Custom exceptions
├── Middleware/           # Global exception handling
├── Data/                 # DbContext + Migrations
└── Program.cs            # DI composition root
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (included with Visual Studio) or a full SQL Server instance

### Setup

1. **Clone the repo**
   ```bash
   git clone https://github.com/joemekowsky/CinemaTicketBookingAPI.git
   cd CinemaTicketBookingAPI
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Set your connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CinemaBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

4. **Apply the EF Core migrations** (creates the database and tables):
   ```bash
   dotnet ef database update
   ```
   *(If `dotnet ef` isn't recognized, install it once: `dotnet tool install --global dotnet-ef`)*

5. **Run the API**
   ```bash
   dotnet run
   ```

6. Open `https://localhost:<port>/swagger` to explore and test every endpoint interactively.

## API Overview

| Resource | Endpoint | Notes |
|---|---|---|
| Movies | `GET/POST/PUT/DELETE /api/v{1,2}/movies` | Versioned; supports search, genre filter, sorting, pagination |
| Auditoriums | `GET/POST/PUT/DELETE /api/auditoriums` | |
| ShowTimes | `GET/POST/DELETE /api/showtimes` | Validates movie/auditorium existence and cinema availability |
| Customers | `GET /api/customers` | Guest customers created implicitly via booking |
| Bookings | `GET/POST /api/bookings`, `PUT /api/bookings/{id}/cancel` | Filterable by customerId, customerName, showTimeId, status |

### Example: versioned Movie responses

```
GET /api/v1/movies/1   →  { "id": 1, "name": "Inception", "availableInCinema": true }
GET /api/v2/movies/1   →  { "id": 1, "name": "Inception", "genre": "Sci-Fi", "releaseDate": "2010-07-16", "availableInCinema": true }
```

## Business Rules

- A movie referenced by a showtime must exist and be marked available in cinema
- An auditorium referenced by a showtime must exist
- Duplicate movie titles are rejected (`409 Conflict`)
- A booking cannot be created for a nonexistent showtime
- Deleting a movie/auditorium/showtime with active relationships is blocked (`409 Conflict`)
- Booking status transitions (Pending → Confirmed/Cancelled) are controlled entirely by the service layer — the create-booking request has no status field at all

## Error Handling

All errors are returned as RFC 7807 `ProblemDetails`:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Movie Not Found",
  "status": 404,
  "detail": "Movie with id 9999 was not found.",
  "instance": "/api/v1/movies/9999"
}
```

## Author

Built by [Yousef  Mohamed Ahmed Elsayed](https://github.com/joemekowsky) as a final mini-project for a backend development course 
