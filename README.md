# BudgetApp – Backend

Backendová časť osobnej finančnej aplikácie **BudgetApp**, vytvorená v C# a ASP.NET Core. Projekt poskytuje REST API pre Android klienta a zabezpečuje prácu s používateľmi, autentifikáciou a finančnými údajmi.

> Praktický osobný projekt zameraný na rozvoj znalostí v oblasti C#/.NET, backendového vývoja, databáz a REST API.

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

Backend aktuálne zahŕňa:

- registráciu a prihlasovanie používateľov
- JWT autentifikáciu
- prácu s používateľskými účtami
- komunikáciu s PostgreSQL databázou pomocou Entity Framework Core
- oddelenie databázových entít a DTO modelov
- validáciu vstupných údajov
- databázové migrácie
- REST API pre komunikáciu s Android aplikáciou

## 🏗️ Architektúra

Projekt je postavený ako **ASP.NET Core Web API**.

Pri návrhu sa zameriavam najmä na:

- oddelenie API vrstvy od dátového modelu
- používanie DTO namiesto priameho vystavovania databázových entít
- prácu s databázou prostredníctvom Entity Framework Core
- JWT autentifikáciu používateľov
- validáciu vstupných údajov
- konzistentnú prácu s dátami

## 📱 Android klient

Backend je prepojený s Android aplikáciou vytvorenou v **Kotline a Jetpack Compose**.

**Frontend:**  
https://github.com/martinamotlochova/BudgetApp-frontend

Android aplikácia komunikuje s backendom prostredníctvom REST API.

## 🎯 Cieľ projektu

BudgetApp je praktický projekt, na ktorom si prehlbujem znalosti **C#/.NET a backendového vývoja**.

Pri jeho vývoji sa venujem najmä:

- tvorbe REST API
- práci s relačnou databázou
- Entity Framework Core
- autentifikácii používateľov
- návrhu a oddeleniu dátových modelov
- komunikácii medzi backendom a klientskou aplikáciou

Projekt je priebežne vyvíjaný a jeho funkcionalita sa postupne rozširuje.
