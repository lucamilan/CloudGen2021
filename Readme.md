# "Information Flow Processing" con DAPR in salsa CQRS

## Create the SQL Server Image with the database

```powershell
powershell ./Dapr.Cqrs.Infra/mssql/build.ps1
```

## Install DAPR Cli

```powershell
Set-ExecutionPolicy RemoteSigned -scope CurrentUser
powershell -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"
```

### Run the init CLI command

```powershell
dapr init
```

### Verify components directory has been initialized

```powershell
explorer "%USERPROFILE%\.dapr\"
```

## Install Tye

```powershell
dotnet tool install -g Microsoft.Tye --version "0.9.0-alpha.21380.1"
```

## Run Tye

Reference: https://github.com/dotnet/tye/blob/main/docs/reference/commandline/tye-run.md

```powershell
tye run --watch
```


