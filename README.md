# Halem Store – ASP.NET Core MVC E-Commerce Platform 🛒

**Live Demo:** [http://halemstore.runasp.net/](http://halemstore.runasp.net/)
**Repository:** [https://github.com/AbdoHalem/E-Commerce_MVC_ASP_DOTNET_Core](https://github.com/AbdoHalem/E-Commerce_MVC_ASP_DOTNET_Core)

---

## Overview

This project was developed as part of the ASP.NET Core MVC course at the Information Technology Institute (ITI). It serves as a practical implementation of the concepts covered in the course, including MVC architecture, Entity Framework Core, authentication, and common backend design patterns used in real-world ASP.NET applications.

**Halem Store** is a full‑stack E‑Commerce web application built using **ASP.NET Core MVC (.NET)**.
The solution follows a multi‑project architecture where the **web application** handles the UI and controllers, while a separate **Entities project** contains the Data Access Layer implementation, including the database models, repositories, and Unit of Work pattern.

The goal of the project is to demonstrate a real‑world ASP.NET Core MVC architecture with proper separation between presentation logic, business logic, and data access.

---

## Features

### Customer Features

* Browse products with **category filtering**
* **Search** products by name
* **Sorting** and **pagination** for product lists
* Add and remove products from the **shopping cart**
* Checkout process with **address selection**
* View **order history** and order details
* User **registration and login** using ASP.NET Identity

### Admin Features

* Secure **Admin Area** protected with role‑based authorization
* **Product Management (CRUD)**
* **Category Management (CRUD)**
* **Order Management** and order status updates
* **Product image upload and management**
* Automatic **stock restoration** when an order is cancelled

---

## Architecture & Design Patterns

### Generic Repository Pattern

The project implements the Repository Pattern using a **generic repository** rather than creating a separate repository for each entity.

Inside the **Entities/Repos** folder, there is a generic interface and implementation:

* `IEntityRepo<T>`
* `EntityRepo<T>`

This repository provides common database operations such as:

* Get entity by id
* Get all entities
* Add entity
* Update entity
* Delete entity
* Query using LINQ

Using a generic repository reduces duplicated code and provides a unified way to access the database across the application.

---

### Unit of Work Pattern

The **Unit of Work** implementation is located in the **Entities/UnitOfWork** folder.

The UnitOfWork:

* Receives the application's `DbContext`
* Lazily initializes repositories when needed
* Provides a single `Save` or `Commit` method to persist changes

This ensures that multiple operations can be executed within a single transaction, which is especially important during operations such as order checkout.

---

## Project Structure

```
E-Commerce_MVC_ASP_DOTNET_Core/
│
├── ECommerce_WebSite/                # Main ASP.NET Core MVC web application
│   ├── Controllers/
│   ├── Views/
│   ├── Areas/
│   ├── wwwroot/
│   ├── Program.cs
│   └── appsettings.json
│
├── Entities/                         # Data Access Layer project
│   ├── Data/                         # DbContext and database configuration
│   ├── Migrations/                   # Entity Framework migrations
│   ├── Models/                       # Entity classes (Product, Category, Order, etc.)
│   ├── Repos/                        # Generic repository implementation
│   │   ├── IEntityRepo.cs
│   │   └── EntityRepo.cs
│   ├── UnitOfWork/                   # Unit of Work implementation
│   │   ├── IUnitOfWork.cs
│   │   └── UnitOfWork.cs
│   └── Entities.csproj
│
├── ECommerce_WebSite.sln
├── Authentication.txt
└── Project.pdf
```

---

## Technologies Used

### Backend

* C#
* ASP.NET Core MVC
* Entity Framework Core

### Database

* Microsoft SQL Server

### Authentication & Security

* ASP.NET Core Identity
* Cookie Authentication

### Frontend

* Razor Views
* HTML5
* CSS3
* Bootstrap

### State Management

* Session
* TempData
* ViewData

---

## Running the Project Locally

### 1. Clone the repository

```bash
git clone https://github.com/AbdoHalem/E-Commerce_MVC_ASP_DOTNET_Core.git
cd E-Commerce_MVC_ASP_DOTNET_Core
```

### 2. Open the solution

Open `ECommerce_WebSite.sln` using **Visual Studio**.

### 3. Configure the database connection

Edit the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "con1": "Server=.;Database=YourDatabaseName;Trusted_Connection=True;"
}
```

### 4. Apply database migrations

Open **Package Manager Console** and run:

```powershell
Update-Database
```

### 5. Run the application

Run the `ECommerce_WebSite` project using:

```
F5 (Visual Studio)
```

or

```
dotnet run
```

---

## Author

**Abdelrahman Abdelhalem**
LinkedIn: [www.linkedin.com/in/abdelrahman-abdelhalem-28040a230](http://www.linkedin.com/in/abdelrahman-abdelhalem-28040a230)
