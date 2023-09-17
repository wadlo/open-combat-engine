using System;
using System.Diagnostics;
using Godot;

/**
    Used for Guns, spells, weapons, abilities, or tools. Anything that has a concept of cooldown, ammo, and stamina.

    A Usable is assumed to always be in one of the following states:

    Idle - When a tool isn't doing anything.
    Reloading - A shotgun may have a high reload time, whereas swinging a sword wouldn't have a reload at all (would have a recoil)
    Charging - The time before you start firing. For a sword, this can be the time it takes to pull the sword up into position for a swing. Or it might be the time before unleashing a powerful attack, but after it's triggered.
    Firing - This is the state while something is actively firing. For most guns, this should be instant. However, for a sword swing or a laser, this represents the period of time while a hitbox is active.
    Recoil - The period of time after using the weapon before you can use again. This is the amount of time between bullets in a machine gun, or the amount of time between sword swings.
*/
public partial class Usable : Node
{
    enum FireState
    {
        Idle = 0,
        Reloading = 1,
        Charging = 2,
        Firing = 3,
        Recoil = 4
    }

    [Signal]
    public delegate void OnFireEventHandler();

    [Export]
    private UsableConfig config;

    [Export]
    public bool autofire;

    // Ammo variables
    [Export]
    public float ammo = 0.0f;

    // Private
    private FireState currentState = FireState.Idle;
    private float currentStateCooldown = 0.0f;

    public override void _Process(double delta)
    {
        /* The only state that can override another state is firing. If you're firing, you're able to override the
        reload state. So we do that here before decrementing delta. */
        if (currentState == FireState.Reloading && ShouldFire())
        {
            currentStateCooldown = 0.0f;
            Charge();
        }

        currentStateCooldown -= (float)delta;

        // Using a while loop here lets us use tools with very high fire rates, allowing multiple uses per frame.
        bool exit = false;
        int maxIterations = 1000;
        while (currentStateCooldown <= 0 && exit == false && maxIterations > 0)
        {
            maxIterations -= 1;

            switch (currentState)
            {
                case FireState.Reloading:
                    ProcessReloadState();
                    break;

                case FireState.Charging:
                    ProcessChargeState();
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

    private void ProcessChargeState()
    {
        if (currentStateCooldown <= 0)
        {
            Fire();
        }
    }

    private bool ProcessIdleState()
    {
        bool nothingElseToDo = false;
        if (ShouldFire())
        {
            Charge();
        }
        else if (ShouldReload())
        {
            StartReload();
        }
        else
        {
            // There's nothing else to do.
            currentStateCooldown = 0.0f;
            nothingElseToDo = true;
        }

        return nothingElseToDo;
    }

    private void Charge()
    {
        currentState = FireState.Charging;
        currentStateCooldown += config.chargeDuration;
    }

    private void StartReload()
    {
        currentState = FireState.Reloading;
        currentStateCooldown += config.reloadTimePerUnit;
    }

    private void Fire()
    {
        currentState = FireState.Firing;
        currentStateCooldown += config.fireDuration;
        ammo -= 1.0f;
        EmitSignal(SignalName.OnFire);
    }

    private void ProcessReloadState()
    {
        if (currentStateCooldown <= 0)
        {
            currentState = FireState.Idle;
            ammo += 1.0f;
            ammo = Mathf.Min(ammo + 1.0f, config.maxUses);
        }
    }

    private void ProcessFireState()
    {
        if (currentStateCooldown <= 0)
        {
            currentState = FireState.Recoil;
            currentStateCooldown += config.recoilTime;
        }
    }

    private void ProcessRecoilState()
    {
        if (currentStateCooldown < 0)
        {
            currentState = FireState.Idle;
        }
    }

    public bool ShouldFire()
    {
        return CanFire() && autofire;
    }

    public bool IsFiring()
    {
        return currentState == FireState.Firing;
    }

    public bool IsReloading()
    {
        return currentState == FireState.Reloading;
    }

    public bool IsCharging()
    {
        return currentState == FireState.Charging;
    }

    public bool CanFire()
    {
        return ammo >= 1.0f
            && (currentState == FireState.Idle || currentState == FireState.Reloading);
    }

    public bool ShouldReload()
    {
        return CanReload() && (config.autoReload || Input.IsKeyPressed(Key.Space));
    }

    public bool CanReload()
    {
        return ammo < config.maxUses && currentState == FireState.Idle;
    }

    // Returns a float between 0 and 1. 0 if the action is just staring, and 1 if it's finishing up.
    public float GetCurrentStatePercent()
    {
        float maxTime;
        switch (currentState)
        {
            case FireState.Charging:
                maxTime = config.chargeDuration;
                break;
            case FireState.Firing:
                maxTime = config.fireDuration;
                break;
            case FireState.Recoil:
                maxTime = config.recoilTime;
                break;
            case FireState.Reloading:
                maxTime = config.reloadTimePerUnit;
                break;
            default:
                return 0.0f;
        }

        return 1.0f - (currentStateCooldown / maxTime);
    }
}
