# open-combat-engine
Combat ECS components for Godot 4.x, with examples. Everything is completely open source.

# Setup
1. Use the [Godot Engine .NET / mono build](https://godotengine.org/download). **You can still use GDScript with Mono build.**
2. Download and run the [.net 7.0 SDK](https://dotnet.microsoft.com/en-us/download).
3. Open the Godot project located at `src/project.godot`
4. Build your C# scripts by pressing `Alt` + `B`, or click the square _Build_ button in the top-right corner.
5. Run the project by pressing `f5` or pressing the play button near the top right of the Godot window.

If you have problems setting up or run into bugs, please [create an issue](https://github.com/wadlo/open-combat-engine/issues/new) or reach out on [our discord](https://discord.gg/h3d8bTbcE2).

# Components
- Health
- Knockback
- AbilityTimer (Supports cooldown, charging up an ability etc)
- Damage
- Projectiles
- Target (Used for tracking what groups to target)

# Other Features
- Unit AI movement (Port of GSAI to C# and Godot 4)

# Example
- Clash -- An example with swordsmen and archers that can be spawned by both sides.
<img src="gifs/Clash.gif" width="500">

# Contribute
For feature requests, or to contribute, [join the discord](https://discord.gg/h3d8bTbcE2).
For bugs, please [create an issue](https://github.com/wadlo/open-combat-engine/issues/new).
