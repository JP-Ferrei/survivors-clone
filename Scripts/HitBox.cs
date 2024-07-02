using Godot;

public partial class HitBox : Area2D, IHitbox
{
    [Export]
    public int Damage { get; set; } = 10;

    public CollisionShape2D CollisionShape { get; set; }
    public Timer DisableTimer { get; set; }

    [Signal]
    public delegate void OnHitEventHandler();

    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        DisableTimer = GetNode<Timer>("DisableTimer");
        DisableTimer.Timeout += OnDisableTimerTimeout;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void TempDisable()
    {
        GD.Print($"{GetParent().Name} hitBox disabled");
        CollisionShape.CallDeferred("set", "disable", true);
        DisableTimer.Start();
    }

    public void OnDisableTimerTimeout()
    {
        GD.Print($"{GetParent().Name} hitBox enabled");
        CollisionShape.CallDeferred("set", "disable", false);
    }

    public void Hit()
    {
        EmitSignal(SignalName.OnHit);
    }
}

public interface IHitbox
{
    int Damage { get; set; }
    void Hit();
    void TempDisable();
}
