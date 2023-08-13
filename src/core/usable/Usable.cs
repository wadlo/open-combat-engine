using System;
using System.Diagnostics;
using Godot;

/**
	Used for Guns, spells, weapons, abilities, or tools. Anything that has a concept of cooldown, ammo, and stamina.

	A Usable is assumed to always be in one of the following states:

	Idle - When a tool isn't doing anything.
	Reloading - A shotgun may have a high reload time, whereas swinging a sword wouldn't have a reload at all (would have a recoil)
	Firing - This is the state while something is actively firing. For most guns, this should be instant. However, for a sword swing or a laser, this represents the period of time while a hitbox is active.
	Recoil - The period of time after using the weapon before you can use again. This is the amount of time between bullets in a machine gun, or the amount of time between sword swings.
*/
public partial class Usable: Node {
	enum FireState {
		Idle = 0,
		Reloading = 1,
		Firing = 2,
		Recoil = 3
	}


	// Ammo variables
  	[Export]
	public float ammo = 0.0f;

	[Export]
	public float maxUses = 10.0f;


	// State times
	[Export]
	public float recoilTime = 0.1f;

	[Export]
	public float reloadTimePerUnit = 0.1f;

	[Export]
	public float fireDuration = 0.0f;

	// Private
	private FireState currentState = FireState.Idle;
	private float currentStateCooldown = 0.0f;
	
	public override void _Process(double delta)
	{   

		/* The only state that can override another state is firing. If you're firing, you're able to override the 
		reload state. So we do that here before decrementing delta. */
		if (currentState == FireState.Reloading && ShouldFire()) {
			currentStateCooldown = 0.0f;
			Fire();
		}

		currentStateCooldown -= (float)delta;

		// Using a while loop here lets us use tools with very high fire rates, allowing multiple uses per frame.
		bool exit = false;
		int maxIterations = 1000;
		while (currentStateCooldown <= 0 && exit == false && maxIterations > 0) {
			maxIterations -= 1;

			switch (currentState) {
				case FireState.Reloading:
					ProcessReloadState();
					break;

				case FireState.Firing:
					ProcessFireState();
					break;

				case FireState.Recoil:
					ProcessRecoilState();
					break;

				case FireState.Idle:
					exit = ProcessIdleState();
					break;
			}

		}
	}

	private bool ProcessIdleState() {
		bool nothingElseToDo = false;
		if (ShouldFire()) {
			Fire();
		}
		else if (ShouldReload()) {
			StartReload();
		}
		else {
			// There's nothing else to do.
			currentStateCooldown = 0.0f;
			nothingElseToDo = true;
		}

		return nothingElseToDo;
	}

	private void StartReload() {
		currentState = FireState.Reloading;
		currentStateCooldown += reloadTimePerUnit;
	}

	private void Fire() {
		currentState = FireState.Firing;
		currentStateCooldown += fireDuration;
		ammo -= 1.0f;
		// Todo -- Fire a "on start fire" event.
	}

	private void ProcessReloadState() {
		if (currentStateCooldown <= 0) {
			currentState = FireState.Idle;
			ammo = Mathf.Min(ammo + 1.0f, maxUses);
		}
	}

	private void ProcessFireState() {
		if (currentStateCooldown <= 0) {
			currentState = FireState.Recoil;
			currentStateCooldown += recoilTime;
		}
	}

	private void ProcessRecoilState() {
		if (currentStateCooldown < 0) {
			currentState = FireState.Idle;
		}
	}

	public bool ShouldFire() {
		return CanFire() && Input.IsMouseButtonPressed(MouseButton.Left);
	}

	public bool CanFire() {
		return ammo > 1.0f && (currentState == FireState.Idle || currentState == FireState.Reloading);
	}

	public bool ShouldReload() {
		return CanReload() && Input.IsKeyPressed(Key.Space);
	}

	public bool CanReload() {
		return ammo < maxUses && currentState == FireState.Idle;
	}
}
