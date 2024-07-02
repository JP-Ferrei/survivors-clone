using Godot;

public partial class KoboldWeak : CharacterBody2D
{
    [Export]
    public float MovementSpeed { get; set; } = 20.0f;

    [Export]
    public HealthComponent HealthComponent { get; set; }

    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }

    [Export]
    public AudioStreamPlayer HitSound { get; set; }

    [Export]
    public HitBox HitBox { get; set; }

    [Export]
    public HurtBox HurtBox { get; set; }

    [Export]
    public PackedScene? DeathAnimation { get; set; }

    [Export]
    public float KnockBackRecovery { get; set; } = 3.5f;

    private Vector2 _knockBack;
    private Node _player;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("Player");
        Sprite?.Play("walk");
        HealthComponent.OnDeath += Death;
        HurtBox.OnHurtBoxCollision += OnHurtBoxCollision;
        HurtBox.OnHurtBoxKnockBack += OnHurtBoxKnockBack;
    }

    private void OnHurtBoxKnockBack(Vector2 angle, int knockBackAmout)
    {
        _knockBack = angle * knockBackAmout;
    }

    private void OnHurtBoxCollision(int damage)
    {
        AnimationPlayer?.Play("Hit");
        HitSound?.Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = GlobalPosition.DirectionTo((Vector2)_player.Get(Node2D.PropertyName.GlobalPosition));

        if (direction.X is not 0)
            Sprite.FlipH = direction.X * -1 < 0;

        Velocity = direction * MovementSpeed;
        HandleKnockBack();
        MoveAndSlide();
    }

    private void HandleKnockBack()
    {
        _knockBack = _knockBack.MoveToward(Vector2.Zero, KnockBackRecovery);
        Velocity += _knockBack;
    }

    private void Death()
    {
        var explosion = DeathAnimation?.Instantiate<Explosion>();
        if (explosion is not null)
        {
            explosion.GlobalPosition = GlobalPosition;
            explosion.Scale = Sprite.Scale;
            GetParent().CallDeferred(MethodName.AddChild, explosion);
        }

        QueueFree();
    }
}
