# Resume Builder - Backend (ASP.NET Core API)

## 📌 Project Overview
This is the backend service for the Resume Builder application, built using **ASP.NET Core**. It provides RESTful APIs for creating, retrieving, updating, and deleting resumes. The API also includes **JWT authentication** to ensure security.

## 🚀 Features
- **CRUD Operations** for resume management
- **JWT Authentication & Authorization**
- **Role-based access control** (if needed)
- **Entity Framework Core with SQL Server**
- **Validation & Error Handling**

## 🛠️ Tech Stack
- **.NET 8 (ASP.NET Core)**
- **Entity Framework Core 9.0.2**
- **SQL Server**
- **JWT Authentication**
- **Swagger (for API documentation)**

## 🔧 Setup & Installation
### 1️⃣ Clone the Repository
```bash
git clone https://github.com/99khalid/Resume-Backend.git
cd Resume-Backend
```
### 2️⃣ Configure Database
- Update `appsettings.json` with your **SQL Server** connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ResumeDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
```
- Run migrations to create the database:
```bash
dotnet ef database update
```

### 3️⃣ Run the API
```bash
dotnet run
```
- The API will be available at: `https://localhost:5024/swagger`

## 🔑 Authentication & Authorization
- **Register/Login** to get a JWT token.
- Use the token in API requests under `Authorization` header.

## 📄 API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| POST   | `/api/auth/register` | Register a new user |
| POST   | `/api/auth/login` | User login (returns JWT token) |
| GET    | `/api/resumes` | Get all resumes (Authenticated users) |
| POST   | `/api/resumes` | Create a new resume |
| PUT    | `/api/resumes/{id}` | Update a resume |
| Get    | `/api/resumes/{id}` | Get Resume By Id |
| DELETE | `/api/resumes/{id}` | Delete a resume |

## 📜 License
This project is for interview purposes and is not intended for production use.

## 📬 Contact
For any inquiries, please reach out via **GitHub Issues** or email me at **khalidhassan912999@gmail.com**.

