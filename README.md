# BookSwap – internetowa giełda wymiany książek

## 📚 Opis projektu

**BookSwap** to aplikacja internetowa umożliwiająca użytkownikom wymianę książek między sobą. Projekt został stworzony jako realizacja zadania laboratoryjnego z przedmiotu programowania aplikacji webowych. Użytkownicy mogą dodawać swoje książki, przeglądać książki innych, proponować wymiany, akceptować lub odrzucać oferty oraz wysyłać wiadomości. System wspiera autoryzację, sesje użytkowników i komunikację z aplikacją przez REST API.

Pierwszy zarejestrowany użytkownik zostaje automatycznie administratorem. Administratorzy mają dodatkowe uprawnienia, takie jak dodawanie nowych użytkowników i zarządzanie aktywnością w systemie.

---

## 🎯 Główne funkcjonalności

### ✅ Uwierzytelnianie i autoryzacja
- Rejestracja i logowanie użytkowników
- Hasła hashowane (bcrypt)
- Autoryzacja oparta na rolach (użytkownik / administrator)
- Sesje użytkowników

### ✅ Modele danych (baza danych – 4+ tabele)
- **User**: `ID`, `username`, `passwordHash`, `token`, `role`
- **Book**: `ID`, `title`, `author`, `description`, `condition`, `ownerID`
- **ExchangeRequest**: `ID`, `offeredBookID`, `requestedBookID`, `status`, `date`
- **Message**: `ID`, `fromUserID`, `toUserID`, `content`, `timestamp`

### ✅ Interfejs webowy (MVC)
- Zarządzanie książkami (CRUD)
- Propozycje wymian (CRUD)
- Przeglądanie książek innych użytkowników
- Historia wymian i wyszukiwarka książek
- Nawigacja: Twoje książki, Przeglądaj, Propozycje, Wiadomości, Konto

### ✅ REST API
- Endpointy do obsługi książek i wymian
- Autoryzacja przez token w nagłówku lub parametrze (np. `?token=XYZ`)
- Przykładowy klient REST (np. program konsolowy w C# lub Python)

### ✅ Ciekawe zestawienia danych
- Ranking użytkowników z największą liczbą wymian
- Lista najczęściej wymienianych książek
- Historia wymian wybranego użytkownika
- Wyszukiwanie książek po tytule, autorze, stanie

### ✅ Inicjalizacja danych
- Automatyczne dodanie konta administratora przy pierwszym uruchomieniu
- Opcjonalnie: dodanie przykładowych książek

---

## 🚀 Jak uruchomić aplikację?

1. **Krok 1 – Klonowanie repozytorium**
   ```bash
   git clone https://github.com/twoje-konto/bookswap.git
   cd bookswap
2. **Krok 2 – Migracje bazy danych**
   dotnet ef database update
3. **Krok 3 - Uruchomienie aplikacji**
   dotnet run
4. **Krok 4 - Dostęp**
  Aplikacja dostępna pod https://localhost:5001
  Domyślna strona: formularz logowania (/Account/Login)

## 🔌 REST API – przykład użycia
**Przykładowe zapytanie GET do pobrania listy książek:**

```bash
GET /api/books?token=XYZ
```
**Przykładowe zapytanie POST do dodania książki:**
POST /api/books?token=XYZ
Content-Type: application/json

{
  "title": "Zbrodnia i kara",
  "author": "Fiodor Dostojewski",
  "description": "Klasyka literatury rosyjskiej",
  "condition": "dobry"
}

## 🧱Struktura projektu

```plaintext
BookSwap/
│
├── Controllers/
│   ├── AccountController.cs (MVC)
│   └── AuthApiController.cs (REST API)
│
├── Models/
│   └── User.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Views/
│   └── Account/
│       ├── Login.cshtml
│       ├── Register.cshtml
│       ├── ForgotPassword.cshtml
│       └── Welcome.cshtml
│
├── wwwroot/
├── Program.cs
├── Startup.cs (lub Program.cs w nowszych wersjach)
└── appsettings.json
```
## 👨‍💻 Autorzy
Paweł Rycerz i Igor Wadas

## 📎 Uwagi końcowe
Projekt spełnia wszystkie wymagania laboratoryjne:

- ✔️ 4+ tabele  
- ✔️ Rejestracja + logowanie  
- ✔️ Role (admin, user)  
- ✔️ REST API + token  
- ✔️ Sesje  
- ✔️ CRUD + ciekawe zestawienia  
- ✔️ Webowy interfejs użytkownika  
- ✔️ README + struktura + opis  

