# Blazor Hands-on: Sock Store Webshop

## Voorbereiding

Zorg ervoor dat je het volgende hebt geïnstalleerd:
* .NET 9 SDK (controleer met `dotnet --version`)
* Een IDE naar keuze (Visual Studio 2022+, VS Code met de C# Dev Kit, of JetBrains Rider)

## Oefeningen

---

### Oefening 1: Project Setup & Eerste Verkenning (5-10 min)

1.  **Nieuw Project Aanmaken:**
    * Open IDE (Visual Studio / Rider)
      * Maak een nieuwe solution aan
      * Kies voor Blazor Webassembly App
      * Noem het project SockStore
    * Voor Visual Studio Code:
      * Open terminal
      * Navigeer naar gewenste folder
      * Creëer een nieuw Blazor WebAssembly project met de naam `SockStore` via het commando: `dotnet new blazorwasm -o SockStore`
      * Navigeer in je terminal naar de aangemaakte `SockStore` map.

2.  **Project Openen en Verkennen:**
    * Open het `SockStore.csproj` bestand (of de map) in je IDE.
    * Neem even de tijd om de projectstructuur te bekijken en de rol van de volgende bestanden/mappen te begrijpen:
        * `Program.cs`: Startpunt van de applicatie, configuratie van services.
        * `wwwroot/`: Statische bestanden (CSS, JavaScript, afbeeldingen), inclusief `index.html`.
        * `Pages/`: Bevat de routeerbare Razor componenten (pagina's).
        * `Shared/`: Bevat herbruikbare Razor componenten, zoals `MainLayout.razor` en `NavMenu.razor`.
        * `_Imports.razor`: Globale `@using` directives voor componenten.
        * `App.razor`: Configureert de Blazor Router.

3.  **Project Uitvoeren:**
    * Start de applicatie vanuit je IDE (meestal met een "Run" of "Debug" knop, of via `dotnet run` in de terminal).
    * Bekijk de standaard Blazor template in je browser en navigeer door de voorbeeldpagina's.

---

### Oefening 2: Product Data en Service Logica (15-20 min)

We definiëren nu de data voor onze sokken en de logica om deze op te halen.

1.  **Product Model (`Sock.cs`):**
    * Maak een nieuwe map `Models` in je project.
    * Voeg hieraan een C# class `Sock.cs` toe met de volgende properties:
        ```csharp
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        ```

2.  **Product Service (`IProductService` en `ProductService.cs`):**
    * Maak een nieuwe map `Services` in je project.
    * Definieer een interface `IProductService.cs` met de volgende method signatures:
        ```csharp
        using SockStore.Models; // Zorg voor de juiste using
        using System.Collections.Generic;
        using System.Threading.Tasks;

        public interface IProductService
        {
            Task<List<Sock>> GetProductsAsync();
            Task<Sock> GetProductByIdAsync(int id);
        }
        ```
    * Implementeer deze interface in een class `ProductService.cs`.
    * Maak in `ProductService` een private `List<Sock>` aan en initialiseer deze met 3-4 voorbeeld `Sock` objecten (hardcoded data).
        * *Tip: Voor `ImageUrl`, gebruik placeholders zoals `images/socks1.jpg`. Maak een `images` map aan in `wwwroot` en plaats daar eventueel een paar (royalty-vrije) voorbeeldafbeeldingen.*
    * Implementeer de `GetProductsAsync` en `GetProductByIdAsync` methoden om data uit deze hardcoded lijst te retourneren. Gebruik `Task.FromResult()` om een voltooide Task te retourneren voor deze synchrone, hardcoded data.

3.  **Service Registratie (Dependency Injection):**
    * Open `Program.cs`.
    * Registreer de `ProductService` voor Dependency Injection. Een `Scoped` lifetime is een goede algemene keuze.
        * *Hint: `builder.Services.AddScoped<IProductService, ProductService>();`*

---

### Oefening 3: Producten Tonen - De Productlijst Pagina (20-25 min)

Nu maken we een pagina om alle sokken te tonen.

1.  **ProductLijst Component (`ProductList.razor`):**
    * Maak in de `Pages` map een nieuw Razor component genaamd `ProductList.razor`.
    * Maak dit component routeerbaar via de URL `/products` met de `@page` directive.

2.  **Service Injecteren en Data Laden:**
    * Injecteer `IProductService` in het component met de `@inject` directive.
    * Definieer een private C# field/property in de `@code` block om de lijst van `Sock` objecten op te slaan.
    * Overschrijf de `OnInitializedAsync` lifecycle methode om de productdata asynchroon te laden via de geïnjecteerde service en op te slaan in je field/property.

3.  **Producten Weergeven in HTML:**
    * Voeg in de HTML-sectie een check toe om een "Loading..." bericht te tonen zolang de productlijst nog niet geladen is (d.w.z., `null` is).
    * Zodra de data beschikbaar is, gebruik een `@foreach` loop om over de lijst met sokken te itereren.
    * Toon voor elk product de `ImageUrl` (in een `<img>` tag), `Name`, `Price`, en een link "View Details".
        * *Styling Tip: Gebruik de CSS classes uit het `app.css` bestand (indien overgenomen) zoals `.card`, `.card-img-top`, etc., voor een betere weergave.*

4.  **Navigatie Toevoegen:**
    * Open het `Shared/NavMenu.razor` component.
    * Voeg een nieuw `NavLink` element toe dat linkt naar de `/products` pagina.

5.  **Testen:**
    * Draai de applicatie en navigeer naar je productlijst pagina. Controleer of de producten correct worden weergegeven.

---

### Oefening 4: Product Details Tonen (15-20 min)

Gebruikers moeten de details van een specifieke sok kunnen bekijken.

1.  **ProductDetail Component (`ProductDetails.razor`):**
    * Maak in de `Pages` map een nieuw Razor component `ProductDetails.razor`.
    * Definieer een route voor dit component die een `int` parameter `ProductId` accepteert, bijvoorbeeld: `@page "/productdetails/{ProductId:int}"`.

2.  **Route Parameter Ontvangen:**
    * Declareer een public C# property `ProductId` in de `@code` block en voorzie deze van het `[Parameter]` attribuut, zodat Blazor de waarde uit de route hieraan kan binden.
    * Definieer ook een private field/property voor het `Sock` object dat getoond moet worden.

3.  **Service Injecteren en Specifiek Product Laden:**
    * Injecteer `IProductService`.
    * Overschrijf de `OnParametersSetAsync` lifecycle methode. Gebruik de waarde van de `ProductId` parameter om het specifieke `Sock` object op te halen via de `ProductService` en sla dit op.

4.  **Details Weergeven:**
    * Voeg HTML markup toe om de `Name`, `ImageUrl`, `Description`, en `Price` van het geladen product te tonen.
    * Toon een "Loading..." of "Product niet gevonden" bericht als het productobject `null` is.
    * Voeg een link toe om terug te navigeren naar de `/products` lijst (bijvoorbeeld met `NavigationManager` of een simpele `<a href>`).

5.  **Linken vanaf Productlijst:**
    * Ga terug naar `ProductList.razor`.
    * Wijzig de "View Details" link voor elk product zodat deze navigeert naar de detailpagina, waarbij de `Id` van het product wordt meegegeven in de URL.
        * *Hint: `<a href="/productdetails/@product.Id">View Details</a>`*

6.  **Testen:**
    * Draai de applicatie. Klik op "View Details" bij een product. Controleer of de juiste detailpagina wordt getoond met de correcte informatie.

---

### Oefening 5: Winkelwagen & Eenvoudig State Management (20-25 min)

Laten we een basis winkelwagen implementeren en kijken hoe de staat ervan beheerd kan worden.

1.  **CartItem Model (Optioneel, voor aantallen):**
    * Overweeg een `CartItem.cs` model in de `Models` map als je meer wilt bijhouden dan alleen het product, bijvoorbeeld `ProductId` en `Quantity`. Voor nu kun je ook direct `Sock` objecten in de cart opslaan.

2.  **Cart Service (`ICartService` en `CartService.cs`):**
    * Definieer een `ICartService` en een implementatie `CartService.cs` in de `Services` map.
    * De service moet minimaal methoden hebben om:
        * Een product (of `CartItem`) toe te voegen.
        * Alle items in de winkelwagen op te halen.
        * Het totaal aantal unieke items of de totale hoeveelheid te retourneren.
    * Gebruik een `private List<Sock>` (of `List<CartItem>`) in de `CartService` om de items op te slaan.
    * Registreer `ICartService` en `CartService` in `Program.cs`. Een `Scoped` lifetime is hier geschikt om de winkelwagen per gebruiker/sessie te behouden.

3.  **"Voeg toe aan Winkelwagen" Functionaliteit:**
    * Injecteer `ICartService` in `ProductDetails.razor`.
    * Voeg een "Voeg toe aan Winkelwagen" knop toe.
    * Koppel een `@onclick` event aan deze knop dat het huidige product toevoegt aan de winkelwagen via de `CartService`.

4.  **Winkelwagen Weergave en State Updates:**
    * **Winkelwagen Indicator:** Injecteer `ICartService` in `Shared/NavMenu.razor`. Voeg een indicator toe (bijv. "Winkelwagen (@CartService.GetItemCount())") die het aantal items toont.
    * **State Management Aspect:**
        * Wanneer een item wordt toegevoegd aan de winkelwagen vanuit `ProductDetails.razor`, hoe zorg je ervoor dat de `NavMenu` de update van het aantal items reflecteert?
        * *Hint 1: De `CartService` kan een `event Action OnCartChanged;` definiëren. Componenten (zoals `NavMenu`) kunnen hierop subscriben. Wanneer de cart verandert, roept de service `OnCartChanged?.Invoke()` aan. In de event handler in `NavMenu` roep je `StateHasChanged()` aan.*
        * *Hint 2 (eenvoudiger voor nu): Je kunt ook overwegen om na het toevoegen van een item, een navigatie naar dezelfde pagina te forceren of een andere methode te gebruiken om de UI te verversen, hoewel een event-gebaseerde aanpak cleaner is voor state management.* Voor deze oefening kun je starten met het idee dat de `NavMenu` bij elke render de count opnieuw ophaalt. Voor een directe update na een actie op *een andere pagina* is een expliciet signaal (event) of een gedeelde observable state nodig.
    * **(Optioneel) Winkelwagen Pagina:** Maak een `ShoppingCart.razor` pagina die de inhoud van de winkelwagen toont.

---

### Oefening 6: JavaScript Interop - Winkelwagen Opslaan in `localStorage` (20-25 min)

We gaan de winkelwagen persistent maken met `localStorage` via JavaScript interop.

1.  **JavaScript Functies:**
    * Maak een nieuw JavaScript bestand in `wwwroot/js/`, bijvoorbeeld `cartInterop.js`.
    * Definieer hierin twee JavaScript functies:
        * `saveCart(cartData)`: Slaat de `cartData` (een JSON string) op in `localStorage` onder een key (bv. "sockCart").
        * `loadCart()`: Haalt de cart data op uit `localStorage` en retourneert deze. Als er niks is, retourneer `null`.
    * Zorg ervoor dat dit script geladen wordt in `wwwroot/index.html` (`<script src="js/cartInterop.js"></script>`).

2.  **JSInterop in CartService:**
    * Injecteer `IJSRuntime` in je `CartService.cs`.
    * **Opslaan:** Pas de methode in `CartService` die een item toevoegt (en eventueel andere methoden die de cart wijzigen) aan:
        * Na het wijzigen van de interne lijst van items, serialiseer de lijst naar een JSON string (bijv. met `System.Text.Json.JsonSerializer`).
        * Roep de `saveCart` JavaScript functie aan via `jsRuntime.InvokeVoidAsync("saveCart", jsonCartData)`.
    * **Laden:** In de constructor of een initialisatiemethode van `CartService`:
        * Roep de `loadCart` JavaScript functie aan via `await jsRuntime.InvokeAsync<string>("loadCart")`.
        * Als er data wordt geretourneerd, deserialiseer de JSON string terug naar je `List<Sock>` (of `List<CartItem>`) en vul hiermee de interne lijst.
        * *Let op: De constructor van een service kan geen async calls doen die wachten op `IJSRuntime` als de service te vroeg wordt geïnstantieerd. Een aparte `InitializeAsync` methode die je aanroept vanuit `OnInitializedAsync` in een root component (zoals `App.razor` of `MainLayout.razor`) is een robuustere aanpak voor het laden van de cart bij de start van de app.*

3.  **Testen:**
    * Voeg items toe aan de winkelwagen. Sluit de browser tab en open de applicatie opnieuw. Wordt de winkelwagen hersteld?
    * Controleer `localStorage` in de developer tools van je browser om te zien of de data correct wordt opgeslagen.

---

### Oefening 7: Eenvoudige Authenticatie Flow (Mock) (25-30 min)

Implementeer een basis login/logout functionaliteit.

1.  **AuthService (`IAuthService` en `AuthService.cs`):**
    * Definieer een `IAuthService` en `AuthService.cs` in de `Services` map.
    * De service moet bijhouden of een gebruiker is ingelogd en wie de gebruiker is (bv. `CurrentUser` property).
    * Implementeer:
        * `Task<bool> LoginAsync(string username, string password)`: Controleer hardcoded credentials (bv. "admin"/"password"). Zet `CurrentUser` en retourneer `true` bij succes.
        * `Task LogoutAsync()`: Reset `CurrentUser` en de login status.
        * `IsUserLoggedIn()`: Property of methode die `true` retourneert als er een gebruiker is ingelogd.
    * Definieer een `event Action OnAuthenticationChanged;` om componenten te notificeren van login/logout. Roep `OnAuthenticationChanged?.Invoke()` aan in `LoginAsync` en `LogoutAsync`.
    * Registreer de service (bv. `Scoped`) in `Program.cs`.

2.  **Login Pagina (`LoginPage.razor`):**
    * Maak een `LoginPage.razor` in de `Pages` map (`@page "/login"`).
    * Bouw een simpel formulier met input velden voor gebruikersnaam en wachtwoord (gebruik `<InputText>`). Bind deze aan properties in je component.
    * Injecteer `IAuthService` en `NavigationManager`.
    * Bij het submitten van het formulier, roep `AuthService.LoginAsync()` aan.
    * Als login succesvol is, navigeer naar de hoofdpagina (`/`) met `NavigationManager`. Toon anders een foutmelding.

3.  **Login/Logout Status in NavMenu:**
    * Injecteer `IAuthService` in `Shared/NavMenu.razor`.
    * Implementeer `IDisposable` in `NavMenu.razor` om te unsubscriben van `OnAuthenticationChanged`.
    * In `OnInitialized`, subscribe op `AuthService.OnAuthenticationChanged` en roep `StateHasChanged()` aan in de handler.
    * Toon conditioneel:
        * Een "Login" link als de gebruiker niet is ingelogd.
        * "Welkom, @AuthService.CurrentUser!" en een "Logout" knop als de gebruiker wel is ingelogd.
    * De "Logout" knop roept `AuthService.LogoutAsync()` aan en navigeert daarna naar de loginpagina of hoofdpagina.

4.  **Beschermde Content (Optioneel):**
    * Op een pagina (bv. de winkelwagen pagina), injecteer `IAuthService` en `NavigationManager`.
    * In `OnInitializedAsync`, als de gebruiker niet is ingelogd (`!AuthService.IsUserLoggedIn()`), navigeer dan naar `/login` met `NavigationManager.NavigateTo("/login", true);` (forceer een redirect).
    * *Alternatief: Gebruik de ingebouwde `AuthorizeView` component als je dieper op Blazor security wilt ingaan, maar voor een mock is dit een eenvoudigere start.*

5.  **Testen:**
    * Probeer in en uit te loggen. Verandert de UI in `NavMenu` correct? Word je omgeleid als je een "beschermde" pagina probeert te bezoeken zonder ingelogd te zijn?

---