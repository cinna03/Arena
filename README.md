# Arena Survivor
A 2D top-down shooter built in Unity demonstrating core Object-Oriented 
Programming principles, design patterns, and algorithms.

## Game Overview
Arena Survivor is a 2D top-down shooter where the player must survive 
endless waves of enemies. The player moves around the arena, shoots 
enemies, and tries to achieve the highest score possible before dying.

### How to Play
- **WASD** — Move the player
- **Mouse** — Aim
- **Left Click** — Shoot
- **Survive** as many waves as possible
- **Score points** by killing enemies
- **Game Over** when Lives reach 0

---

## Setup Instructions

### Requirements
- Unity 2022.3 or later
- 2D Core template

### How to Run
1. Clone the repository:
   git clone https://github.com/YOURUSERNAME/ArenaSurvivor.git
2. Open Unity Hub
3. Click **Add Project**
4. Navigate to the cloned folder and select it
5. Open the project in Unity
6. Open the scene: **Assets/Scenes/SampleScene**
7. Press **Play** to run the game

---

## Project Structure
```
Assets/
  Scripts/
    GameManager.cs       — Singleton pattern, tracks score/kills/state
    Player.cs            — Player movement, shooting, health
    Enemy.cs             — Base enemy class, State pattern
    FastEnemy.cs         — Inherits Enemy, higher speed
    TankEnemy.cs         — Inherits Enemy, damage resistance
    Bullet.cs            — Bullet movement and collision
    EnemySpawner.cs      — Wave spawning algorithm
    UIManager.cs         — Observer pattern, updates all UI
    IDamageable.cs       — Interface for damageable objects
  Prefabs/
    Player.prefab
    BasicEnemy.prefab
    FastEnemy.prefab
    TankEnemy.prefab
    Bullet.prefab
  Scenes/
    SampleScene.unity
```

---

## OOP Principles

### Encapsulation
Player health is stored as a private field `_currentHealth` and can 
only be modified through the `TakeDamage()` method. This prevents 
any external script from directly manipulating health values.
```csharp
// Player.cs
private int _currentHealth;
public void TakeDamage(int amount)
{
    if (!IsAlive()) return;
    _currentHealth -= amount;
    _currentHealth = Mathf.Max(_currentHealth, 0);
}
```

### Abstraction
The `IDamageable` interface abstracts the concept of taking damage. 
Any script that deals damage only needs to find an IDamageable 
component — it does not need to know if it is hitting a Player or Enemy.
```csharp
// IDamageable.cs
public interface IDamageable
{
    void TakeDamage(int amount);
    bool IsAlive();
}
```

### Inheritance
FastEnemy and TankEnemy both inherit from the Enemy base class, 
sharing all core behavior while overriding only what makes them unique.
```csharp
// FastEnemy.cs
public class FastEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 4f;
        health = 1;
    }
}
```

### Polymorphism
TakeDamage() behaves differently depending on the object type. 
TankEnemy overrides it to reduce incoming damage, while the base 
Enemy takes full damage. The calling code is identical in both cases.
```csharp
// TankEnemy.cs
public override void TakeDamage(int amount)
{
    int reducedDamage = Mathf.Max(1, amount - 1);
    base.TakeDamage(reducedDamage);
}
```

---

## Design Patterns

### Singleton — GameManager.cs
Ensures only one GameManager exists. All scripts access it through 
`GameManager.Instance` without needing direct scene references.
```csharp
public static GameManager Instance { get; private set; }
private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
}
```

### Observer — GameManager.cs + UIManager.cs
GameManager fires events when score, kills, or game state changes. 
UIManager subscribes to these events and updates the display automatically.
```csharp
// Subject - GameManager.cs
public event System.Action<int> OnScoreChanged;
public event System.Action<int> OnKillsChanged;
public event System.Action OnGameOver;

// Observer - UIManager.cs
GameManager.Instance.OnScoreChanged += UpdateScore;
GameManager.Instance.OnKillsChanged += UpdateKills;
GameManager.Instance.OnGameOver     += ShowGameOver;
```

### State Pattern — Enemy.cs
Each enemy has three states: Idle, Chase, and Attack. Behavior 
changes automatically based on the current state, keeping the 
Update method clean and organized.
```csharp
protected enum EnemyState { Idle, Chase, Attack }
protected virtual void Update()
{
    switch (currentState)
    {
        case EnemyState.Chase:  ChasePlayer();  break;
        case EnemyState.Attack: AttackPlayer(); break;
    }
}
```

---

## Algorithms

### Algorithm 1 — Wave Spawning (EnemySpawner.cs)
Spawns progressively harder waves of enemies. Enemy count and 
type scale with the wave number, creating an escalating difficulty curve.
- **Time Complexity: O(n)** where n = total enemies in the wave
- **Space Complexity: O(n)** — each enemy creates a new GameObject

### Algorithm 2 — Enemy Search and Chase (Enemy.cs)
On spawn each enemy searches the scene for the player using 
FindGameObjectWithTag() then chases them using Vector2.MoveTowards() 
every frame, integrated with the State pattern for attack behavior.
- **Search Complexity: O(n)** — runs once on spawn
- **Chase Complexity: O(1)** per enemy per frame

---

## Assets
All game assets (shapes and colors) are built using Unity's default 
2D primitive sprites. No external assets were used.

## Author
[YOUR NAME]
