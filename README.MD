📌 **Project Overview**<br>
Due to the limited time for the assignment, I built a simplified project to implement the basic flow of the requirements.  
This project includes the ability to create events, ticket types, tickets, and users.  

The Ticket Type entity is an additional feature not explicitly required, but I included it because I believe it is helpful for managing different ticket categories for an event (e.g., VIP, General Admission).  

Since many details were not specified in the requirements, I made assumptions about most fields and business rules in this project.  
<br>

🚀 **Features**<br>
🗓️ **Event Management**<br>
✅ Create & update concert events  <br>
✅ Define ticket types & pricing  <br>
✅ Manage available venue capacity  <br>
✅ Provide event details (date, venue, description)  <br>  

🎫 **Ticket Reservations & Sales**<br>
✅ Reserve tickets for a time window  <br>
✅ Purchase tickets (assumes external payment processing)  <br>
✅ Cancel reservations  <br>
✅ View ticket availability  <br>
✅ Prevent overselling with real-time validation  <br>  

👤 **User Management**<br>
✅ User authentication (JWT-based login/register)  <br>
✅ View user’s ticket purchase history  <br>  

🛠️ **Technology Stack**<br>
| Technology | Description |
|------------|-------------|
| .NET 8+ | Backend framework |
| Entity Framework Core | Database ORM |
| In-Memory Database | Stores event & ticket data |
| JWT Authentication | Secures user authentication |
| Swagger (Swashbuckle) | API documentation & testing |

📂 **Project Structure**<br>
📦 **ConcertTicketAPI**<br>
 ┣ 📂 **Controllers**            # API Controllers  <br>
 ┣ 📂 **DTOs**                   # Data Transfer Objects  <br>
 ┣ 📂 **Models**                 # Entity Models  <br>
 ┣ 📂 **Repositories**           # Data Access Layer  <br>
 ┣ 📂 **Services**               # Business Logic Layer  <br>
 ┣ 📂 **Tests**                  # Unit Tests  <br>
 ┣ 📜 **Program.cs**             # App Startup Configuration  <br>
 ┣ 📜 **appsettings.json**       # Configuration File  <br>
 
 🎯 **Summary of Relationships**<br>
📅 **Event (1)** ─── 🏷️ **TicketType (Many)** ─── 🎟️ **Ticket (Many)** ─── 👤 **User (1)**<br>
👤 **User (1)** ─── 📅 **Event (Many)**<br>

✅ **Events**: Created by users  <br>
✅ **TicketTypes**: Different ticket categories for an event  <br>
✅ **Tickets**: Represent actual purchases/reservations  <br>
✅ **Users**: Buy tickets and manage events  <br>
