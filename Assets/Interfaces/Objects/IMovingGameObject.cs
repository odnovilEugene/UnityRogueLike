using RogueLike.Components.Core;

namespace RogueLike.Interfaces.Objects
{
    public interface IMovingGameObject
    {

        (int, int) ChooseDirection();

        void Move();
    }
}