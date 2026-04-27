# To-Do List en .NET MAUI (Windows)

Application de niveau **débutant amélioré** pour apprendre les bases de .NET MAUI avec une architecture propre (MVVM simple).

## Fonctionnalités

- Ajout d'une tâche (titre + description).
- Marquage d'une tâche comme terminée.
- Suppression d'une tâche.
- Filtrage: **Toutes / À faire / Terminées**.
- Persistance locale dans un fichier JSON.

## Structure

- `Models/` : modèle métier `TodoTask`
- `Services/` : stockage local (`TaskRepository`)
- `ViewModels/` : logique de présentation (`MainViewModel`)
- `Views/` : interface XAML (`MainPage`)

## Lancer le projet (sur Windows)

1. Installer .NET 8 SDK.
2. Installer la workload MAUI:
   ```bash
   dotnet workload install maui
   ```
3. Restaurer et exécuter:
   ```bash
   dotnet --version
   dotnet restore
   dotnet build -f net8.0-windows10.0.19041.0
   dotnet run -f net8.0-windows10.0.19041.0
   ```


> Astuce : si `dotnet --version` affiche `10.x`, placez-vous à la racine du projet (fichier `global.json`) pour forcer l'usage du SDK .NET 8.
