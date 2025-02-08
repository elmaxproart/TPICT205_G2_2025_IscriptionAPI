# API de Gestion des Notes :Module Inscriptions Académiques

## Description
L'API de gestion des notes et du module d'inscription académique permet de gérer les étudiants, leurs inscriptions, le choix des unités d'enseignement (UE), des filières, et d'autres aspects liés à l'organisation académique. Cette API est documentée avec Swagger pour faciliter son utilisation.

## Fonctionnalités principales
- **Gestion des étudiants** : Création, mise à jour, suppression et récupération des informations des étudiants.
- **Gestion des inscriptions** : Inscription des étudiants aux différentes filières et unités d'enseignement.
- **Gestion des unités d'enseignement (UE)** : Consultation et attribution des UE aux étudiants.
- **Gestion des filières** : Ajout, mise à jour et suppression des filières disponibles.
- **Gestion des notes** : Attribution, modification et récupération des notes des étudiants.
- **Authentification et Sécurité** : Authentification par token et gestion des autorisations.
- **Documentation Swagger** : Interface interactive pour tester et comprendre les endpoints.

## Technologies utilisées
- **Langage** : C#
- **Framework** : ASP.NET Core
- **Base de données** : SQL Server
- **Authentification** : JWT (JSON Web Token)
- **Documentation** : Swagger

## Installation
### Prérequis
- .NET SDK installé sur votre machine
- SQL Server pour la base de données
- Un outil comme Postman ou Swagger UI pour tester l'API

### Étapes d'installation
1. **Cloner le projet**
   ```sh
   git clone https://github.com/votre-repo/api-gestion-notes.git
   cd api-gestion-notes
   ```
2. **Configurer la base de données**
   - Modifier la chaîne de connexion dans `appsettings.json`
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=GestionNotesDB;User Id=sa;Password=yourpassword;"
   }
   ```
   - Exécuter les migrations
   ```sh
   dotnet ef database update
   ```
3. **Lancer l'API**
   ```sh
   dotnet run
   ```

## Documentation Swagger
Une fois l'API lancée, la documentation Swagger est accessible à l'URL suivante :
```
http://localhost:5000/swagger/index.html
```

## Endpoints Principaux
### Étudiants
- `GET /api/etudiants` : Liste des étudiants
- `POST /api/etudiants` : Ajouter un étudiant
- `GET /api/etudiants/{id}` : Obtenir un étudiant par ID
- `PUT /api/etudiants/{id}` : Mettre à jour un étudiant
- `DELETE /api/etudiants/{id}` : Supprimer un étudiant

### Inscriptions
- `POST /api/inscriptions` : Inscrire un étudiant
- `GET /api/inscriptions/{id}` : Obtenir l'inscription d'un étudiant

### Notes
- `POST /api/notes` : Ajouter une note
- `GET /api/notes/etudiant/{id}` : Obtenir les notes d'un étudiant

## Sécurité
L'API utilise JWT pour sécuriser les endpoints sensibles. Pour accéder aux ressources protégées, il est nécessaire d'envoyer un token JWT dans l'en-tête `Authorization`.

## Auteurs
- **Votre Nom** - Développeur principal

## Licence
Ce projet est sous licence MIT. Vous êtes libre de le modifier et de le distribuer en respectant les conditions de la licence.

