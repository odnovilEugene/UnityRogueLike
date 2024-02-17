using RogueLike.Components.Core;

namespace RogueLike.Interfaces.Objects
{
    public interface IGameObject
    {
        public Position2D Position { get; set; }
        public char Symbol { get; }
    }
}