# deskBooking 🪑

Kompletna aplikacja do zarządzania biurkami (hot-desking) w architekturze **Clean Architecture** i wzorcem **CQRS**, wraz z nowoczesnym frontendem.
---

## 🛠️ Technologie

**Backend (.NET 10)**
- **Architektura:** Clean Architecture
- **Wzorzec:** CQRS (z wykorzystaniem MediatR)
- **Baza danych:** Entity Framework Core + SQL Server (LocalDB)
- **Walidacja & API:** FluentValidation, Swagger (z OpenApi), Global Exception Handling (Problem Details RFC 7807)
- **Testy:** xUnit, Moq (Testy jednostkowe handlerów)

**Frontend (Next.js 16)**
- **Framework:** Next.js (App Router)
- **Język:** TypeScript
- **Styling:** Czysty CSS (Dark Theme, Glassmorphism) bez użycia Tailwind CSS
- **Komunikacja:** Fetch API (z obsługą błędów konfliktów 409)

---

## 🚀 Jak uruchomić projekt lokalnie?

### 1. Uruchomienie Backendu (.NET)

Przejdź do katalogu głównego projektu i uruchom API:

```bash
dotnet run --project deskBooking.Api
```

API wystartuje na porcie `5058` i **automatycznie zaaplikuje migracje do bazy danych LocalDB**.
- **Swagger UI:** `http://localhost:5058/swagger`

### 2. Uruchomienie Frontendu (Next.js)

Otwórz nowe okno terminala, wejdź do folderu `frontend` i uruchom serwer deweloperski:

```bash
cd frontend
npm install
npm run dev
```

Aplikacja będzie dostępna pod adresem:
- **Interfejs UI:** `http://localhost:3000`

---

## 📚 Funkcjonalności

1. **Biurka (`/desks`)**
   - Wyświetlanie listy biurek z podziałem na "Stojące" / "Standardowe".
   - Dodawanie nowego biurka.
   - Usuwanie biurka (z automatycznym usunięciem rezerwacji).

2. **Rezerwacje (`/bookings`)**
   - Przeglądanie aktualnych rezerwacji.
   - Formularz z weryfikacją w locie – **Backend automatycznie sprawdza nakładające się terminy** (tzw. Overlapping bookings). Próba zarezerwowania zajętego terminu zwraca błąd `409 Conflict`, który jest obsługiwany przez Frontend.
   - Anulowanie własnych rezerwacji.

---

## 🧪 Testy

Aplikacja posiada dedykowany projekt testowy `deskBooking.Tests`, testujący logikę biznesową w izolacji od bazy danych.

```bash
dotnet test
```

---

## 🏗️ Struktura Repozytorium

```text
deskBooking/
├── frontend/                     # Aplikacja w Next.js
├── deskBooking.Api/              # Kontrolery i konfiguracja aplikacji ASP.NET
├── deskBooking.Application/      # CQRS (Commands, Queries, DTOs, Handlery)
├── deskBooking.Domain/           # Encje, Interfejsy Repozytoriów, Wyjątki domenowe
├── deskBooking.Infrastructure/   # Implementacja repozytoriów, Entity Framework (DbContext)
└── deskBooking.Tests/            # Testy jednostkowe z użyciem xUnit i Moq
```
