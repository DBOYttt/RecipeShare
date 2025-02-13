# RecipeShare – Dokumentacja Projektu

## 1. Opis i cel projektu

**Cel:**  
RecipeShare to platforma społecznościowa dedykowana miłośnikom gotowania, która umożliwia użytkownikom dzielenie się własnymi przepisami kulinarnymi, inspirowanie się propozycjami innych oraz ułatwianie planowania zakupów. Aplikacja rozwiązuje problem poszukiwania nowych, ciekawych przepisów oraz integruje społeczność kulinarną, umożliwiając interakcje między użytkownikami (oceny, komentarze, udostępnienia).

**Zakres:**  
- **Rejestracja i logowanie:** Możliwość zakładania konta przez e-mail oraz logowania z wykorzystaniem mediów społecznościowych.
- **Dodawanie, edycja i usuwanie przepisów:** Użytkownicy mogą tworzyć własne przepisy, modyfikować je i usuwać.
- **Przeglądanie przepisów:** Lista dostępnych przepisów z wyszukiwarką i filtrowaniem według kategorii, czasu przygotowania, poziomu trudności itp.
- **System ocen i komentarzy:** Użytkownicy mogą oceniać przepisy w skali 1–5 gwiazdek oraz dodawać komentarze.
- **Generowanie listy zakupów:** Automatyczne tworzenie listy zakupów na podstawie wybranych składników przepisu.
- **Personalizacja profilu:** Zarządzanie profilem użytkownika, zapisywanie ulubionych przepisów oraz przeglądanie własnych publikacji.
- **Interakcje społecznościowe:** Możliwość polubienia, udostępnienia przepisu w mediach społecznościowych oraz otrzymywanie powiadomień o aktywnościach.

---

## 2. Wymagania funkcjonalne

### Lista funkcji:
- **Rejestracja i logowanie:**
  - Rejestracja konta przy użyciu adresu e-mail lub integracji z mediami społecznościowymi.
  - Logowanie do systemu z uwierzytelnianiem (np. za pomocą tokenów JWT).

- **Dodawanie przepisów:**
  - Użytkownik może tworzyć nowe przepisy, wprowadzając tytuł, opis, listę składników, instrukcje przygotowania oraz dodając zdjęcia.
  - Przypisywanie przepisu do jednej lub wielu kategorii (np. desery, obiady, szybkie przepisy).

- **Edycja i usuwanie przepisów:**
  - Użytkownik może edytować i usuwać własne przepisy.

- **Przeglądanie i wyszukiwanie przepisów:**
  - Wyświetlanie listy dostępnych przepisów na stronie głównej.
  - Wyszukiwarka oraz możliwość stosowania filtrów (według kategorii, czasu przygotowania, poziomu trudności).
  - Sortowanie przepisów (np. najpopularniejsze, najnowsze).

- **System ocen i komentarzy:**
  - Ocena przepisów (np. w skali 1–5 gwiazdek).
  - Dodawanie komentarzy oraz możliwość odpowiadania na komentarze.

- **Generowanie listy zakupów:**
  - Automatyczne generowanie listy zakupów na podstawie wybranych składników przepisu.

- **Interakcje społecznościowe:**
  - Opcja „polubienia” przepisu.
  - Udostępnianie przepisu na portalach społecznościowych (np. Facebook, Instagram).
  - Zapisywanie przepisów do ulubionych.

- **Powiadomienia:**
  - System powiadomień o nowych komentarzach, ocenach, aktualizacjach przepisów oraz wydarzeniach społecznościowych.

### Przykładowe scenariusze użycia:
- **Scenariusz 1:**  
  Użytkownik rejestruje się, loguje, dodaje nowy przepis wraz ze zdjęciami, przypisuje kategorię, przegląda przepisy, ocenia jeden z nich i generuje listę zakupów.

- **Scenariusz 2:**  
  Użytkownik przegląda przepisy, zapisuje interesujące danie do ulubionych oraz udostępnia je na swoich profilach społecznościowych.

- **Scenariusz 3:**  
  Użytkownik komentuje opublikowany przepis, a autor przepisu otrzymuje powiadomienie o nowym komentarzu.

---

## 3. Wymagania niefunkcjonalne

- **Wydajność:**
  - Aplikacja powinna działać płynnie zarówno na urządzeniach mobilnych, jak i desktopowych.
  - Odpowiedź serwera przy operacjach (np. pobieranie listy przepisów) nie powinna przekraczać 2 sekund.
  - System musi obsługiwać jednocześnie od 1000 do 10000 użytkowników.

