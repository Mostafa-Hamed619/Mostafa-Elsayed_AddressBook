# Mostafa-Elsayed_AddressBook

A simple Address Book web application developed using **ASP.NET Core Web API** and **Angular**.

The application allows authenticated users to manage address book entries, including creating, editing, deleting, searching, and exporting contacts.

---

## 📌 Features

### Authentication
- User Registration
- User Login
- JWT-based authentication
- Protected routes using Angular `AuthGuard`
- JWT automatically sent with API requests using an HTTP interceptor
- Logout functionality

### Address Book
Users can:
- View all address book entries
- Add a new entry
- Edit an existing entry using a popup/modal
- Delete an entry with a confirmation message
- Search addresses
  - Search using multiple fields
  - Filter by date of birth range
- Upload a profile photo
- Calculate/display age
- Export the address book to an Excel file

### Address Entry Fields
Each address book entry contains:
- Full Name
- Job Title
- Department
- Mobile Number
- Date of Birth
- Address
- Email
- Photo
- Age

### Job Titles Management
Authenticated users can:
- Add Job Title
- Edit Job Title
- Delete Job Title
- View all Job Titles

### Departments Management
Authenticated users can:
- Add Department
- Edit Department
- Delete Department
- View all Departments

### Validation
The application validates:
- Required fields
- Full Name length
- Email format
- Egyptian mobile number format
- Address length
- Job Title selection
- Department selection
- Date of Birth

### UI / UX
- Responsive design
- Fullscreen Login page
- Fullscreen Register page
- Popup/modal for editing
- Confirmation dialog before deletion
- No page reload when adding, editing, or deleting data
- Angular Reactive Forms
- Date picker for Date of Birth

---

## 🏗️ Architecture

The backend follows an **Onion Architecture** approach.

```
AddressBook
│
├── AddressBook.Domain
│   ├── Entities
│   └── ...
│
├── AddressBook.Business
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── ...
│
├── AddressBook.Infrastructure
│   ├── Data
│   ├── Repositories
│   └── ...
│
└── AddressBook.Presentation
    ├── Controllers
    ├── Authentication
    ├── Middleware
    └── Program.cs
```

### Domain Layer
The Domain layer contains the core business entities of the application.

Examples:
- `User`
- `Address`
- `JobTitle`
- `Department`

The Domain layer does not depend on the Presentation layer.

### Business Layer
The Business layer contains the application's business logic.

It contains:
- DTOs
- Service interfaces
- Service implementations
- Business rules

Examples:
- `IAddressService` / `AddressService`
- `IJobTitleService` / `JobTitleService`
- `IDepartmentService` / `DepartmentService`

DTOs are used to control the data transferred between the API and the client.

### Infrastructure Layer
The Infrastructure layer is responsible for external concerns such as:
- Entity Framework Core
- SQL Server
- Database configuration
- Repositories
- Database migrations
- File storage

The project uses **Code First** with Entity Framework Core.

### Presentation Layer
The Presentation layer contains the ASP.NET Core Web API.

It contains:
- Controllers
- Authentication configuration
- JWT configuration
- CORS configuration
- Dependency Injection
- Middleware
- API endpoints

Example:
- `AddressController`
- `AuthController`
- `JobTitleController`
- `DepartmentController`

---

## 🗄️ Database

The project uses:
- SQL Server
- Entity Framework Core
- Code First

The database is generated from the application's entities and EF Core migrations.

Typical migration commands:

```bash
dotnet ef migrations add InitialCreate
```

Then:

```bash
dotnet ef database update
```

Make sure the SQL Server connection string is configured correctly in `appsettings.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AddressBookDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Update the connection string according to your SQL Server configuration.

---

## 🔐 Authentication Flow

The application uses JWT authentication.

The authentication flow is:

```
Register
   ↓
User created in database
   ↓
Login
   ↓
Validate email/password
   ↓
Generate JWT
   ↓
Angular stores JWT
   ↓
AuthInterceptor adds JWT
   ↓
Protected API endpoints
```

Protected endpoints require:

```csharp
[Authorize]
```

For example:

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
}
```

Angular also protects private pages using `AuthGuard`.

For example:

```typescript
{
  path: 'address-book',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./features/address-book/address-list/address-list.component')
      .then(m => m.AddressList)
}
```

---

## 📡 API Endpoints

### Authentication
The authentication endpoints provide:
- Register
- Login

Login returns a JWT token which is required to access protected endpoints.

### Address Book

**Get All Addresses**
```
GET /api/Address
```
Requires authentication.

**Get Address By ID**
```
GET /api/Address/{id}
```
Requires authentication.

**Create Address**
```
POST /api/Address
```
The endpoint accepts `multipart/form-data` because the address can contain a photo.

