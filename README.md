# My Meal Planner

This is a web application designed to help users to plan their shopping trips and shop faster. This project was built to demonstrate full-stack development skills using ASP.NET Core and a modern database solution.

## Technology Stack

* **Backend:** ASP.NET Core MVC (.NET 9)
* **Database:** PostgreSQL (running in Docker)
* **ORM:** Entity Framework Core

## Getting Started

Follow these instructions to get the project running on your local machine.

### Prerequisites

* [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* A Git client

### 1. Clone the Repository

```bash
git clone https://github.com/gvidas333/Sheepy.git
cd Sheepy
```

### 2. Run the Backend Environment

The database runs in a Docker container. Use `docker-compose` to start it.

```bash
# Navigate back to the root 'Server' folder
cd ../../.. 

# Start the database container
docker-compose up -d postgres
```

### 4. Apply Database Migrations

Once the database container is running, apply the Entity Framework migrations to create the database schema.

```bash
# Make sure you are in the 'Server' directory
dotnet ef database update
```

### 5. Run the Application

You can now run the main web application from Rider by pressing the green "Play" button, or by using the following command from the `Server` directory:

```bash
dotnet run
```
The application will be available at `https://localhost:7021`.

---

## Exploring the Project

### Accessing the API with Swagger

Once the application is running, you can explore the available API endpoints using the built-in Swagger UI.

* **Swagger URL:** [https://localhost:7021/swagger](https://localhost:7021/swagger)

This interface allows you to see all API controllers and execute requests directly from your browser.

### Connecting to the Database with PgAdmin

The project includes a `pgadmin` service for easy database management.

1.  **Start the PgAdmin Container:** If it's not already running, start it from your `Server` directory:
    ```bash
    docker-compose up -d pgadmin
    ```
2.  **Open PgAdmin:** Navigate to [http://localhost:8080](http://localhost:8080) in your web browser.
3.  **Login:** Use the credentials defined in `docker-compose.yml`:
    * **Email:** `admin@admin.com`
    * **Password:** `admin`
4.  **Register the Server:** To connect to the PostgreSQL database, you need to register it in PgAdmin.
    * Right-click `Servers` -> `Register` -> `Server...`
    * **General Tab:** Give it a name, like `sheepy-db`.
    * **Connection Tab:** Use the following settings:
        * **Host name/address:** `postgres`
        * **Port:** `5432`
        * **Maintenance database:** `sheepydb`
        * **Username:** `user`
        * **Password:** `password`
    * Click **Save**.

You can now browse the `sheepydb` database, view the tables created by Entity Framework, and run SQL queries.