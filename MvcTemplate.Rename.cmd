@echo off
dotnet restore
dotnet run -p tools/MvcTemplate.Rename
if exist tools ( rmdir /s /q tools )
if exist CONTRIBUTING.md ( del CONTRIBUTING.md )
if exist LICENSE.txt ( del LICENSE.txt )
if exist README.md ( del README.md )
