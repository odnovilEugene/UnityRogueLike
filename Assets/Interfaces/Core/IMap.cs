using RogueLike.Components;
using RogueLike.Components.Core;

namespace RogueLike.Interfaces.Objects
{
    public interface IMap : IReadOnlyMap
    {
        static int Seed { get; set; }
    }

    public interface IReadOnlyMap
    {
        static int Height { get; }
        static int Width { get; }
        MazeGenerator MazeGenerator { get; }
        GameObject[,] Field { get; }
    }
}