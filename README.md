# Scents Symphony

Acesta este un proiect ASP.NET Core MVC care se conectează la o bază de date locală SQL Server și rulează prin IIS Express în Visual Studio 2022.

## Repository GitHub

[https://https://github.com/ehleeesa/ScentsSymphony/]

---

## Pași de instalare

### 1. Clonează repository-ul

bash
git clone https://https://github.com/ehleeesa/ScentsSymphony/
cd repo


### 2. Asigură-te că ai următoarele instalate:

- *Visual Studio 2022* cu workload-ul „ASP.NET and web development”
- *SQL Server Management Studio 19 (SSMS)*
- *SQL Server LocalDB sau Express Edition*
- *.NET Framework* (.NET Framework 6.0)

---

## Configurarea bazei de date

1. Deschide *SQL Server Management Studio 19*
2. Conectează-te astfel:
   - *Server type*: Database Engine  
   - *Server name*: . (localhost)
   - *Authentication*: Windows Authentication
3. Apasă *Connect*
4. Verifică dacă baza de date folosită de aplicație există deja:
   - Dacă *da*, poți trece mai departe
   - Dacă *nu*, creează baza de date manual sau rulează scripturi .sql din proiect (dacă există)
5. Verifică string-ul de conexiune din fișierul appsettings.json:

<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Data Source=.;Initial Catalog=ScentsSymphony;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>


---

## Pași de compilare

1. Deschide proiectul în *Visual Studio 2022* (fișier .sln)
2. Selectează configurația Debug și platforma Any CPU
3. Mergi la:
   - *Build* → *Build Solution*
   - Sau apasă Ctrl + Shift + B

---

## Pași de rulare

1. În Visual Studio, selectează *IIS Express* ca profil de rulare
2. Apasă pe *Start* (F5)
3. Se va deschide browserul pe:


http://localhost:xxxx/


xxxx este portul alocat de IIS Express pentru proiect, poate varia în funcție de configurație

---
