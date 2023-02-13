# Servergerendertes HTML mit ASP.NET Core Razor Pages

## 2 Strukturen - 1 Ziel

Mit ASP.NET Core kannst du serverseitig gerenderte Webapps auf 2 Arten erstellen:

### MVC (Model View Controller): dotnet new mvc

MVC Projekte haben 3 Ordner:

```
Webapp
  |
  +--- Controllers
  +--- ViewModels
  +--- Views
```

Der Controller ist der Endpunkt für das Routing. Mit *return View(viewmodel)* liefert er eine
Razor View an den Client aus, die das gerenderte HTML enthält.

### MVVM (Model - View - Viewmodel) mit Razor Pages: dotnet new webapp

Die Razor Pages Applikation besteht aus einer Razor View (die cshtml Datei) und einem entsprechenden
Viewmodel (cshtml.cs Datei). Das Viewmodel behandelt die Requests selbst, es ist also Controller und
(klassisches) Viewmodel in einem. Dadurch sind nur 2 Dateien notwendig:

```
Webapp
  |
  +--- Pages
         |
         +---- Index.cshtml
         +---- Index.cshtml.cs
```

### Gegenüberstellung der Konzepte

| Razor Pages                                                     | MVC                                                                          |
| --------------------------------------------------------------- | ---------------------------------------------------------------------------- |
| PageModel Klasse                                                | Die Properties kommen in ein ViewModel, die Methoden in den Controller.      |
| Page Handler wie *OnGet()*, *OnPost()*, *OnPostNewStore()*, ... | Controller Actions mit Routing Parameter wie *HttpGet*, *HttpPost(...)*, ... |
| *BindProperty* Annotation                                       | Modelbinding über den Parameter der POST Actionmethode.                      |
| Page Filter (*OnPageHandlerExecuting()*, …)                     | *OnActionExecuting* bzw. *OnActionExecuted* Methode im Controller.           |
| return Page()                                                   | return View(viewmodel)                                                       |
