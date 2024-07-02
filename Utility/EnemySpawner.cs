using Godot;
using System;
using System.Linq;

public partial class EnemySpawner : Node2D
{
    private int _timer = 0;

    [Export]
    public Godot.Collections.Array<SpawnInfo> SpawnsInfo { get; set; }
    public Node Player { get; set; }

    [Export]
    public bool IsEnable { get; set; }

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player");
        GetNode<Timer>("Timer").Timeout += EnemySpawnerTimeout;
    }

    private void EnemySpawnerTimeout()
    {
        if (IsEnable is false) return;

        _timer++;
        foreach (var spawnInfo in SpawnsInfo)
        {
            if (_timer >= spawnInfo.TimerStart && _timer <= spawnInfo.TimerEnd)
            {
                if (spawnInfo.EnemySpawnDelay < spawnInfo.EnemySpawnDelay)
                {
                    spawnInfo.EnemySpawnDelay++;
                }
                else
                {
                    spawnInfo.EnemySpawnDelay = 0;
                    SpawnEnemies(spawnInfo);
                }
            }
        }
    }

    private int SpawnEnemies(SpawnInfo spawnInfo)
    {
        var enemySpawned = 0;
        while (enemySpawned < spawnInfo.EnemyNumber)
        {
            var newEnemy = spawnInfo.Enemy.Instantiate();
            var randomPos = GetNewRandoPosition();

            newEnemy.Set(Node2D.PropertyName.GlobalPosition, randomPos);

            AddChild(newEnemy);
            enemySpawned++;
        }

        return enemySpawned;
    }

    private Vector2 GetNewRandoPosition()
    {
        var halfViewPort = (GetViewportRect().Size) / 3;
        var playerLocation = Player.Get(Node2D.PropertyName.GlobalPosition).AsVector2();

        var topLeft = new Vector2(
            x: playerLocation.X - halfViewPort.X,
            y: playerLocation.Y - halfViewPort.Y
        );
        var topRight = new Vector2(
            x: playerLocation.X + halfViewPort.X,
            y: playerLocation.Y - halfViewPort.Y
        );
        var bottomLeft = new Vector2(
            x: playerLocation.X - halfViewPort.X,
            y: playerLocation.Y + halfViewPort.Y
        );
        var bottomRight = new Vector2(
            x: playerLocation.X + halfViewPort.X,
            y: playerLocation.Y + halfViewPort.Y
        );

        var randomPosition = Random.Shared.GetItems(
            [
                (topLeft, topRight),
                (bottomLeft, bottomRight),
                (topLeft, bottomLeft),
                (topRight, bottomRight)
            ],
            1
        ).First();

        return new Vector2()
        {
            X = (float)GD.RandRange(randomPosition.Item1.X, randomPosition.Item2.X),
            Y = (float)GD.RandRange(randomPosition.Item1.Y, randomPosition.Item2.Y)
        };

    }
}
