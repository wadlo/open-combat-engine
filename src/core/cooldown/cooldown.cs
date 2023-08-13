using Godot;

/**
	Cooldown - A class that represents cooldown, such as for a weapon, ability, spawner, or really anything that can have cooldown.
*/
public partial class Cooldown: Resource {

	[Export]
	public float cooldown;
	[Export]
	public bool startWithCooldown = false;

	private float cooldownTimerReset;
	private float cooldownTimer = 0.0f;

	public Cooldown(int cooldown) {
		cooldownTimerReset = cooldown;
	}

	public bool CanUse() {
		return cooldownTimer <= 0.0f;
	}

	public bool TryActivate() {
		if (CanUse()) {
			ActivateAndReset();
			return true;
		}
		else {
			return false;
		}
	}

	private void ActivateAndReset() {
		cooldownTimer = cooldownTimerReset;
	}

	public void ForceActivate() {
		ActivateAndReset();
	}

	public void Update(float deltaTime) {
		cooldownTimer -= deltaTime;
	}
}
