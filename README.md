# Hotel Booking API

This is the backend API for the Hotel Booking application, built using ASP.NET Core and Entity Framework Core with a SQL Server database.

## Database Setup

The database schema, including all necessary tables, constraints, and stored procedures, is maintained via SQL scripts rather than EF Core Migrations (per project rules).

To set up the database locally:
1. Open SQL Server Management Studio (SSMS) or Azure Data Studio.
2. Ensure you have a database named `HotelBookingDb` (or create one).
3. Navigate to the `Migrations` folder in this project repository.
4. Open the `DatabaseSchema.sql` file.
5. Execute the SQL script against your `HotelBookingDb` database.

*Note: Any structural updates (like `ALTER TABLE`) to the database must be appended or modified within this `DatabaseSchema.sql` script manually.*
