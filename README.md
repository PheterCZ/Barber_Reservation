# Barber Booking API

Backendové REST API pro rezervační systém holičství postavené na .NET 8.  
Systém umožňuje správu rezervací, uživatelů a dostupných časů s důrazem na konzistenci dat a bezpečnost.

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

## ✨ Klíčové vlastnosti

- Rezervační systém pro správu termínů holiče
- Ochrana proti kolizím rezervací (konkurentní zápisy)
- Role-based access control (Admin / Barber / User)
- Validace vstupů a ochrana API proti nevalidním datům
- Potvrzovací e-maily pro vytvořené rezervace
- REST API napojené na frontend aplikaci

---

## 🛠 Spuštění projektu

```bash
git clone https://github.com/PheterCZ/Barber_Reservation
cd Barber_Reservation
cd backend
docker-compose up --build