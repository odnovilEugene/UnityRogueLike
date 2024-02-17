using System;
using RogueLike.Interfaces.Objects;
using RogueLike.Settings;



namespace RogueLike.Components.Core
{
    public sealed class Map : IMap
    {
        private static readonly Lazy<Map> lazy = new(() => new Map(), false);

        public static Map Instance { get { return lazy.Value; } }

        public static int Height { get; } = MapSettings.Height;
        public static int Width { get; } = MapSettings.Width;
        public GameObject[,] Field { get; set; }
        public MazeGenerator MazeGenerator { get; }

        public Map()
        {
            MazeGenerator = new MazeGenerator(Width, Height);
            Field = MazeGenerator.Generate();
        }
        
        public override string ToString()
        {
            string stringMap = "";
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    stringMap += this[x, y];
                }
                stringMap += "\n";
            }
            return string.Format(stringMap);
        }
    
        public GameObject this [Position2D pos] {
            get => Field[pos.X, pos.Y];
            set => Field[pos.X, pos.Y] = value;
        }

        public GameObject this [int x, int y] {
            get => Field[x, y];
            set => Field[x, y] = value;
        }
    }
}
