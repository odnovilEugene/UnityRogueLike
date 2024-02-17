
using RogueLike.Components.Core;
using RogueLike.Components.StaticObjects;
using static RogueLike.Utils.Utils;
using static RogueLike.Settings.ObjectSymbols;
using System;
using RogueLike.Settings;

namespace RogueLike.Components
{
    public class MazeGenerator
    {
        private readonly char[,] _data;
        private readonly int _width;
        private readonly int _height;
        private Random _random;

        public MazeGenerator(int width, int height)
        {
            _random = Game.Random;
            _width = width;
            _height = height;
            _data = new char[_width, _height];
        }

        public GameObject[,] Generate()
        {
            Initialize();
            GenerateMaze(MapSettings.start.X, MapSettings.start.Y);
            MakeAccessible(MapSettings.finish);
            _random = new Random((int)DateTime.Now.Ticks);
            return CharMazeToGameObjectMaze();
        }

        private GameObject[,] CharMazeToGameObjectMaze()
        {
            var gameObjectMaze = new GameObject[_width, _height];
            
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var pos = new Position2D(x, y);
                    if (_data[x, y] == WallSymbol)
                    {                 
                        gameObjectMaze[x, y] = new Wall(pos);
                    }
                    else if (_data[x, y] == EmptyCellSymbol)
                    {
                        gameObjectMaze[x, y] = new Empty(pos);
                    }
                }
            }
            return gameObjectMaze;
        }

        private void Initialize()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _data[x, y] = WallSymbol;
                }
            }
        }

        private void GenerateMaze(int x, int y)
        {
            _data[x, y] = ' ';
            var directions = new (int, int)[] { (0, -2), (2, 0), (0, 2), (-2, 0) };
            directions = Shuffle(directions, _random);

            foreach (var (dx, dy) in directions)
            {
                int newX = x + dx, newY = y + dy;
                if (IsInsideBounds(newX, newY) && _data[newX, newY] != EmptyCellSymbol)
                {
                    _data[newX - dx / 2, newY - dy / 2] = EmptyCellSymbol;
                    GenerateMaze(newX, newY);
                }
            }
        }

        private void MakeAccessible(Position2D finish)
        {   
            var directions = new (int, int)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
            foreach (var (dx, dy) in directions)
            {
                int newX = finish.X + dx, newY = finish.Y + dy;
                if (IsInsideBounds(newX, newY))
                {
                    _data[newX, newY] = EmptyCellSymbol;
                }
            }
        }

        private bool IsInsideBounds(int x, int y)
        {
            return x > 0 && x < _width - 1 && y > 0 && y < _height - 1;
        }
    }
}