- **Bezpieczeństwo:**
  - Dane użytkowników (w tym hasła) muszą być szyfrowane (np. z użyciem bcrypt).
  - Cała komunikacja między aplikacją a serwerem powinna odbywać się przez HTTPS.
  - Uwierzytelnianie użytkowników przy użyciu tokenów (np. JWT) oraz kontrola dostępu do zasobów.

- **Niezawodność:**
  - System powinien być dostępny 24/7, z minimalnymi przerwami technicznymi.
  - Regularne tworzenie kopii zapasowych bazy danych oraz wdrożenie procedur odzyskiwania po awarii.

- **Użyteczność i interfejs (UX/UI):**
  - Intuicyjny i responsywny interfejs, dostosowany do różnych rozmiarów ekranów.
  - Spójny design na wszystkich platformach, z uwzględnieniem specyfiki interfejsów dotykowych.

- **Zgodność:**
  - Aplikacja musi być kompatybilna z urządzeniami mobilnymi (Android, iOS) oraz desktopowymi (Windows, macOS) dzięki zastosowaniu .NET MAUI.
  - W razie potrzeby możliwe wdrożenie wersji web (np. przy użyciu ASP.NET Core).

---

## 4. Technologia i architektura

### Języki programowania i frameworki:
- **.NET MAUI:**  
  Framework do tworzenia aplikacji wieloplatformowych (Android, iOS, Windows, macOS) z wykorzystaniem C# i XAML.
  
- **C#:**  
  Język programowania używany do logiki aplikacji.

- **XAML:**  
  Język deklaratywny do projektowania interfejsu użytkownika.

- **Baza danych:**  
  SQL Server lub SQLite, w zależności od wymagań i skali projektu.

### Architektura systemu:
- **Warstwa klienta:**  
  Aplikacja mobilna/desktopowa zbudowana w .NET MAUI, odpowiedzialna za interakcję z użytkownikiem.
  
- **Warstwa serwera:**  
  RESTful API stworzone w ASP.NET Core, obsługujące logikę aplikacji oraz operacje na danych.

- **Baza danych:**  
  Relacyjna baza danych przechowująca informacje o użytkownikach, przepisach, komentarzach, ocenach oraz ulubionych przepisach.

### Struktura bazy danych (przykładowe tabele):
- **Users:**  
  `Id`, `Email`, `Hasło` (zaszyfrowane), `Nazwa użytkownika`, `Data rejestracji`, `Profil` (zdjęcie, opis).

- **Recipes:**  
  `Id`, `Tytuł`, `Opis`, `Składniki`, `Instrukcje`, `Zdjęcia`, `Kategoria`, `Data dodania`, `Id autora`.

- **Comments:**  
  `Id`, `RecipeId`, `UserId`, `Treść`, `Data dodania`.

- **Ratings:**  
  `Id`, `RecipeId`, `UserId`, `Ocena`, `Data dodania`.

- **Favorites:**  
  `Id`, `UserId`, `RecipeId`.

---

## 5. Interfejs użytkownika (UI/UX)

### Makiety / prototypy:
- **Strona główna:**  
  Przegląd najnowszych oraz najpopularniejszych przepisów, pasek wyszukiwania, kategorie.

- **Ekran szczegółowy przepisu:**  
  Prezentacja tytułu, opisu, listy składników, instrukcji przygotowania, zdjęć, ocen oraz komentarzy.

- **Ekran dodawania/edycji przepisu:**  
  Formularz umożliwiający wprowadzenie tytułu, opisu, składników, instrukcji oraz dodanie zdjęć.

- **Profil użytkownika:**  
  Widok z informacjami o użytkowniku, zapisanymi przepisami, ulubionymi przepisami oraz możliwością edycji profilu.

- **Ekran listy zakupów:**  
  Lista składników wygenerowana na podstawie wybranego przepisu z możliwością edycji.

### Nawigacja:
- **Główne menu:**  
  Umieszczone na dole (w wersjach mobilnych) lub jako pasek boczny (w wersjach desktopowych) z dostępem do:
  - Strony głównej
  - Dodawania przepisu
  - Ulubionych przepisów
  - Profilu użytkownika

- **Pasek nawigacyjny:**  
  Umożliwia przechodzenie do szczegółowego widoku przepisu oraz powrót do poprzedniej sekcji.

### Style i standardy:
- Minimalistyczny, nowoczesny design z czytelnym układem treści.
- Spójna paleta kolorów, odpowiednio dobrane czcionki oraz intuicyjne ikony.
- Responsywność interfejsu zapewniająca optymalne doświadczenie zarówno na urządzeniach dotykowych, jak i tradycyjnych.

---

## 6. Harmonogram i etapy realizacji

