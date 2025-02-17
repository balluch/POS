# REST Webservices mit ASP.NET Core

## Inhalt

- [GetRoutes](01_GetRoutes/README.md)
- [PostPutDeleteRoutes](02_PostPutDeleteRoutes/README.md)

## Anlegen eines ASP.NET Core WebAPI

Um eine REST API mit ASP.NET Core zu erstellen, wird in Visual Studio 2022 mittels *Create a new project* die Vorlage 
*ASP.NET Core Web API* gewählt. Wenn du auf Linux, macOS oder in der Konsole von Windows arbeiten möchtest, kannst du ein API
Projekt auch in der Eingabeaufforderung anlegen. Dafür erstelle zuerst ein Verzeichnis mit dem
Projektnamen und erstelle danach die Applikation darin.

```text
md GetRoutesDemo
cd GetRoutesDemo
dotnet new webapi
```

Im Projektordner kann einfach auf der Konsole mit *dotnet run* der Server gestartet werden.
Er zeigt den Port und das Netzwerkinterface aus der Konfiguration an. Alternativ kann der Server 
in Visual Studio durch das Ausführen des Projektes gestartet werden.

## Setzen der Ports und der Startadresse

Der Server hört auf die Ports, die in der Datei *Properties/launchSettings.json* spezifiziert sind. 
Folgende Konfiguration kann verwendet werden. Falls die Ports schon belegt sind,
können sie natürlich durch andere Werte ersetzt werden.

Wenn der Server von Visual Studio aus gestartet wird, wird auch das Property *launchUrl* ausgelesen.
Es gibt die Adresse an, die beim Öffnen des Browsers nach dem Serverstart erscheint.

```javascript
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "GetRoutesDemo": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "api/pupil",
      "applicationUrl": "https://localhost:443;http://localhost:80",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```