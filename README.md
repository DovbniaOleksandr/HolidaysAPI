# HolidaysAPI

HolidaysAPI is a .NET 8 Web API that provides public holiday information, including:
- List of countries
- Grouped holidays by month for a given country and year
- Status of a specific day (workday, free day, holiday)
- Maximum number of consecutive free days in a given country and year

## Endpoints Overview

### Countries
- **GET** `/api/countries`  
  Returns the list of supported countries.

### Holidays
- **GET** `/api/holidays/{countryCode}/{year}`  
  Returns holidays grouped by month for the specified country and year.  
  - **Parameters**:
    - `countryCode`: The ISO code of the country (e.g., `us`, `ukr`)
    - `year`: The year for which to retrieve holidays (e.g., `2025`)

### Day Status
- **GET** `/api/daystatus/{countryCode}/{date}`  
  Returns the status of a specific day (workday, free day, or holiday).  
  - **Parameters**:
    - `countryCode`: The ISO code of the country
    - `date`: The date in `YYYY-MM-DD` format (e.g., `2025-01-01`)

### Maximum Consecutive Free Days
- **GET** `/api/maxfreedays/{countryCode}/{year}`  
  Returns the maximum number of consecutive free days in the specified country and year.  
  - **Parameters**:
    - `countryCode`: The ISO code of the country
    - `year`: The year for which to calculate the maximum free days

## Deployment on Azure

### Step 1: Deploy MSSQL Database on Azure

1. **Login to Azure Portal**:  
   Go to [https://portal.azure.com](https://portal.azure.com) and sign in.

2. **Create a Resource Group**:  
   - Search for **Resource groups** in the search bar and click on it.
   - Click **+ Create**.
   - Enter the following details:
     - **Subscription**: Select your subscription.
     - **Resource Group Name**: `HolidaysAPI-RG`
     - **Region**: Choose a region (e.g., East US).
   - Click **Review + Create** and then **Create**.

3. **Create an Azure SQL Server**:
   - Search for **SQL servers** in the search bar and select it.
   - Click **+ Create**.
   - Enter the following details:
     - **Subscription**: Select your subscription.
     - **Resource Group**: `HolidaysAPI-RG`
     - **Server name**: `holidaysapi-sqlserver` (must be globally unique)
     - **Location**: Choose the same region as the resource group.
     - **Authentication method**: Use **SQL authentication**.
       - **Server admin login**: `sqladmin`
       - **Password**: `YourStrongPassword123`
   - Click **Review + Create** and then **Create**.

4. **Create SQL Database**:
   - Go to the **SQL server** you just created.
   - Under **Settings**, select **Databases** and click **+ New database**.
   - Enter the following details:
     - **Database name**: `Holidays`
     - **Compute + storage**: Choose **Standard (S0)** or as needed.
   - Click **Review + Create** and then **Create**.

5. **Configure Firewall Rules**:
   - In the **SQL server** settings, select **Networking**.
   - Under **Firewall rules**, click **+ Add your client IP** to allow your IP address.
   - Set **Allow Azure services and resources to access this server** to **Yes**.
   - Click **Save**.

6. **Get Connection String**:
   - Navigate to your **SQL Database** (`Holidays`).
   - Under **Settings**, select **Connection strings**.
   - Copy the **ADO.NET** connection string.

---

### Step 2: Deploy HolidaysAPI Web API from GitHub

1. **Create an App Service**:
   - Search for **App Services** and click **+ Create**.
   - Enter the following details:
     - **Subscription**: Select your subscription.
     - **Resource Group**: `HolidaysAPI-RG`
     - **Name**: `holidaysapi-webapp`
     - **Publish**: Code
     - **Runtime stack**: `.NET 8`
     - **Region**: Same region as your database.
   - Click **Review + Create** and then **Create**.

2. **Configure Deployment from GitHub**:
   - Go to the newly created **App Service**.
   - In the left menu, select **Deployment Center**.
   - Choose **GitHub** as the source.
     - **Authorize** if prompted.
     - **Organization**: Select your GitHub account.
     - **Repository**: `HolidaysAPI`
     - **Branch**: `main`
     - **Build provider**: Select **GitHub Actions**.
   - Click **Save**.
   - Go to GitHub and open generated **Workflow** file.
   - Modify **Build with dotnet** and **dotnet publish** steps to target **HolidaysAPI/HolidaysAPI.csproj** project.
3. **Configure Environment Variables**:
   - In the **App Service**, navigate to **Configuration**.
   - Under **Application settings**, add connection string to database.
   - Click **Save** and confirm the restart of the App Service.
