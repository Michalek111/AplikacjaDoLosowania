# Aplikacja Do Losowania Drużyn

Aplikacja webowa stworzona w technologii ASP.NET MVC, umożliwiająca losowanie drużyn, przewidywanie wyników, zarządzanie meczami oraz statystykami graczy. Wspiera rolę administratora i użytkownika, a dane przechowywane są w bazie danych Microsoft Azure SQL.

## Funkcje aplikacji

- **Losowanie drużyn** – wybór graczy i automatyczne tworzenie dwóch drużyn.
- **Szansa na wygraną** – predykcja zwycięstwa drużyn na podstawie `ML.NET`.
- **Zatwierdzanie i edycja meczu** – z walidacją wyników w stylu gry CS2 (np. 13:11, 16:14).
- **Historia meczów** – przegląd wszystkich rozegranych gier.
- **Panel administratora** – edycja wyniku, zmiana mapy oraz wymiana graczy 1 na 1 między drużynami.
- **Baza danych na Microsoft Azure SQL** – pełna integracja z chmurą.
- **Autoryzacja i logowanie** – użytkownicy i administratorzy z różnymi uprawnieniami.

---

##  Technologie

- `ASP.NET Core MVC`
- `Entity Framework Core`
- `SQL Server` (lokalnie oraz Azure)
- `ML.NET`
- `Bootstrap`
- `JavaScript (Fetch API)`
- `Azure SQL` + `Microsoft Entra ID` (logowanie)


## Używanie aplikacji

Aplikacji można używać pod adresem:
https://aplikacjadolosowania-azdgezhsb0e7fvcq.polandcentral-01.azurewebsites.net
