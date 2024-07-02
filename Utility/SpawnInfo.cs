using Godot;

[GlobalClass]
public partial class SpawnInfo : Resource
{
    [Export]
    public int TimerStart { get; set; }

    [Export]
    public int TimerEnd { get; set; }

    [Export]
    public PackedScene Enemy { get; set; }

    [Export]
    public int EnemyNumber { get; set; }

    [Export]
    public int EnemySpawnDelay { get; set; }
}
