# Appel.SharpTemplate

## Overview
Appel.SharpTemplate is a .NET Core 8 template project designed for building clean architecture solutions. Inspired by [Ardalis's CleanArchitecture](https://github.com/ardalis/CleanArchitecture), this project serves as a robust boilerplate, ensuring maintainability, scalability, and efficiency.

## Key Features
- **.NET Core 8:** Utilizes the latest .NET Core 8 framework for enhanced performance and scalability.
- **Clean Architecture:** Adheres to clean architecture principles for modular and maintainable code.
- **Comprehensive Library Support:** Equipped with a suite of essential libraries for development efficiency.

## Included Libraries
```xml
<Project>
  <ItemGroup>
    <!-- Essential Libraries -->
    <PackageVersion Include="EFCore.NamingConventions" Version="7.0.2" />
    <PackageVersion Include="ErrorOr" Version="1.3.0" />
    <PackageVersion Include="FluentAssertions" Version="6.12.0" />
    <PackageVersion Include="FluentValidation" Version="11.8.1" />
    <PackageVersion Include="Isopoh.Cryptography.Argon2" Version="2.0.0" />
    <PackageVersion Include="MailKit" Version="4.3.0" />
    <PackageVersion Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
    <PackageVersion Include="Riok.Mapperly" Version="3.2.0" />
    <PackageVersion Include="Serilog.AspNetCore" Version="8.0.0" />
    <!-- ... and others ... -->
  </ItemGroup>
</Project>
```

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/guilhermeappel/appel-sharp-template.git
   ```

2. **Navigate to the project directory:**
   ```bash
   cd appel-sharp-template
   ```
   
3. **Restore dependencies::**
   ```bash
   dotnet restore
   ```

4. **Restore dependencies::**
   ```bash
   dotnet run
   ```

## Contributing
Contributions are welcome. Please fork the repository and submit pull requests with your changes.

## License
This project is licensed under the MIT License.

## Acknowledgments
Special thanks to [Ardalis](https://github.com/ardalis) for the CleanArchitecture inspiration.
