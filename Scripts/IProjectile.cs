
using Godot;

namespace SurvivorsClone.Scripts;
internal interface IProjectile
{
    int Damage { get; }

    int KnockBackAmount { get; }

    Vector2 Angle { get; }

}
