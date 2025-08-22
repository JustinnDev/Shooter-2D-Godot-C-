using Godot;

namespace Game
{
    public partial class Bullet : Node2D
    {
        [ExportGroup("Physics2D")]
        private Vector2 _velocity;
        private int _velocityLimit = 120;

        public override void _PhysicsProcess(double delta)
        {
            _velocity.X = (_velocity.X + _velocityLimit) * (float)delta;
            Position = _velocity;
        }

        public void Consume(Player consumer)
        {

        }
    }
}
