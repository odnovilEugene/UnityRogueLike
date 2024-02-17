using RogueLike.Components.Core;
using RogueLike.Components.StaticObjects;
using RogueLike.Interfaces.Objects;

namespace RogueLike.Components.MovingGameObject
{
    public class Player : GameObject, ILivingGameObject
    {
        public int MaxHp { get; }
        public int Hp { get; private set; }
        public int Attack { get; }
        public bool IsDead { get => Hp <= 0; }

        public Player(Position2D pos)
        {
            Position = pos;
            Symbol = Settings.ObjectSymbols.PlayerSymbol;
            MaxHp = 10;
            Hp = MaxHp;
            Attack = 2;
        }

        public void Move((int, int) direction) 
        {
            (int dx, int dy) = direction;
            Position2D newPos = new(Position.X + dx, Position.Y + dy);
            var objectOnCell = Map.Instance[newPos];
            switch (objectOnCell)
            {
                case IStaticGameObject staticObject:
                    if (staticObject.IsPassable)
                    {
                        if (staticObject is FirstAidKit aidKit)
                            Heal(aidKit.HealAmount);
                        ChangePosition(newPos);
                    }
                    break;
                case ILivingGameObject livingGameObject:
                    livingGameObject.TakeDamage(Attack);
                    if (livingGameObject.IsDead)
                    {
                        ChangePosition(newPos);
                    }
                    break;
                case Projectile projectile:
                    projectile.BlowUp();
                    ChangePosition(newPos);
                    break;
            }
        }

        public void TakeDamage(int amount)
        {
            Hp -= amount;
        }

        private void Heal(int amount)
        {
            int newHp = Hp + amount;
            Hp = MaxHp <= newHp ? MaxHp : newHp;
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }

        public string GetInfo()
        {
            var className = GetType().Name;
            return $"{className}: Hp {Hp} / {MaxHp}, Position: {Position}";
        }
        public void Die() {}
        public void ChangePosition(Position2D newPosition) 
        {
            Map.Instance[Position] = new Empty(Position);
            Map.Instance[newPosition] = this;
            Position = newPosition;
        }
    }
}