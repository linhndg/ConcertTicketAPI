📌 Project Overview
Due to the limited time for the assignment, I built a simplified project to implement the basic flow of the requirements.
This project includes the ability to create events, ticket types, tickets, and users.

The Ticket Type entity is an additional feature not explicitly required, but I included it because I believe it is helpful for managing different ticket categories for an event (e.g., VIP, General Admission).

Since many details were not specified in the requirements, I made assumptions about most fields and business rules in this project.

🚀 Features
🗓️ Event Management
✅ Create & update concert events
✅ Define ticket types & pricing
✅ Manage available venue capacity
✅ Provide event details (date, venue, description)

🎫 Ticket Reservations & Sales
✅ Reserve tickets for a time window
✅ Purchase tickets (assumes external payment processing)
✅ Cancel reservations
✅ View ticket availability
✅ Prevent overselling with real-time validation

👤 User Management
✅ User authentication (JWT-based login/register)
✅ View user’s ticket purchase history

🛠️ Technology Stack
Technology	Description
.NET 8+	Backend framework
Entity Framework Core	Database ORM
In-Memory Database	Stores event & ticket data
JWT Authentication	Secures user authentication
Swagger (Swashbuckle)	API documentation & testing
📂 Project Structure

📦 ConcertTicketAPI
 ┣ 📂 Controllers            # API Controllers
 ┣ 📂 DTOs                   # Data Transfer Objects
 ┣ 📂 Models                 # Entity Models
 ┣ 📂 Repositories           # Data Access Layer
 ┣ 📂 Services               # Business Logic Layer
 ┣ 📂 Tests                  # Unit Tests
 ┣ 📜 Program.cs             # App Startup Configuration
 ┣ 📜 appsettings.json       # Configuration File
 
 
 🎯 Summary of Relationships
📅 Event (1) ─── 🏷️ TicketType (Many) ─── 🎟️ Ticket (Many) ─── 👤 User (1)
👤 User (1) ─── 📅 Event (Many)

✅ Events: Created by users
✅ TicketTypes: Different ticket categories for an event
✅ Tickets: Represent actual purchases/reservations
✅ Users: Buy tickets and manage events