using Godot;
using SurvivorsClone.Scripts;

public partial class IceSpear : Node2D, IProjectile
{
    [Export]
    public int Damage { get; set; } = 5;

    [Export]
    public int Health { get; set; } = 2;

    [Export]
    public int Speed { get; set; } = 50;

    [Export]
    public int KnockBackAmount { get; set; } = 100;

    [Export]
    public float AttackSize { get; set; } = 1;

    [Export]
    public Timer Timer { get; set; }

    [Export]
    public HitBox HitBox { get; set; }
    [Export]
    public AudioStreamPlayer SpawnSound { get; set; }

    public Vector2 Target { get; set; } = Vector2.Zero;

    public Vector2 Angle { get; private set; }

    private Node _player;

    public override void _Ready()
    {
        HitBox = GetNode<HitBox>("HitBox");
        Timer = GetNode<Timer>("Timer");
        SpawnSound = GetNode<AudioStreamPlayer>("SpawnSound");

        HitBox.OnHit += EnemyHit;
        Timer.Timeout += OnTimerTimeout;

        _player = GetTree().GetFirstNodeInGroup("player");
        Angle = GlobalPosition.DirectionTo(Target);
        Rotation = Angle.Angle();

        var tween = CreateTween();
        tween
            .TweenProperty(this, "scale", new Vector2(1, 1) * AttackSize, 1)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);
        tween.Play();

        SpawnSound.Play();
    }


    public override void _Process(double delta)
    {
        Position += Angle * Speed * (float)delta;
    }

    public void EnemyHit()
    {
        Health--;

        if (Health <= 0)
            QueueFree();
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
