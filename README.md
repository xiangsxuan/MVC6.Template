![License](https://img.shields.io/badge/license-MIT-green.svg?style=plastic)

# Installation
1. Set Web as default start up project.
2. Rename your project using "MvcTemplate.Rename.cmd".
2. Run "dotnet ef database update" command from src/{YourSolutionName}.Data folder.
3. Run "dotnet ef database update" command from test/{YourSolutionName}.Tests folder.

# Running tests
1. Make sure you started the web site at least once.
2. Run "dotnet test -parallel none" command from test/{YourSolutionName}.Tests folder.

# Features in progress
- Script and styling minification and bundling.
- Dynamic script and styling loading.
- Globalization.

# Features
- Model-View-ViewModel architectural design.
- Latest technologies and frameworks.
- Lowercase or normal ASP.NET urls.
- Protection from CSRF, XSS, etc.
- Custom membership providers.
- Easy project renaming.
- Globalization.
- Site map.
- Tests.

# Contribution
- Before you start writing a pull request you should discuss it using GitHub issues.
- Bugs, improvements or features should be reported using GitHub issues.
- Questions should be asked at [CodeProject](http://www.codeproject.com/Articles/820836/ASP-NET-MVC-Template-introduction).
