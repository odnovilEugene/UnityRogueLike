using RogueLike.Components.MovingGameObject;
using RogueLike.Components.StaticObjects;
using RogueLike.Components.Render;
using RogueLike.Settings;
using RogueLike.Interfaces.Objects;
using System;
using System.Collections.Generic;
using Random = System.Random;

namespace RogueLike.Components.Core
{
    public sealed class Game
    {
        private static readonly Lazy<Game> lazy = new(() => new Game(), false);
        
        public static Game Instance { get { return lazy.Value; } }
        public static event Action OnTurn;
        public static Random Random { get; } = new Random(MapSettings.Seed != -1 ? MapSettings.Seed : (int)DateTime.Now.Ticks);
        public int Level { get; private set; } = 0;
        public Player Player { get; private set; }
        public Dictionary<Position2D, ILivingGameObject> Enemies { get; } = new();
        public bool IsGameOver => Player.Hp <= 0;
        public bool LevelDone => Enemies.Count == 0;

        public Game()
        {
            Player = new Player(MapSettings.start);
            Initialize();
        }

        public void Initialize(bool startCorner = true)
        {
            Level++; 
            int MapHeight = Map.Height;
            int MapWidth = Map.Width;

            int enemiesNumber = MapWidth / 8 + Level;
            int propNumber = (MapWidth / 8) - Level >= 1 ? (MapWidth / 8) - Level : 1;

            Map.Instance.Field = Map.Instance.MazeGenerator.Generate();

            Player.Position = startCorner ? MapSettings.start : MapSettings.finish;
            Map.Instance[Player.Position] = Player;
            

            Range widthR = startCorner ? new(3, MapWidth - 1) : new(0, MapWidth - 3);
            Range heightR = startCorner ? new(3, MapHeight - 1) : new(0, MapHeight - 3);
            GenerateEnemies(enemiesNumber, widthR, heightR);
            GenerateProps(propNumber, widthR, heightR);
        }

        public static void RenderGame()
        {
            Renderer.PrintGame();
        }

        private void GenerateEnemies(int n, Range xR, Range yR)
        {
            int counter = 0;
            int enemyCount = 0;
            while (enemyCount < n)
            {
                if (counter > 100)
                {
                    Console.WriteLine("No place for enemies");
                    break;
                }
                Position2D enemyPos = Position2D.GetRandom(xR, yR);
                if (Map.Instance[enemyPos] is Empty)
                {
                    Enemies.Add(enemyPos, (Random.Next(0, 100) % 2 == 0) ? new Zombie(enemyPos) : new Shooter(enemyPos));
                    Map.Instance[enemyPos] = (GameObject)Enemies[enemyPos];
                    enemyCount++;
                }
                counter++;
            }
        }

        private void GenerateProps(int n, Range xR, Range yR)
        {
            int counter = 0;
            int propsCount = 0;
            while (propsCount < n)
            {
                if (counter > 100)
                {
                    Console.WriteLine("No place for enemies");
                    break;
                }
                Position2D propPos = Position2D.GetRandom(xR, yR);
                if (Map.Instance[propPos] is Empty)
                {
                    Map.Instance[propPos] = new FirstAidKit(propPos);
                    propsCount++;
                }
                counter++;
            }
        }

        public void MakeTurn((int, int) direction)
        {
            Player.Move(direction);
            OnTurn?.Invoke();
        }

        public GameObject[,] GetRenderData()
        {
            return Map.Instance.Field;
        }
    }
}