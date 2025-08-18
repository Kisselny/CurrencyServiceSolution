Start-Process powershell -ArgumentList 'dotnet run --project .\src\MigrationsService\MigrationsService.Api --launch-profile "https"'
Start-Process powershell -ArgumentList 'dotnet run --project .\src\CurrencyUpdaterBackgroundService\CurrencyUpdaterService.Worker'
Start-Process powershell -ArgumentList 'dotnet run --project .\src\CurrencyService\CurrencyService.Api --launch-profile "https"'
Start-Process powershell -ArgumentList 'dotnet run --project .\src\UserService\UserService.Api --launch-profile "https"'
Start-Process powershell -ArgumentList 'dotnet run --project .\src\GatewayService\GatewayService.Api --launch-profile "https"'
Start-Process "https://localhost:7239/swagger"
