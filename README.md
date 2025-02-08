# API de Gestion des Notes :Module Inscriptions Acad�miques

## Description
L'API de gestion des notes et du module d'inscription acad�mique permet de g�rer les �tudiants, leurs inscriptions, le choix des unit�s d'enseignement (UE), des fili�res, et d'autres aspects li�s � l'organisation acad�mique. Cette API est document�e avec Swagger pour faciliter son utilisation.

## Fonctionnalit�s principales
- **Gestion des �tudiants** : Cr�ation, mise � jour, suppression et r�cup�ration des informations des �tudiants.
- **Gestion des inscriptions** : Inscription des �tudiants aux diff�rentes fili�res et unit�s d'enseignement.
- **Gestion des unit�s d'enseignement (UE)** : Consultation et attribution des UE aux �tudiants.
- **Gestion des fili�res** : Ajout, mise � jour et suppression des fili�res disponibles.
- **Gestion des notes** : Attribution, modification et r�cup�ration des notes des �tudiants.
- **Authentification et S�curit�** : Authentification par token et gestion des autorisations.
- **Documentation Swagger** : Interface interactive pour tester et comprendre les endpoints.

## Technologies utilis�es
- **Langage** : C#
- **Framework** : ASP.NET Core
- **Base de donn�es** : SQL Server
- **Authentification** : JWT (JSON Web Token)
- **Documentation** : Swagger

## Installation
### Pr�requis
- .NET SDK install� sur votre machine
- SQL Server pour la base de donn�es
- Un outil comme Postman ou Swagger UI pour tester l'API

### �tapes d'installation
1. **Cloner le projet**
   ```sh
   git clone https://github.com/votre-repo/api-gestion-notes.git
   cd api-gestion-notes
   ```
2. **Configurer la base de donn�es**
   - Modifier la cha�ne de connexion dans `appsettings.json`
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=GestionNotesDB;User Id=sa;Password=yourpassword;"
   }
   ```
   - Ex�cuter les migrations
   ```sh
   dotnet ef database update
   ```
3. **Lancer l'API**
   ```sh
   dotnet run
   ```

## Documentation Swagger
Une fois l'API lanc�e, la documentation Swagger est accessible � l'URL suivante :
```
http://localhost:5000/swagger/index.html
```

## Endpoints Principaux
### �tudiants
- `GET /api/etudiants` : Liste des �tudiants
- `POST /api/etudiants` : Ajouter un �tudiant
- `GET /api/etudiants/{id}` : Obtenir un �tudiant par ID
- `PUT /api/etudiants/{id}` : Mettre � jour un �tudiant
- `DELETE /api/etudiants/{id}` : Supprimer un �tudiant

### Inscriptions
- `POST /api/inscriptions` : Inscrire un �tudiant
- `GET /api/inscriptions/{id}` : Obtenir l'inscription d'un �tudiant

### Notes
- `POST /api/notes` : Ajouter une note
- `GET /api/notes/etudiant/{id}` : Obtenir les notes d'un �tudiant

## S�curit�
L'API utilise JWT pour s�curiser les endpoints sensibles. Pour acc�der aux ressources prot�g�es, il est n�cessaire d'envoyer un token JWT dans l'en-t�te `Authorization`.

## Auteurs
- **Votre Nom** - D�veloppeur principal

## Licence
Ce projet est sous licence MIT. Vous �tes libre de le modifier et de le distribuer en respectant les conditions de la licence.

