# Roguelike (Working Title)

A 3D roguelike dungeon crawler built in Unity, inspired by The Binding of Isaac. This project is in active development as part of my game development portfolio.

## 🎮 Play It

👉 [Play on Unity Play](https://play.unity.com/es/games/aef779e3-afab-4ea2-9b93-fa4c1943f6f0/roguelikev01)

##✅ Current State (V0.1)
- Two handcrafted rooms with door trigger navigation
- Three enemy types with contact and projectile damage
- Agile boss with 5-state FSM
- Full player movement and twin-stick shooting

##🔧 Technical Highlights
- Boss AI — 5-State FSM (Idle → Telegraphing → Charging → Recovering → Shooting). Behaviour isolated per state; extensible to new boss types.
- ScriptableObject Data Layer: Base stats (EnemyData, PlayerData) defined as SOs. Runtime state (currentHealth, active stats) lives in MonoBehaviours. Supports per-run stat mutation without touching base values.
- Damage System with i-Frames: TakeDamage() guards against damage stacking via timed invincibility coroutine.
- Event-Driven Architecture: Enemy death notifies RoomManager via static C# event — no direct reference coupling.
- GameManager Singleton + DontDestroyOnLoad: Cross-scene run state management with explicit StartNewRun() reset.
- New Input System: Directional shooting via 2D Vector composite action; movement and shooting on separate bindings.
- Physics.IgnoreCollision: Bullet-to-owner collision suppressed at spawn time.

##🗺️ Roadmap (V0.2 — In Progress)
- Procedural floor generation (9×9 grid, random walk)
- Dynamic door activation based on room adjacency
- Enemy variants: MeleeFast, Tank, Fearful
- SpawnerBoss (second boss type)
- Item and power-up system (ItemSO + weighted drops)
- Dash ability with i-frames and cooldown UI
- Minimap generated at runtime
- Object pooling for bullets
- Screen shake, hit flash, basic SFX

##🛠️ Built With
- Unity 6 (URP)
- C#
- Unity Input System (New)
- ScriptableObjects
