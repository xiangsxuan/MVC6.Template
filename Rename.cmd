@echo off
cd tools/MvcTemplate.Rename
dotnet restore
cd ../..
dotnet run -p tools/MvcTemplate.Rename/MvcTemplate.Rename.csproj

if exist tools ( rmdir /s /q tools )
if exist CONTRIBUTING.md ( del CONTRIBUTING.md )
if exist LICENSE.txt ( del LICENSE.txt )
if exist README.md ( del README.md )
del /s Rename.cmd