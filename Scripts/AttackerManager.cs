using Godot;

public partial class AttackerManager : Node2D
{
    [Export]
    public Godot.Collections.Array<PackedScene> Attacks { get; set; } = new();

    public override void _Process(double delta)
    {
    }

    private void SubscribeToAttacks()
    {
    }
}