Fields include:
- FullName
- JobId
- DepartmentId
- MobileNumber
- DateOfBirth
- AddressLine
- Email
- Photo

**Update Address**
```
PUT /api/Address/{id}
```
The endpoint also accepts `multipart/form-data`. This allows updating the address information and optionally uploading a new photo.

**Delete Address**
```
DELETE /api/Address/{id}
```

**Search Addresses**
```
GET /api/Address/search
```
The search endpoint supports filtering using address fields and Date of Birth range.

Possible filters include:
- FullName
- JobId
- DepartmentId
- MobileNumber
- DateOfBirthFrom
- DateOfBirthTo
- AddressLine
- Email

**Export Addresses**
```
GET /api/Address/export
```
The API returns an Excel file: `Addresses.xlsx`

Angular receives the response as a Blob and triggers the browser download.

---

## 📁 Photo Storage

Uploaded address photos are stored in the backend under:

```
wwwroot/uploads/addresses
```

The API returns the photo path, for example:

```
/uploads/addresses/38ae261b-b63a-4b47-b848-521360c31a51.webp
```

The Angular application builds the complete URL using the API base URL:

```
https://localhost:7107/uploads/addresses/...
```

---

## 🖥️ Angular Architecture

The Angular application is organized by features and responsibilities.

```
src/
└── app/
    │
    ├── core/
    │   ├── guards/
    │   ├── interceptors/
    │   ├── services/
    │   └── models/
    │
    ├── features/
    │   ├── auth/
    │   │   ├── login/
    │   │   └── register/
    │   │
    │   ├── address-book/
    │   │   ├── address-list/
    │   │   ├── address-form/
    │   │   └── services/
    │   │
    │   ├── job-title/
    │   │
    │   └── department/
    │
    ├── shared/
    │   ├── components/
    │   ├── validators/
    │   └── models/
    │
    ├── app.routes.ts
    ├── app.config.ts
    └── app.component.ts
```

### Core
Contains functionality shared across the application.

**Guards**
The `AuthGuard` prevents unauthenticated users from accessing protected pages.

**Interceptors**
The HTTP interceptor attaches the JWT token to API requests.

Conceptually:

```
Angular Request
      ↓
AuthInterceptor
      ↓
Add Authorization: Bearer <token>
      ↓
ASP.NET Core API
```

**Services**
Services communicate with the backend API.

Examples:
- `AuthService`
- `AddressService`
- `JobTitleService`
- `DepartmentService`

---

## 📦 Features

### Auth
Contains:
- Login
- Register

Login is the first page displayed when the application starts.

### Address Book
Contains:
- Address List
- Address Form

**Address List** — Responsible for:
- Displaying contacts
- Delete
- Edit popup
- Export Excel
- Search
- Logout
- Navigation to Add New Entry
- Navigation to Job Titles
- Navigation to Departments

**Address Form** — Responsible for:
- Creating a new address
- Reactive form validation
- Job Title dropdown
- Department dropdown
- Date picker
- Photo upload
- Age calculation
- Sending multipart/form-data to the API

### 💼 Job Titles
The Job Titles page provides:
- Add
- Edit
- Delete
- List

Job Titles are loaded dynamically into the Address Form dropdown.

### 🏢 Departments
The Departments page provides:
- Add
- Edit
- Delete
- List

Departments are loaded dynamically into the Address Form dropdown.

---

## 🔎 Search

The application provides a search field for the address book.

The search functionality communicates with:

```
GET /api/Address/search
```

The backend supports searching using the available address fields and Date of Birth range.

The Angular application sends the selected filters as query parameters.

---

## 📊 Excel Export

The Address Book can be exported to Excel.

Angular calls:

```
GET /api/Address/export
```

The API returns:

```
application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
```

Angular receives the response as a `Blob` and creates a browser download without reloading the page.

---

## 🔄 No Page Reload

The application does not reload the browser after CRUD operations.

For example, after deleting an address, Angular updates the local array:

```typescript
this.addresses = this.addresses.filter(
  address => address.id !== id
);
```

Instead of:

```
Delete
 ↓
Reload entire page
 ↓
Call API again
```

the application uses:

```
Delete
 ↓
API
 ↓
Success
 ↓
Update Angular state
 ↓
UI updates immediately
```

The same approach is used for adding and editing entities.

---

## 🛠️ Technologies Used

**Backend**
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- REST API
- Code First
- Onion Architecture

**Frontend**
- Angular
- TypeScript
- HTML5
- CSS3
- Angular Reactive Forms
- Angular Router
- HTTP Client

**Development Tools**
- Visual Studio
- Visual Studio Code
- SQL Server
- Git / GitHub

---

