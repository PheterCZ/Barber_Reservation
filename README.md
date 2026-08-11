Barber Booking System

A full-stack reservation system for barber services built with ASP.NET Core 8 Web API and React.

The project focuses on backend development, database design, authentication and authorization, validation, and real-world booking logic. The system allows users to manage appointments, barbers, services, and available time slots while preventing conflicting bookings.

🚀 Tech Stack
Backend: ASP.NET Core 8, C#
Frontend: React, JavaScript
Database: Microsoft SQL Server
ORM: Entity Framework Core
Validation: FluentValidation
Testing: xUnit
DevOps: Docker, Docker Compose
Architecture: REST API
✨ Key Features
Reservation system for managing barber appointments
Protection against conflicting and concurrent bookings
Role-based access control (Admin / Barber / User)
Input validation and API request validation
Confirmation emails for created appointments
REST API connected to the React frontend
Database access using Entity Framework Core
🧠 Technical Highlights

One of the main challenges of the project was preventing two users from successfully booking the same time slot.

The backend therefore handles booking conflicts and concurrent requests to maintain the consistency of reservations.

The project also uses role-based authorization to control access to different parts of the API.

🛠️ Getting Started
Prerequisites
Docker
Docker Compose
Run with Docker
git clone https://github.com/PheterCZ/Barber_Reservation.git
cd Barber_Reservation/backend
cp .env.example .env
docker compose up --build

The application can then be accessed through the configured API and frontend endpoints.

📌 Project Status

Personal project developed to improve my practical experience with C#/.NET backend development and modern web application architecture.
