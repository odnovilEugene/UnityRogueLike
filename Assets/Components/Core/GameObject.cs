using RogueLike.Interfaces.Objects;

namespace RogueLike.Components.Core
{
    public abstract class GameObject : IGameObject
    {
        public Position2D Position { get; set; }

        public char Symbol { get; protected set; }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}