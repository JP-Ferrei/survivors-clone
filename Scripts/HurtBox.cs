using Godot;
using SurvivorsClone.Scripts;

public partial class HurtBox : Area2D
{
    public enum HurtBoxEnum
    {
        Cooldown,
        HitOnce,
        DisableHitBox
    }

    [Signal]
    public delegate void OnHurtBoxCollisionEventHandler(int damage);

    [Signal]
    public delegate void OnHurtBoxKnockBackEventHandler(Vector2 angle, int knockBackAmout);

    [Export]
    public HurtBoxEnum HurtBoxType { get; set; }

    [Export]
    public HealthComponent? HealthComponent { get; set; }

    public CollisionShape2D? CollisionShape { get; set; }

    public Timer? DisableTimer { get; set; }

    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        DisableTimer = GetNode<Timer>("DisableTimer");
        DisableTimer.Timeout += OnDisableTimerTimeout;
        AreaEntered += OnAreaEntered;
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area is IHitbox hitbox)
        {
            if (HurtBoxType == HurtBoxEnum.Cooldown)
            {
                CollisionShape?.CallDeferred("set", "disable", true);
                DisableTimer?.Start();
            }
            else if (HurtBoxType == HurtBoxEnum.HitOnce)
            {
                //CollisionShape?.QueueFree();
            }
            else
            {
                hitbox.TempDisable();
            }

            hitbox.Hit();
        }

        if (area.Owner is IProjectile proj)
        {
            HealthComponent?.Damage(proj.Damage);
            EmitSignal(SignalName.OnHurtBoxCollision, proj.Damage);
            EmitSignal(SignalName.OnHurtBoxKnockBack, proj.Angle, proj.KnockBackAmount);
        }
    }

    public void OnDisableTimerTimeout()
    {
        CollisionShape?.CallDeferred("set", "disable", false);
    }
}
