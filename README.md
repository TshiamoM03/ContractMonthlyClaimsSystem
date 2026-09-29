# Contract Monthly Claims System

The Contract Monthly Claims System is a demo C# ASP.NET Core MVC web application. The prototype aims to provide university independent-contractor lecturers with a way to submit payment 
claims for work performed, attach supporting documentation, have their claims reviewed and approved, and receive payment documentation. The system uses role-based access control and a 
relational database for storing and managing application data.

## Functionality
**Lecturers can:**
* View and manage their profiles
* Submit monthly claims with their hourly rate, hours worked, and additional notes
* Upload supporting documentation such as timesheets
* View all submitted claims
* Track the status of claims
* View auto-generated income documentation

**Reviewers can:**
* View pending lecturer claims
* Review claims and supporting documentation
* Approve or reject claims
* Provide reasons for rejected claims
* Track claims that they have reviewed

**HR can:**
* Create and view lecturer profiles
* View approved and rejected claim information
* Access summarised claim reports
* Issue payments for approved claims
* Track payment history associated with claims

## Technology Stack
* C#
* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* Bootstrap
* HTML
* CSS

## Installation
### Prerequisites
* .NET SDK
* Visual Studio
* SQL Server

### Clone the Repository
```bash
git clone https://github.com/TshiamoM03/ContractMonthlyClaimsSystem.git
cd CMCS
```

### Configure the Database
Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CMCS;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Apply Database Migrations
Run the following command in the project directory:

```bash
dotnet ef database update
```

### Run the Application
Open the solution in Visual Studio and click the **Run** button to launch the application.
