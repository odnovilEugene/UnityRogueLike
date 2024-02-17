using System;
using System.Collections.Generic;
using RogueLike.Components.Core;
using RogueLike.Interfaces.Objects;

namespace RogueLike.Components.Render
{
    public class Renderer
    {
        public static void PrintGame()
        {
            Console.Clear();
            Console.WriteLine($"Level : {Game.Instance.Level}");
            Console.WriteLine(Map.Instance);
            PrintInfo();
        }
        public static void PrintInfo()
        {
            Console.WriteLine(Game.Instance.Player.GetInfo());
            foreach (KeyValuePair<Position2D, ILivingGameObject> enemy in Game.Instance.Enemies)
            {
                Console.WriteLine(enemy.Value.GetInfo());
            }
        }
    }
}