### Etapy projektu:
1. **Analiza i projektowanie (2 tygodnie):**
   - Zbieranie i analiza wymagań.
   - Opracowanie makiet i prototypów UI.
   - Projekt architektury systemu.

2. **Implementacja warstwy backend (3 tygodnie):**
   - Konfiguracja serwera API przy użyciu ASP.NET Core.
   - Projektowanie i wdrożenie bazy danych.
   - Implementacja mechanizmów autoryzacji i uwierzytelniania.

3. **Implementacja warstwy frontend w .NET MAUI (4 tygodnie):**
   - Tworzenie interfejsu użytkownika przy użyciu MAUI i XAML.
   - Integracja aplikacji z API (CRUD dla przepisów, komentarzy, ocen).
   - Implementacja mechanizmu generowania listy zakupów.

4. **Testy i poprawki (2 tygodnie):**
   - Testowanie funkcjonalności na wszystkich docelowych platformach.
   - Przeprowadzenie testów wydajnościowych i bezpieczeństwa.
   - Wprowadzanie poprawek oraz optymalizacji.

### Szacowany czas realizacji:
- Całkowity czas realizacji: około **11–12 tygodni**.

---

## 7. Zarządzanie ryzykiem

### Identyfikacja ryzyk i metody ich minimalizacji:
- **Opóźnienia w realizacji:**
  - *Ryzyko:* Problemy z integracją API i aplikacją MAUI mogą wpłynąć na harmonogram.
  - *Minimalizacja:* Regularne spotkania zespołu, ścisłe monitorowanie postępów, elastyczny przydział zasobów oraz priorytetyzacja kluczowych funkcjonalności.

- **Problemy z kompatybilnością między platformami:**
  - *Ryzyko:* Różnice w działaniu na Android, iOS, Windows i macOS.
  - *Minimalizacja:* Testowanie na rzeczywistych urządzeniach oraz wykorzystanie platformowo niezależnych komponentów MAUI.

- **Bezpieczeństwo danych:**
  - *Ryzyko:* Możliwość ataków lub wycieków danych użytkowników.
  - *Minimalizacja:* Szyfrowanie haseł, stosowanie HTTPS, regularne audyty bezpieczeństwa oraz wdrożenie mechanizmów uwierzytelniania.

- **Błędy w implementacji:**
  - *Ryzyko:* Wprowadzenie błędów przy wdrażaniu nowych funkcji.
  - *Minimalizacja:* Prowadzenie testów jednostkowych, integracyjnych oraz code review, wdrożenie ciągłej integracji (CI).

---

## 8. Dokumentacja kodu i standardy

- **Dokumentacja kodu:**
  - Kod źródłowy powinien być odpowiednio komentowany, szczególnie w kluczowych modułach (np. obsługa przepisów, autoryzacja, integracja API).
  - Automatyczne generowanie dokumentacji (np. XML Comments w C#) i utrzymywanie jej w repozytorium.
  - Instrukcje konfiguracji środowiska deweloperskiego, budowania aplikacji oraz wdrażania zmian powinny być dołączone do repozytorium.

- **Standardy kodowania:**
  - Stosowanie konwencji nazewniczych zgodnych z wytycznymi Microsoft dla języka C#.
  - Regularne przeglądy kodu (code review) oraz użycie narzędzi do analizy statycznej (np. SonarQube).
  - Utrzymanie kontroli wersji przy użyciu Git z ustalonymi zasadami commitowania i branchingu.

---

## 9. Podsumowanie

**Główne założenia:**  
- **RecipeShare** to aplikacja społecznościowa dla miłośników gotowania, umożliwiająca publikowanie, przeglądanie, ocenianie i komentowanie przepisów kulinarnych.
- Użytkownicy mogą generować listy zakupów na podstawie przepisów, zapisywać swoje ulubione dania oraz dzielić się przepisami w mediach społecznościowych.
- Projekt oparty jest o **.NET MAUI**, co gwarantuje wsparcie dla wielu platform oraz spójne doświadczenie użytkownika niezależnie od urządzenia.
- System składa się z warstwy klienta (aplikacja MAUI), warstwy serwera (API w ASP.NET Core) oraz relacyjnej bazy danych.

**Dalsze perspektywy rozwoju:**  
- Rozbudowa systemu rekomendacji na podstawie preferencji użytkowników i historii ocen.
- Integracja z zewnętrznymi bazami danych przepisów kulinarnych oraz dostawcami składników.
- Rozwój funkcji społecznościowych, takich jak organizacja konkursów kulinarnych czy live cooking sessions.
- Wprowadzenie trybu offline umożliwiającego przeglądanie zapisanych przepisów bez aktywnego połączenia z internetem.
