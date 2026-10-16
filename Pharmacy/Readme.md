Pharmacy Management System

A console-based Pharmacy Management System built with C# and .NET. The project is designed to practice object-oriented programming, layered architecture, and service-based application development.

About the Project

The Pharmacy Management System helps manage essential pharmacy operations, including medicines, categories, suppliers, purchases, and sales.

The application uses a console-based user interface to interact with the user. Business operations are handled by service classes, while domain entities represent the main objects in the pharmacy system.

Features

1. Category Management

- Add new medicine categories.
- Search for categories by name.
- Display category information.

2. Medicine Management

- Add new medicines.
- Search for medicines by name.
- Display all medicines.
- Remove medicines.
- Store medicine information, including:
  - Medicine name
  - Manufacturer
  - Barcode
  - Active ingredient
  - Strength
  - Purchase price
  - Sale price
  - Reorder level
  - Dosage form
  - Category

3. Supplier Management

- Add suppliers.
- Search for suppliers by name or phone number.
- Update supplier information.
- Delete suppliers.
- Display all suppliers.
- Store contact and business information.

4. Purchase Management

- Create purchase invoices.
- Search for purchases by ID or invoice number.
- Display purchase records.
- Associate purchases with suppliers and medicines.
- Manage batch-related purchase information.

5. Sales Management

- Create sales invoices.
- Search for sales by ID or invoice number.
- Display sales records.
- Select medicines and batches.
- Validate batch availability and expiry.
- Update batch quantities when medicines are sold.
- Support payment methods defined in the application.

6. Initial Sample Data

The application includes a "PharmacySeeder" class that initializes sample data, including:

- Two suppliers
- Two medicine categories
- Two medicines: Panadol and Amoxicillin

The seeder runs when "pharmacySeeder.Seed()" is called from "Program.cs".


Technologies Used

- C# — Main programming language.
- .NET — Application runtime and development platform.
- Object-Oriented Programming (OOP) — Organizing application components into classes and objects.
- Enums — Representing predefined values such as dosage forms and payment methods.
- Collections — Managing application data in memory.
- Exception Handling — Handling invalid operations and application errors.
- Console UI — Providing a text-based interface for users.

Getting Started

Prerequisites

- A compatible .NET SDK.
- A C# development environment.
- Git (optional, for version control).

Installation

1. Clone the repository:
   
   git clone YOUR_REPOSITORY_URL

2. Navigate to the project directory:
   
   cd YOUR_PROJECT_DIRECTORY

3. Build the project:
   
   dotnet build

4. Run the application:
   
   dotnet run

Replace "YOUR_REPOSITORY_URL" and "YOUR_PROJECT_DIRECTORY" with your actual repository URL and project directory.

Initial Data

The application seeds the following sample data:

Type| Name
Supplier| Al-Shifa Pharmaceutical Company
Supplier| Al-Hayat Medical Supplies
Category| Painkillers
Category| Antibiotics
Medicine| Panadol
Medicine| Amoxicillin

The sample data is initialized in memory when the application starts and the seeder is executed. Unless a persistent storage mechanism is implemented, this data is not retained after the application terminates.

Current Limitations

- The application currently uses a console-based interface.
- Data persistence depends on the current service implementations; in-memory collections do not preserve data between application runs.
- Authentication and user roles are not currently documented as implemented features.
- Additional validation, automated tests, and database integration can be introduced as the project evolves.

Future Improvements

- Integrate a database using Entity Framework Core or ADO.NET.
- Implement inventory reports and low-stock alerts.
- Add medicine expiry notifications.
- Improve input validation and error handling.
- Add automated unit tests.
- Introduce authentication and role-based access control.
- Develop a graphical or web-based user interface.

Learning Objectives

This project is intended to develop practical experience with:

- C# and object-oriented programming.
- Classes, objects, properties, and constructors.
- Separation of responsibilities.
- Service classes and dependency relationships.
- Collections and data manipulation.
- Exception handling.
- Building a multi-feature console application.
- Using Git and GitHub for version control.

Project Status

Status: In Development

New features and improvements will be added as development progresses.

License

No license has been specified for this project yet.