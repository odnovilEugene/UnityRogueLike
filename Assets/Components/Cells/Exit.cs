using RogueLike.Components.Core;
using RogueLike.Interfaces.Objects;

using static RogueLike.Settings.ObjectSymbols;

namespace RogueLike.Components.StaticObjects
{
    public class Exit : GameObject, IStaticGameObject
    {
        public bool IsPassable { get; }
        
        public Exit(Position2D pos)
        {
            Position = pos;
            Symbol = ExitSymbol;
            IsPassable = true;
        }
    }
}