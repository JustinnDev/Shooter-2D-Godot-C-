using Godot;

namespace Game
{
    public partial class Weapon : Area2D, IConsumable
    {
        public void Consume(Player consumer)
        {
            consumer.state = Player.State.Weaponed;
            QueueFree();
        }
    }
}