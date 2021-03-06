# ASP.net Core MVC

Wesentlich ist die Entkoppelung der Komponenten. Je feiner und granularer Komponenten gestaltet werden und je besser sie entkoppelt sind, umso flexibler wird das Softwaregerüst.

## Services

* Eigenes Projekt (.Net Core Class Library)
* Eigenen Namespace (bzw. Folder) `Services` erstellen

### Service anlegen

Dafür im generiten Namespace eine neue Klasse `SchoolclassService` erstellen. Code kann nun aus dem Controller in diese Klasse transferiert werden. Die Abhängigkeiten zum DB-Context können anschließend aus dem Controller entfernen.

Im einfachsten Fall kann man die LinQ-Query aus dem Controller in eine neue Methode `GetAllAsync()` ohne Parameter kopieren.

Danach wird die Methode Schritt für Schritt um weitere Features erweitert.

## DB-Context injizieren

Service benötigt den DB-Context:

```C#
private readonly TestsAdministratorContext _context;
private readonly ILogger<SchoolclassService> _logger;

public SchoolclassService(TestsAdministratorContext context, ILogger<SchoolclassService> logger)
{
    _context = context;
    _logger = logger;
}
```

```C#
public async Task<IEnumerable<Schoolclass>> GetAllAsync()
{
    return await _context.Schoolclass
        .Include(s => s.C_ClassTeacherNavigation)
        .ToListAsync();
}
```

### Create-Methode

```C#
public async Task<Schoolclass> CreateAsync(Schoolclass newModel)
{
    try
    {
        _context.Add(newModel);
        await _context.SaveChangesAsync();
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogError(ex, "Methode CreateAsync(Schoolclass) ist fehlgeschlagen!");
        throw new ServiceLayerException("Methode CreateAsync(Schoolclass) ist fehlgeschlagen!", ex);
    }
    return newModel;
}
```

### Delete.-Methode

```C#
public async Task DeleteAsync(string id)
{
    try
    {
        var schoolclass = await _context.Schoolclass.FindAsync(id);
        _context.Schoolclass.Remove(schoolclass);
        await _context.SaveChangesAsync();
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogError(ex, "Methode DeleteAsync(long) ist fehlgeschlagen!");
        throw new ServiceLayerException("Methode DeleteAsync(long) ist fehlgeschlagen!", ex);
    }
}
```

### Edit-Methode

```C#
public async Task<Schoolclass> EditAsync(string id, Schoolclass newModel)
{
    if (id != newModel.C_ID)
    {
        throw new KeyNotFoundException("Datensatz wurde nicht gefunden!");
    }

    try
    {
        Schoolclass existingSchoolclass = _context.Schoolclass.Find(id);
        if (existingSchoolclass == null)
        {
            throw new KeyNotFoundException("Datensatz wurde nicht gefunden!");
        }
        else
        {
            existingSchoolclass.C_ClassTeacher = newModel.C_ClassTeacher;
            existingSchoolclass.C_Department = newModel.C_Department;

            _context.Update(existingSchoolclass);
            await _context.SaveChangesAsync();
        }
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogError(ex, "Methode EditAsync(long, Schoolclass) ist fehlgeschlagen!");
        throw new ServiceLayerException("Methode EditAsync(long, Schoolclass) ist fehlgeschlagen!", ex);
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!EntityExists(newModel.C_ID))
        {
            throw new KeyNotFoundException("Datensatz wurde nicht gefunden!");
        }
        throw;
    }
    return newModel;
}

private bool EntityExists(string id)
{
    return _context.Schoolclass.Any(e => e.C_ID == id);
}
```

Im Service wird nicht zwischen GET und POST unterschieden. Diese Methoden manipulieren nur die Datenbank. Entsprechend dem generischen Ansatz sollen die Methoden auch für Applikationen, die über ein anders Front End verfügen, verwendbar sein.

## Controller

Im Controller werden die Methoden dann noch aufgerufen. Auch hier wird eine Instanz vom Service benötigt:

```C#
private readonly ISchoolclassService _schoolclassService;

public SchoolclassController(ISchoolclassService schoolclassService)
{
	_schoolclassService = schoolclassService;
}
```

### Details

Dafür wird eine Methode `GetSingleOrDefaultAsync` erstellt. Die Namensgebung entspricht der üblichen Konvention.

```C#
public async Task<IActionResult> Details(string id)
{
    if (id == null)
    {
        return NotFound();
    }

    var model = await _schoolclassService.GetSingleOrDefaultAsync(id);

    if (model == null)
    {
        return NotFound();
    }

    return View(model);
}
```

### Create-POST

```C#
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("C_ID,C_Department,C_ClassTeacher")] Schoolclass schoolclass)
{
    if (ModelState.IsValid)
    {
        try
        {
            await _schoolclassService.CreateAsync(schoolclass);
            return RedirectToAction(nameof(Index));
        }
        catch (ServiceLayerException)
        {
            return StatusCode(500);
        }
    }
    ViewData["C_ClassTeacher"] = new SelectList(await _teacherService.GetAllAsync(), "T_ID", "T_ID", schoolclass.C_ClassTeacher);
    return View(schoolclass);
}
```

### Edit-GET

```C#
public async Task<IActionResult> Edit(string id)
{
    if (id == null)
    {
        return NotFound();
    }

    var schoolclass = await _schoolclassService.GetSingleOrDefaultAsync(id);

    if (schoolclass == null)
    {
        return NotFound();
    }
    ViewData["C_ClassTeacher"] = new SelectList(await _teacherService.GetAllAsync(), "T_ID", "T_ID", schoolclass.C_ClassTeacher);
    return View(schoolclass);
}
```

### Edit-POST

```C#
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(string id, [Bind("C_ID,C_Department,C_ClassTeacher")] Schoolclass schoolclass)
{
    if (id != schoolclass.C_ID)
    {
        return Conflict();
    }

    if (ModelState.IsValid)
    {
        try
        {
            await _schoolclassService.EditAsync(id, schoolclass);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ServiceLayerException)
        {
            return StatusCode(500);
        }
        return RedirectToAction(nameof(Index));
    }
    ViewData["C_ClassTeacher"] = new SelectList(await _teacherService.GetAllAsync(), "T_ID", "T_ID", schoolclass.C_ClassTeacher);
    return View(schoolclass);
}
```

### Delete GET

```C#
public async Task<IActionResult> Delete(string id)
{
	if (id == null)
	{
		return NotFound();
	}

	var model = await _schoolclassService.GetSingleOrDefaultAsync(id);

	if (model == null)
	{
		return NotFound();
	}

	return View(model);
}
```

### Delete POST

```C#
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(string id)
{
    try
    {
        await _schoolclassService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
    catch (ServiceLayerException)
    {
        return StatusCode(500);
    }
}
```

## Interfaces

Hier wurden bereits Interfaces als Typen für die jeweiligen Instanzen verwendet. Es sollte immer eine Trennung durch die Erstellung von Interfaces vorgenommen werden.

Registrieren der Services in der `Startup.cs`:

```C#
services.AddTransient<ISchoolclassService, SchoolclassService>();
```

## DTO

Für den Datenaustausch kann anstatt einem Objekt der jeweiligen Modelklasse ein Data Transfer Object (DTO) verwendet werden.
