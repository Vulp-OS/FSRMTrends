# FSRM Trends

This project connects with an existing SQL database and FSRM (File Server Resource Manager) server in order to save trend information for FSRM quotas.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* Visual Studio 2017
* .NET Framework 4.5.2
* SQL Server Instance

### Installing

#### Building the Program
1. Clone the repository ```git clone https://github.com/drockwood94/FSRMTrends.git```
2. Launch the project ```FSRMTrends.sln``` in Visual Studio 2017
3. Build the project with ```Ctrl+Shift+B```

#### Setting up the SQL Database
1. Create a new database with whatever name you intend to use ```eg: FSRMTrendData```

2. Grant the user you intend to use access to the database

3. Populate the Database
   * **EITHER** Use Visual Studio to publish the included SQL Database Project
   * **OR** Create the following tables:
      * Dates
         * Date (PK, datetime, notnull)
      * QuotaPaths
         * QuotaPath (PK, nvarchar(255), not null)
      * Sizes
        * QuotaPaths (PK, FK, nvarchar(255), not null)
        * Date (PK, FK, datetime, not null)
        * Size (Numeric(38,0), null)
      * Usages
        * QuotaPath (PK, FK, nvarchar(255), not null)
        * Date (PK, FK, datetime, not null)
        * UsageBytes (numeric(38,0, null)
4. Determine the proper SQL connection string for your use case (including authentication)

## Usage

### Initial Configuration
Launch the app without any command line switches to bring up the GUI. App settings are user-level and currently need to be set in ```Settings->Options...```

Ensure that the SQL Server is set up beforehand (refer to the Getting Started)

### Command Line Switches
* Update Quotas
```FSRMTrends.exe /updateQuotas```