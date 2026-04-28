# Roguelike (Working Title)

A 3D roguelike dungeon crawler built in Unity, inspired by The Binding of Isaac. This project is in active development as part of my game development portfolio.

## 🎮 Play It

👉 [Play on Unity Play](https://play.unity.com/es/games/aef779e3-afab-4ea2-9b93-fa4c1943f6f0/roguelikev01)

## ✅ Current State

- Two handcrafted rooms
- Three basic enemies with contact damage
- One agile boss enemy
- Working player movement and shooting system

## 🔧 Technical Highlights

- **Boss AI — 5-State Finite State Machine**: Idle → Telegraphing → Charging → Recovering → Shooting. Clean separation of behaviour per state, easy to extend with new boss types.
- **ScriptableObject Architecture**: Runtime state data (health, stats) decoupled from behaviour logic via ScriptableObjects, keeping components focused and reusable.
- **Damage System with Invincibility Frames**: `TakeDamage()` system with iFrame logic to prevent damage stacking on collision.
- **Contact Damage via OnCollisionStay**: Continuous damage applied correctly on sustained contact.
- **Enemy Shooting**: Enemies detect and fire at the player using directional logic.
- **GameManager Singleton with DontDestroyOnLoad**: Persistent game state management across scenes.
- **Input System Refactor**: Continuous and diagonal shooting implemented via Unity's Input Action system.

## 🗺️ Roadmap

- [ ] Procedural room generation
- [ ] More enemy types
- [ ] Item and power-up system
- [ ] Floor progression with difficulty scaling
- [ ] UI (health bar, minimap)
- [ ] Sound and visual feedback polish

## 🛠️ Built With

- Unity (URP)
- C#
- Unity Input System
- ScriptableObjects