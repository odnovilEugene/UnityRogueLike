using System;
using RogueLike.Settings;

namespace RogueLike.Components.Core
{
    public struct Position2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public readonly bool InBound => X > 0 && X < MapSettings.Width - 1 && Y > 0 && Y < MapSettings.Height - 1;

        public Position2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position2D(Position2D pos)
        {
            X = pos.X;
            Y = pos.Y;
        }
        
        public static bool operator ==(Position2D a, Position2D b) => (a.Y == b.Y) && (a.X == b.X);

        public static bool operator !=(Position2D a, Position2D b) => (a.Y != b.Y) || (a.X != b.X);

        public static bool operator ==(Position2D a, (int, int) coords) => (coords.Item1 == a.X) && (coords.Item2 == a.Y);

        public static bool operator !=(Position2D a, (int, int) coords) => (coords.Item1 != a.X) || (coords.Item2 != a.Y);

        public override readonly bool Equals(object obj)
        {
            return obj is Position2D pos &&
                   X == pos.X &&
                   Y == pos.Y;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Position2D GetRandom(Range x, Range y)
        {
            int randX = Game.Random.Next(x.Start.Value, x.End.Value);
            int randY = Game.Random.Next(y.Start.Value, y.End.Value);
            return new Position2D(randX, randY);
        }

        public override readonly string ToString() => $"({X}, {Y})";
    }
}