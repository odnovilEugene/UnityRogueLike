using RogueLike.Components.Core;
using RogueLike.Interfaces.Objects;

using static RogueLike.Settings.ObjectSymbols;

namespace RogueLike.Components.StaticObjects
{
    public class FirstAidKit : GameObject, IStaticGameObject
    {
        public bool IsPassable { get; }

        public int HealAmount { get; }

        public FirstAidKit(Position2D pos, int healAmount = 5)
        {
            Position = pos;
            Symbol = FirstAidKitSymbol;
            IsPassable = true;
            HealAmount = healAmount;
        }

        public void SelfDestruct()
        {
            Map.Instance[Position] = new Empty(Position);
        }
    }
}