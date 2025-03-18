# MyContact

## Description
MyContact est une application de bureau développée avec WPF (.NET 8.0) qui permet de gérer des contacts. Cette application offre une interface utilisateur moderne et intuitive pour ajouter, modifier, supprimer et rechercher des contacts.

## Prérequis
- .NET 8.0 SDK
- Visual Studio 2022 (ou version ultérieure)
- Windows 10 ou Windows 11

## Installation
1. Clonez ce dépôt sur votre machine locale
2. Ouvrez la solution `MyContact.sln` dans Visual Studio
3. Restaurez les packages NuGet si nécessaire
4. Compilez et exécutez l'application

## Fonctionnalités
- Interface utilisateur intuitive
- Gestion complète des contacts (ajout, modification, suppression)
- Recherche de contacts
- Stockage persistant des données

## Procédure d'installation de l'application avec l'exécutable
- Télécharger le dossier .zip en suivant ce lien  [release]( https://github.com/hanan3889/MyContact/releases/tag/release) .
- Décompresser le fichier et exécuter le fichier avec l'extension .exe
- Double-cliquez sur le fichier d'installation téléchargé pour lancer le programme d'installation.
- Autoriser l'installation.
- Suivez les instructions à l'écran pour compléter l'installation. 
- Une fois l'installation terminée, cliquez sur Terminer pour fermer le programme d'installation.

## Structure du projet
- `App.xaml` / `App.xaml.cs` : Point d'entrée de l'application
- `MainWindow.xaml` / `MainWindow.xaml.cs` : Fenêtre principale de l'application
- `Models/` : Classes de modèles pour les données de contacts
- `ViewModels/` : Implémentation du pattern MVVM
- `Views/` : Composants d'interface utilisateur supplémentaires

## Développement
Le projet utilise le framework WPF avec le pattern MVVM (Model-View-ViewModel) pour séparer la logique métier de l'interface utilisateur.

### Ajouter une nouvelle fonctionnalité
1. Créez ou modifiez les modèles nécessaires dans le dossier `Models/`
2. Implémentez la logique dans les ViewModels appropriés
3. Créez ou modifiez les vues XAML selon les besoins


