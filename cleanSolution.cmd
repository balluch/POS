REM @echo off
REM L�scht alle tempor�ren Visual Studio Dateien
 
FOR /D /R %%d IN (*) DO (
  rd /S /Q "%%d\.vs"
  rd /S /Q "%%d\.vscode"
  rd /S /Q "%%d\bin"
  rd /S /Q "%%d\obj"
)


