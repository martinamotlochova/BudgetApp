# BudgetApp – Backend

Backendová časť osobnej finančnej aplikácie **BudgetApp**, vytvorená v C# a ASP.NET Core. Projekt poskytuje REST API pre Android klienta a zabezpečuje prácu s používateľmi, autentifikáciou a finančnými údajmi.

> Projekt je vytváraný ako praktický osobný projekt s cieľom prehĺbiť znalosti vývoja backendových aplikácií v C#/.NET, práce s databázami a tvorby REST API.

## 🛠️ Technológie

- **C#**
- **.NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **REST API**
- **JWT autentifikácia**
- **DTO**
- **Git / GitHub**

## 📌 Funkcionalita

Backend aktuálne zahŕňa najmä:

- registráciu používateľov
- prihlasovanie používateľov
- JWT autentifikáciu
- prácu s používateľskými účtami
- komunikáciu s databázou pomocou Entity Framework Core
- oddelenie databázových entít a DTO modelov
- validáciu vstupných údajov
- databázové migrácie
- REST API pre komunikáciu s klientskou aplikáciou

## 🏗️ Architektúra

Projekt je postavený ako **ASP.NET Core Web API** s oddelením jednotlivých častí aplikácie.

Pri návrhu sa zameriavam najmä na:

- oddelenie API vrstvy od dátového modelu
- používanie DTO namiesto priameho vystavovania databázových entít
- prácu s databázou prostredníctvom Entity Framework Core
- bezpečnú autentifikáciu používateľov pomocou JWT
- validáciu vstupov a konzistentnú prácu s dátami

## 📱 Klientská aplikácia

Backend je určený pre Android klienta vytvoreného v **Kotline a Jetpack Compose**.

Frontend repozitár:

**[BudgetApp – Android frontend](https://github.com/martinamotlochova/BudgetApp-frontend)**

Android aplikácia komunikuje s backendom prostredníctvom REST API.

## 🚀 Spustenie projektu

### Požiadavky

Pre spustenie projektu je potrebné mať nainštalované:

- .NET 10 SDK
- PostgreSQL
- Visual Studio alebo JetBrains Rider

### Konfigurácia

Pred spustením je potrebné nastaviť pripojenie k PostgreSQL databáze a ďalšie konfiguračné hodnoty podľa lokálneho prostredia.

Citlivé údaje, ako napríklad heslá alebo connection stringy, nie sú súčasťou repozitára.

### Spustenie

Po nastavení databázy je možné projekt spustiť priamo cez Visual Studio alebo pomocou .NET CLI.

```bash
dotnet restore
dotnet build
dotnet run
```

## 📚 Cieľ projektu

BudgetApp vzniká ako praktický projekt počas môjho učenia sa C#/.NET. Pri jeho vývoji si prehlbujem znalosti backendového vývoja, databáz, REST API, autentifikácie a komunikácie medzi backendom a klientskou aplikáciou.

Projekt je priebežne rozvíjaný a jeho funkcionalita sa postupne rozširuje.
