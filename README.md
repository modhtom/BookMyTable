# BookMyTable

## Overview
BookMyTable is a restaurant reservation system built using ASP.NET Core MVC, Entity Framework Core (EF Core), LINQ, and JWT for authentication. The application provides both a customer interface for browsing restaurants and booking tables, and an admin dashboard for managing restaurant details, table configurations, and reservations.
---
## Features
- **User Authentication and Authorization**
  - User registration and login using JWT-based authentication.
  - Role-based access control to distinguish between Customers and Admins.
- **Restaurant Management (Admin Area)**
  - Create, read, update, and delete restaurant details.
  - Manage table configurations including table numbers and seating capacities.
- **Reservation System (Customer Area)**
  - Display a list of restaurants with filtering options.
  - Check table availability and make reservations.
  - View and cancel reservations.
- **REST API Endpoints**
  - Public endpoints for retrieving restaurant and table information.
  - Protected endpoints for making and managing reservations.
- **Data Persistence**
  - Uses EF Core with SQL Server (or an alternative database) for data management.
- **Security**
  - Secure endpoints with JWT and role-based access.
  - Refresh token functionality to maintain user sessions.
---
## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core (EF Core)
- LINQ
- JWT (JSON Web Tokens)
- REST API
- SQL Server

## Project Structure
- **Controllers:** Contains API endpoints for both Admin and Customer areas.
- **Services:** Business logic and authentication services.
- **Models:** Entity classes for Users, Restaurants, Tables, and Reservations.
- **Data:** EF Core DbContext for database access.
- **DTOs:** Data Transfer Objects for secure API communication.
---
## Getting Started

### Prerequisites
- .NET 6 or later
- SQL Server (or a compatible database)
- Visual Studio 2022 or Visual Studio Code

### Setup Instructions
1. Clone the repository:
```
git clone https://github.com/modhtom/BookMyTable.git
```
2. Navigate to the project directory:
```
cd BookMyTable/BookMyTable
```
3. Update the connection string in `appsettings.json` to point to your database.
4. Apply EF Core migrations to set up the database
5. Build and run the project
---
### API Documentation
- **Authentication:**
- `POST /api/auth/register` - Register a new user.
- `POST /api/auth/login` - Log in and receive JWT tokens.
- `POST /api/auth/refresh-token` - Refresh expired JWT tokens.
- **Customer Endpoints:**
- `GET /api/customer/restaurants` - Retrieve all restaurants.
- `GET /api/customer/tables?restaurantId={id}` - Check table availability.
- `POST /api/customer/reservations` - Create a new reservation.
- `GET /api/customer/reservations/{id}` - View reservation details.
- `DELETE /api/customer/reservations/{id}` - Cancel a reservation.
- **Admin Endpoints:**
- `POST /api/admin/restaurants` - Create a restaurant.
- `GET /api/admin/restaurants` - Retrieve all restaurants.
- `GET /api/admin/restaurants/{id}` - Retrieve a specific restaurant.
- `PUT /api/admin/restaurants/{id}` - Update a restaurant.
- `DELETE /api/admin/restaurants/{id}` - Delete a restaurant.
- `POST /api/admin/tables` - Create a table.
- `GET /api/admin/tables` - Retrieve all tables.
- `GET /api/admin/tables/{id}` - Retrieve a specific table.
- `PUT /api/admin/tables/{id}` - Update a table.
- `DELETE /api/admin/tables/{id}` - Delete a table.
- `GET /api/admin/reservations` - View all reservations.
- `DELETE /api/admin/reservations/{id}` - Cancel a reservation.
---
## Contribution
Feel free to fork the repository and submit pull requests for improvements or bug fixes.
---
## License
This project is licensed under the MIT License.