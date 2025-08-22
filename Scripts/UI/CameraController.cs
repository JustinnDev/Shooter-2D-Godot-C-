using Godot;

namespace Game
{
    public partial class CameraController : Camera2D
    {
        [Export] public Node2D target;

        public override void _PhysicsProcess(double delta)
        {
            Position = target.Position;
        }
    }
}