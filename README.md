# BARBERSHOP BOOKING API

Backendové API pro rezervační systém holičství postavené na .NET 8 a Reactu.

## 🚀 Hlavní technologie
* **Backend:** ASP.NET Core 8 (C#)
* **Frontend:** React (JavaScript)
* **Databáze:** SqlServer
* **ORM:** Entity Framework Core
* **Validace:** FluentValidation
* **Testování:** xUnit (Unit testy)
* **Kontejnerizace:** Docker & Docker Compose
* **Architektura:** RESTful API

## 🛠 Jak projekt spustit
1. Klonujte repozitář: `git clone https://github.com/PheterCZ/Barber_Reservation`
2. Ujistěte se, že máte nainstalovaný Docker.
3. V kořenovém adresáři spusťte: `docker-compose up --build`
4. Aplikace bude dostupná na adrese `http://localhost:5173`.

## ✨ Klíčové vlastnosti
* **Rezervace termínů:** Uživatelé si mohou vybrat volný čas u holiče.
* **Prevence konfliktů:** Systém automaticky hlídá, aby se na stejný čas nemohlo objednat více lidí.
* **Bezpečné API:** Celá komunikace je validovaná a chráněná proti neplatným datům.
* **Autentizace a autorizace:** Systém rolí (Admin, Barber, User) zajišťující bezpečnost dat.
* **Automatizace:** Zasílání potvrzovacích e-mailů o provedených rezervacích.

## 🎓 Získané zkušenosti
* Implementace čisté architektury a standardů REST API.
* Práce s databází prostřednictvím EF Core a řešení transakcí.
* Nasazení aplikace do Docker kontejnerů pro zajištění konzistentního vývojového prostředí.
* Psaní automatizovaných unit testů (xUnit) pro ověření klíčové logiky systému.