# hangnow-back

# Install ⚙

### Back-end
Lancer le projet :
```bash
dotnet run
```

Set up la db :
```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update
```

Créer une migration pour la db :
```bash
dotnet ef migrations add SomeTableChange
```
### Front-end
[Repository front-end](https://github.com/HangNowApp/hangnow-front) 

# Choix techniques 🔧

### Back-end :
Choix de C# car la théorie sur la sécurité à été donnée avec ce langage mais aussi car la techno est utilisée depuis la 2ème année donc bien connu de tous les membres de l'équipe

### Font-end :
[Repository front-end](https://github.com/HangNowApp/hangnow-front) 

# Répartition 👥
Les tâches ont été répartie de sorte à ce que l'avancement du projet se fasse aussi bien côté back que front.


### Melvyn
- Back -> Grande majorité (sécurité, models, controllers, dto)
- Front -> Quelques interventions (set up, composants, intégrations)

### Alex
- Back -> Quelques interventions (controllers)
- Front -> Interventions (composants, intégrations, pages)
### Ana
- Back -> Quelques interventions (model, légers fix)
- Front -> Principalement sur le front (composants, intégrations, pages)


# Project 👋
Application de rencontre instantanée dans laquelle un user peut créer un ou plusieurs évenèments qu'il va host et auxquels d'autres user pourront se joindre, ces évènements vont contenir des tags permettant d'identifier leur type

Un utilisateur de base est limité à 2 évènements à la fois et il peut upgrade pour être premium et pouvoir créer plus de 2 events

Les fonctions de base de l'application sont : création de compte, connexion, création/suppression d'évènements, modification du profil utilisateur, passage à un compte premium, création de tags, voir le profil d'un autre user.
