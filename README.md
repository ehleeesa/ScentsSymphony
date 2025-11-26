# Scents Symphony

This is an ASP.NET Core MVC project connected to a local SQL Server database and running through IIS Express in Visual Studio 2022.

<p align="center"> <img width="674" height="396" src="https://github.com/user-attachments/assets/89ef02d2-ecaf-48f2-82d8-5a7a9b2a88e2" alt="Scents Symphony Screenshot"> </p>


## Repository GitHub

[https://https://github.com/ehleeesa/ScentsSymphony/]

---

## Installation steps:

### 1. Clone the repository:

bash
git clone https://https://github.com/ehleeesa/ScentsSymphony/
cd repo

## Requirements

### 2. Make sure you have the following installed:


- *Visual Studio 2022 with the ASP.NET and Web Development workload*
- *SQL Server Management Studio 19 (SSMS)*
- *SQL Server LocalDB* or *SQL Server Express*
- *.NET 6.0 SDK (for ASP.NET Core MVC)*

---

## Database configuration: 

1. Open *SQL Server Management Studio (SSMS)*
2. Connect using:
   - *Server type*: Database Engine  
   - *Server name*: . (localhost)
   - *Authentication*: Windows Authentication
3. Click *Connect*
4. Check whether the required database exists::
   - If *yes*, continue
   - If *no*, create it manually or run the SQL scripts included in the project (if any)
5. Verify the connection string in appsettings.json:

<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Data Source=.;Initial Catalog=ScentsSymphony;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>


---

## Build instructions: 

1. Open the solution file (.sln) in *Visual Studio 2022*
2. Select *Configuration:* Debug and *Platform:* Any CPU
3. Go to:
   - *Build* → *Build Solution*
   - Or press Ctrl + Shift + B

---

## Run instructions: 

1. In Visual Studio, choose IIS Express as the startup profile
2. Press *Start (F5)*
3. The browser will automatically open at:


http://localhost:xxxx/


xxxx este portul alocat de IIS Express pentru proiect, poate varia în funcție de configurație

---
