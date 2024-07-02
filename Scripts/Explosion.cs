using Godot;

public partial class Explosion : Sprite2D
{
    [Export]
    public AudioStreamPlayer2D? ExplosionSound { get; set; }

    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }
    public override void _Ready()
    {
        AnimationPlayer.Play("explode");
        ExplosionSound?.Play();
        AnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animName)
    {
        QueueFree();
    }
}
