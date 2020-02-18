## Requirements
#### Windows
.NET Core Runtime [2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2/runtime) or [3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1/runtime)

#### Linux
Instructions on the [MS Docs](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1904) website.

#### General
MySQL-5.7 or above

## Setup
1. Install .NET Core runtime 2.2 or 3.1 for your respective OS
2. Install and run MySQL 5.7 locally or get hosting online
3. Update `appsettings.json` to hold your connection string in `MySQLConnection` field
4. Open the terminal in the innermost TickifyLocal folder and type the command `dotnet ef database update`
5. Run the program
