using RogueLike.Components.Core;
using RogueLike.Components.StaticObjects;
using RogueLike.Interfaces.Objects;

namespace RogueLike.Components.MovingGameObject
{
    public class Projectile : GameObject, IMovingGameObject
    {
        private int Damage { get; }
        private (int, int) Direction { get; }

        public Projectile(Position2D pos, (int, int) direction)
        {
            Position = pos;
            Symbol = Settings.ObjectSymbols.ProjectileSymbol;
            Damage = 1;
            Direction = direction;
            Game.OnTurn += Move;
        }

        public void BlowUp()
        {
            Game.OnTurn -= Move;
            Map.Instance[Position] = new Empty(Position);
        }

        public (int, int) ChooseDirection() => Direction;

        public void Move() {
            (int dx, int dy) = ChooseDirection();
            Position2D newPos = new(Position.X + dx, Position.Y + dy);
            var objectOnCell = Map.Instance[newPos];
            switch (objectOnCell)
            {
                case IStaticGameObject staticObject:
                    if (staticObject.IsPassable)
                    {
                        if (staticObject is FirstAidKit aidKit)
                        {
                            aidKit.SelfDestruct();
                            this.BlowUp();
                        }
                        else
                        {
                            Map.Instance[newPos] = this;
                            Map.Instance[Position] = new Empty(Position);
                            Position = newPos;
                        }
                    }
                    else
                    {
                        this.BlowUp();
                    }
                    break;
                case ILivingGameObject livingGameObject:
                    livingGameObject.TakeDamage(Damage);
                    this.BlowUp();
                    break;
                case Projectile projectile:
                    projectile.BlowUp();
                    this.BlowUp();
                    break;
            }
        }
    }
}