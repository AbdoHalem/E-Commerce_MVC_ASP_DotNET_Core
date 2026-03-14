# Repository and Unit of Work Patterns in ASP.NET Core

This document serves as a reference guide for the Data Access Layer architecture used in this project. It explains the implementation details of the Generic Repository Pattern, the Unit of Work Pattern, and the advanced C# techniques used to optimize performance.

---

## 1. The Generic Repository Pattern

### What is it?
Instead of creating a specific repository for every single database table (e.g., `ProductRepo`, `OrderRepo`, `UserRepo`) that duplicates the same CRUD operations, we use a single **Generic Repository** (`EntityRepo<T>`). 

### Why use it?
It strongly enforces the **DRY (Don't Repeat Yourself)** principle. One class handles `Add`, `Update`, `Delete`, and `Get` operations for *any* database entity.

```csharp
// Example: EntityRepo.cs
public class EntityRepo<T> : IEntityRepo<T> where T : class
{
    ECommContext _context;
    DbSet<T> _set;

    public EntityRepo(ECommContext context)
    {
        _context = context;
        _set = _context.Set<T>(); 
    }
    // ... CRUD Implementations ...
}
2. The Unit of Work (UoW) Pattern
What is it?
The Unit of Work acts as a central manager or a "wrapper" for all our repositories. It ensures that all repositories share a single instance of the DbContext during a single HTTP request.

Why use it?
Atomic Transactions: By sharing the DbContext, we can perform multiple database operations (e.g., adding an Order and decreasing Product stock) and save them all at once using a single SaveTransact() call. If one operation fails, nothing is saved to the database, preventing data inconsistency.

Clean Controllers: Controllers only need to inject IUnitOfWork instead of injecting 5 or 6 different repositories, keeping the constructor clean.

3. Lazy Initialization (Performance Optimization)
To prevent the application from consuming unnecessary memory, we use Lazy Initialization inside the UnitOfWork class.

The Syntax
C#
// 1. The Private Backing Field (Hidden storage)
IEntityRepo<Product> _productRepo;

// 2. The Public Property (The open door)
public IEntityRepo<Product> productRepo => _productRepo ??= new EntityRepo<Product>(_context);
Breaking down the C# Features:
=> (Expression-bodied property): Introduced in C# 6.0. In this context, it acts as a shorthand for a read-only get block. It means "When this property is accessed, evaluate and return the following expression."

??= (Null-coalescing assignment operator): Introduced in C# 8.0. It checks the left side (_productRepo). If it is null, it executes the right side (creates a new EntityRepo), assigns it to the left side, and returns it. If it is NOT null, it immediately returns the existing value.

How it saves memory:
When a controller requests unitOfWork.productRepo, the UnitOfWork only instantiates the Product repository. All other repositories (like User or Order) remain null in memory until they are explicitly called.

4. Unit of Work & Dependency Injection (DI)
A common misconception is that we should inject all repositories into the UnitOfWork constructor via the DI Container. We do NOT do this.

The Wrong Way:
C#
// BAD PRACTICE: DI will instantiate ALL repos immediately, destroying Lazy Initialization.
public UnitOfWork(IEntityRepo<App_User> userRepo, IEntityRepo<Product> productRepo) { ... }
The Right Way:
We only inject the DbContext into the UnitOfWork. We manually new up the repositories on-demand using the Lazy Initialization technique explained above.

C#
// GOOD PRACTICE: Inject only the context.
public UnitOfWork(ECommContext context)
{
    _context = context;
}
In Program.cs, we register the Unit of Work so controllers can use it:

C#
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();