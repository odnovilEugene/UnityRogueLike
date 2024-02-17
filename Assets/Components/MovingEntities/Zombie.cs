using System;
using RogueLike.Components.Core;
using RogueLike.Components.StaticObjects;
using RogueLike.Interfaces.Objects;

namespace RogueLike.Components.MovingGameObject
{
    public class Zombie : GameObject, IMovingGameObject, ILivingGameObject
    {
        public int MaxHp { get; }
        public int Hp { get; private set; }
        public int Attack { get; }
        public bool IsDead { get => Hp <= 0; }

        public Zombie(Position2D pos)
        {
            Position = pos;
            Symbol = Settings.ObjectSymbols.ZombieSymbol;
            MaxHp = 3;
            Hp = MaxHp;
            Attack = 1;
            Game.OnTurn += Move;
        }

        public (int, int) ChooseDirection()
        {
            Position2D playerPos = Game.Instance.Player.Position;
            (int dx, int dy) = (0, 0);
            if (Position.Y == playerPos.Y)
            {
                (dx, dy) = Position.X - playerPos.X > 0 ? PlayerInput.InputToDirection(PlayerInput.MoveLeftKey) : PlayerInput.InputToDirection(PlayerInput.MoveRightKey);
                Position2D tempPos = Position;
                Position2D tempPlayerPos = playerPos;
                while (Math.Abs(tempPos.X - tempPlayerPos.X) > 1)
                {
                    tempPos.X += dx;
                    if (Map.Instance[tempPos] is not Empty)
                        return (0, 0);
                }
                return (dx, dy);
            }
            else if (Position.X == playerPos.X)
            {
                (dx, dy) = Position.Y - playerPos.Y > 0 ? PlayerInput.InputToDirection(PlayerInput.MoveDownKey) : PlayerInput.InputToDirection(PlayerInput.MoveUpKey);
                Position2D tempPos = Position;
                Position2D tempPlayerPos = playerPos;
                while (Math.Abs(tempPos.Y - tempPlayerPos.Y) > 1)
                {
                    tempPos.Y += dy;
                    if (Map.Instance[tempPos] is not Empty)
                        return (0, 0);
                }
                return (dx, dy);
            }
            return (dx, dy);
        }

        public void Move() 
        {
            (int dx, int dy) = ChooseDirection();
            if ((dx, dy) != (0, 0))
            {
                Position2D newPos = new(Position.X + dx, Position.Y + dy);
                var objectOnCell = Map.Instance[newPos];
                switch (objectOnCell)
                {
                    case IStaticGameObject staticObject:
                        if (staticObject.IsPassable)
                        {
                            if (staticObject is FirstAidKit aidKit)
                                aidKit.SelfDestruct();
                            ChangePosition(newPos);
                        }
                        break;
                    case Player player:
                        player.TakeDamage(Attack);
                        break;
                }
            }
        }

        public void TakeDamage(int amount)
        {
            Hp -= amount;
            if (IsDead)
            {
                Die();
            }
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

        public void Die()
        {
            Game.OnTurn -= Move;
            Game.Instance.Enemies.Remove(Position);
            Map.Instance[Position] = new Empty(Position);
        }

        public void ChangePosition(Position2D newPosition)
        {
            Game.Instance.Enemies.Remove(Position);
            Game.Instance.Enemies.Add(newPosition, this);
            Map.Instance[Position] = new Empty(Position);
            Map.Instance[newPosition] = this;
            Position = newPosition;
        }
    }
}