# 🚗 M-Motors — Backend API

Backend ASP.NET Core du projet **M-Motors**, plateforme de vente et location de véhicules d’occasion avec option d’achat.

---

# 📌 Présentation du projet

M-Motors est une application Full Stack permettant :

- la consultation des véhicules,
- la gestion des dossiers d’achat et de location,
- l’authentification JWT,
- la gestion des documents,
- le suivi des dossiers,
- et l’administration du back-office.

Ce dépôt contient uniquement le **backend API ASP.NET Core**.

---

# 🛠️ Technologies utilisées

- ASP.NET Core (.NET 8)
- Entity Framework Core
- MariaDB / MySQL
- JWT Authentication
- Swagger
- xUnit
- Git / GitHub

---

# 📂 Architecture du projet

```bash
m_motors_API/
│
├── Controllers/
├── Models/
├── DTO/
├── Services/
├── Data/
├── Middleware/
├── Migrations/
├── uploads/
└── Program.cs
```

---

# ⚙️ Prérequis

Avant l’installation, vérifier les éléments suivants :

- .NET SDK 8.0
- MariaDB ou MySQL
- Git
- Visual Studio 2022 ou VS Code

---

# 🚀 Installation du projet

## 1️⃣ Cloner le dépôt

```bash
git clone https://github.com/pascal2590/M-Motors_Projet_STUDI_Back-end.git
```

---

## 2️⃣ Accéder au projet

```bash
cd M-Motors_Projet_STUDI_Back-end
```

---

## 3️⃣ Restaurer les dépendances

```bash
dotnet restore
```

---

# 🗄️ Configuration de la base de données

## Modifier le fichier :

```bash
appsettings.json
```

## Exemple :

```json
"ConnectionStrings": {
  "MMotorsConnection": "server=localhost;port=3306;database=m_motors;user=admin;password=VOTRE_PASSWORD"
}
```

---

# 🧱 Migration de la base de données

## Appliquer les migrations EF Core

```bash
dotnet ef database update
```

---

# ▶️ Lancer le projet

```bash
dotnet run
```

---

# 🌐 Accès API

## Swagger

```bash
https://localhost:7183/swagger
```

---

# 🔐 Authentification JWT

L’API utilise une authentification JWT sécurisée.

Certaines routes nécessitent :

- un token JWT valide,
- un rôle utilisateur autorisé.

---

# 📁 Gestion des fichiers

Les documents déposés par les clients sont stockés dans :

```bash
uploads/documents/
```

---

# 🧪 Tests unitaires

## Lancer les tests

```bash
dotnet test
```

---

# 📋 Fonctionnalités principales

## Front-office

- Consultation des véhicules
- Recherche et filtres
- Dépôt de dossier
- Upload de documents

## Back-office

- Gestion des dossiers
- Gestion des statuts
- Notes internes
- Logs système
- Gestion des utilisateurs

---

# 🔒 Sécurité

- Authentification JWT
- Gestion des rôles
- Validation serveur
- Middleware de gestion des erreurs
- Journalisation des logs

---

# 📈 Qualité logicielle

- Architecture en couches
- DTO
- Services métier
- Middleware personnalisé
- Tests unitaires
- Logging applicatif

---

# 👨‍💻 Auteur

Pascal MOREL  
Bachelor Développeur d’application C# .NET — STUDI

---

# 📄 Licence

Projet pédagogique réalisé dans le cadre du Bachelor Développeur d’application C# .NET.