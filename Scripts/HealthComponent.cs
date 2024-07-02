using Godot;

public partial class HealthComponent : Node2D
{
    private int _health = 0;

    [Export]
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            EmitSignal(SignalName.OnHealthChange, _health);
            if (_health <= 0)
                Die();
        }
    }

    [Export]
    public int MaxHealth { get; set; } = 100;

    [Signal]
    public delegate void OnHealthChangeEventHandler(int health);

    [Signal]
    public delegate void OnDeathEventHandler();

    [Signal]
    public delegate void OnDamageRecievedEventHandler(int damage);

    [Signal]
    public delegate void OnHealingRecievedEventHandler(int healing);

    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public void Damage(int damage)
    {
        EmitSignal(SignalName.OnDamageRecieved, damage);
        Health -= damage;
    }

    public void HealDamage(int healing)
    {
        EmitSignal(SignalName.OnHealingRecieved, healing);
        Health += healing;
    }

    public void Die()
    {
        EmitSignal(SignalName.OnDeath);
        QueueFree();
    }
}
