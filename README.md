# Barber Booking API

A REST API for a barber reservation system built with .NET 8.
The system provides management of reservations, users, and available time slots, with a focus on data consistency and security.

---

## 🚀 Tech stack

**Backend:** ASP.NET Core 8 (C#)  
**Frontend:** React (JavaScript)  
**Database:** SQL Server  
**ORM:** Entity Framework Core  
**Validation:** FluentValidation  
**Testing:** xUnit  
**DevOps:** Docker, Docker Compose  
**Architecture:** REST API

---

## ✨ Key Features

- Reservation system for managing barber appointments
- Protection against booking conflicts and concurrent requests
- Role-based access control (Admin / Barber / User)
- Input validation and protection against invalid API requests
- REST API connected to the React frontend
- Confirmation emails for created reservations

---

## 🛠 Getting started

```bash
git clone https://github.com/PheterCZ/Barber_Reservation
cd Barber_Reservation/backend
cp .env.example .env
docker compose up --build
