using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class IceSpearAttackManager : Node2D, IAttackManager
{
    public Timer IceSpearTimer { get; set; }
    public Timer IceSpearAttackTimer { get; set; }
    public Area2D AttackRange { get; set; }

    [Export]
    public PackedScene IceSpearScene { get; set; }

    private uint _ammo;
    [Export]
    public uint Ammo { get => _ammo; set { _ammo = (uint)Mathf.Clamp(value, 0, BaseAmmo); } }

    [Export]
    public uint BaseAmmo { get; set; } = 1;

    [Export]
    public float AttackSpeed { get; set; } = 1.5f;

    [Export]
    public uint Level { get; set; } = 1;

    private HashSet<Node2D> _enemiesInRange = [];

    public override void _Ready()
    {
        IceSpearTimer = GetNode<Timer>("IceSpearTimer");
        IceSpearAttackTimer = GetNode<Timer>("IceSpearAttackTimer");
        AttackRange = GetNode<Area2D>("AttackRange");

        AttackRange.BodyEntered += OnAttackRangeBodyEntered;
        AttackRange.BodyExited += OnAttackRangeBodyExited;
        IceSpearTimer.Timeout += OnIceSpearTimerTimeout;
        IceSpearAttackTimer.Timeout += OnIceSpearAttackTimerTimeout;
    }

    public override void _Process(double delta)
    {
        Attack();
    }

    public void Attack()
    {
        if (Level == 0) return;

        IceSpearTimer.WaitTime = AttackSpeed;

        if (IceSpearTimer.IsStopped())
            IceSpearTimer.Start();
    }

    private void OnAttackRangeBodyEntered(Node2D body)
    {
        _enemiesInRange.Add(body);
    }

    private void OnAttackRangeBodyExited(Node2D body)
    {
        _enemiesInRange.Remove(body);
    }

    private void OnIceSpearTimerTimeout()
    {
        Ammo += BaseAmmo;
        IceSpearAttackTimer.Start();
    }

    private void OnIceSpearAttackTimerTimeout()
    {
        if (Ammo <= 0)
            return;

        var randomTarget = GetRandomTarget();

        if (randomTarget is null)
            return;

        var iceSpear = IceSpearScene.Instantiate<IceSpear>();
        iceSpear.Position = GlobalPosition;
        iceSpear.Target = randomTarget.Value;
        //iceSpear.Level = Level;
        AddChild(iceSpear);
        Ammo--;
        if (Ammo > 0)
            IceSpearTimer.Start();
        else
            IceSpearTimer.Stop();
    }

    private Vector2? GetRandomTarget()
    {
        if (_enemiesInRange.Count <= 0)
            return null;

        return Random.Shared.GetItems([.. _enemiesInRange], 1)
            .First()
            .GlobalPosition;
    }
}

public interface IAttackManager
{
    void Attack();
}
