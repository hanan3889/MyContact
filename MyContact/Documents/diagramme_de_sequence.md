```mermaid
---

title: Diagramme de séquence

---
sequenceDiagram
    participant Salarié
    participant Application WPF
    participant API
    participant BDD
    Salarié->>Application WPF: Recherche la liste <br/>des salariés sur Paris
    Application WPF-->>API: /api/Sites/get/name/Paris
    API->>BDD: /get/name/PARIS
    BDD-)API: Retourne les résultats
    API -)Application WPF: Respone 200 OK <br/>avec la liste des salariés
    Application WPF -)Salarié: Affiche la liste des salariés