## 🚀 How to Run the Backend

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Mostafa-Elsayed_AddressBook
```

### 2. Configure SQL Server

Open the backend project `AddressBook` and configure the SQL Server connection string in `appsettings.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AddressBookDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Apply Database Migrations

From the backend project directory:

```bash
dotnet ef database update
```

If migrations have not been created yet:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the API

Using the .NET CLI:

```bash
dotnet run
```

Or run the project through Visual Studio.

The API will be available on the configured HTTPS URL, for example:

```
https://localhost:7107
```

---

## 🚀 How to Run the Angular Frontend

Open another terminal and navigate to `address-book-client`.

Install dependencies:

```bash
npm install
```

Then run:

```bash
ng serve
```

The Angular application will normally be available at:

```
http://localhost:4200
```

---

## 🔗 Frontend ↔ Backend

Make sure the Angular API URL points to the running ASP.NET Core API.

For example:

```typescript
readonly apiBaseUrl = 'https://localhost:7107';
```

The backend must also allow requests from the Angular development server through CORS.

Typical development setup:

```
Angular
http://localhost:4200
        │
        │ HTTP Requests + JWT
        ▼
ASP.NET Core Web API
https://localhost:7107
        │
        ▼
SQL Server
```

---

## 👤 Using the Application

When the application starts, the user is redirected to `/login`.

### 1. Register
Create a new account from `/register`.

### 2. Login
Login using the registered credentials.

After successful authentication, the JWT token is stored by the frontend.

### 3. Address Book
After login, navigate to `/address-book`.

From the Address Book page you can:
- Add a new entry
- Edit an entry
- Delete an entry
- Search
- Export Excel
- Manage Job Titles
- Manage Departments
- Logout

---

## 🧭 Main Routes

```
/login
/register

/address-book
/address-book/new

/job-titles
/departments
```

Protected routes use:

```typescript
canActivate: [authGuard]
```

---

## 🔒 Authorization

**Public pages:**
- `/login`
- `/register`

**Protected pages:**
- `/address-book`
- `/address-book/new`
- `/job-titles`
- `/departments`

The frontend uses `AuthGuard`, while the backend uses ASP.NET Core `[Authorize]`. This means authentication is enforced on both sides.

---

## 📱 Responsive Design

The frontend is designed to support different screen sizes and resolutions.

The UI adapts to:
- Desktop
- Laptop
- Tablet
- Mobile

---

## 📋 Assignment Requirements

The project implements the requested requirements:

| Requirement | Status |
|---|---|
| Add new entry | ✅ |
| Full Name | ✅ |
| Job Title dropdown | ✅ |
| Department dropdown | ✅ |
| Mobile Number | ✅ |
| Date of Birth picker | ✅ |
| Address | ✅ |
| Email | ✅ |
| Password / Authentication | ✅ |
| Photo upload | ✅ |
| Age | ✅ |
| Edit using popup | ✅ |
| Delete confirmation | ✅ |
| Search | ✅ |
| Birth Date range | ✅ |
| Excel export | ✅ |
| Manage Jobs | ✅ |
| Manage Departments | ✅ |
| Fullscreen Login | ✅ |
| Fullscreen Register | ✅ |
| Responsive UI | ✅ |
| Email validation | ✅ |
| Phone validation | ✅ |
| No Visual Studio scaffolding | ✅ |
| Code First | ✅ |
| No page reload for CRUD | ✅ |
| Onion Architecture | ✅ |

---

## 📂 Project Structure

```
Mostafa-Elsayed_AddressBook/
│
├── AddressBook/
│   │
│   ├── AddressBook.Domain/
│   │
│   ├── AddressBook.Business/
│   │
│   ├── AddressBook.Infrastructure/
│   │
│   └── AddressBook.Presentation/
│
├── address-book-client/
│   │
│   ├── src/
│   │   └── app/
│   │       ├── core/
│   │       ├── features/
│   │       ├── shared/
│   │       ├── app.routes.ts
│   │       └── app.config.ts
│   │
│   ├── package.json
│   └── angular.json
│
└── README.md
```

---

## 🧪 Development Notes

The application was developed without using Visual Studio scaffolding.

The frontend communicates with the backend through RESTful APIs.

CRUD operations are performed asynchronously using Angular's `HttpClient` and RxJS observables.

The backend uses DTOs to define API input/output models and separates business logic from controllers through service interfaces.

---

## 👨‍💻 Author

**Mostafa Elsayed**
.NET / Angular Developer

- GitHub: `https://github.com/Mostafa-Hamed619`
- LinkedIn: `https://www.linkedin.com/in/mostafa-hamed-9178111b6/`

---

## 📄 License

This project was developed as part of an Address Book application assignment.
