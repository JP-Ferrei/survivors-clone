using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float MovementSpeed { get; set; } = 40.0f;

    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    [Export]
    public HurtBox HurtBox { get; set; }
    public override void _Ready()
    {
        HurtBox = GetNode<HurtBox>("HurtBox");
    }

    public override void _Process(double delta)
    {
        var direction = Input.GetVector("left", "right", "up", "down").Normalized();

        if (direction.X is not 0)
        {
            if (direction.X > 0)
                Transform = Transform2D.FlipX.Translated(Transform.Origin);
            else
                Transform = new Transform2D(0, Transform.Origin);
        }

        if (direction != Vector2.Zero)
            Sprite.Play("walk");
        else
            Sprite.Pause();

        Velocity = direction * MovementSpeed;
        MoveAndSlide();
    }
}
