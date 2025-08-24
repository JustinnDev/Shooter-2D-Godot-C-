using Godot;

namespace Game
{
    public partial class Bullet : Node2D
    {
        [ExportGroup("Physics2D")]
        private Vector2 _velocity;
        private int _velocityLimit = 120;
        private Vector2I _direction = Vector2I.Right;

        [ExportGroup("Animations")]
        private AnimatedSprite2D _animator;

        [ExportGroup("Timers")]
        private Timer _animationTimer;
        private Timer _destroyTimer;

        [ExportGroup("States")]
        private bool _shooting;

        public override void _Ready()
        {
            _shooting = true;

            _animator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

            _TimerConfiguration();
        }

        public override void _PhysicsProcess(double delta)
        {
            _velocity.X = ((_velocity.X + _velocityLimit) * (float)delta) * _direction.X;
            Position += _velocity;

            _TimersController(delta);
            _AnimationController();
        }

        public void SetDirection(Vector2I direction) => _direction = direction;

        private void _AnimationController()
        {
            if (_shooting)
            {
                _animator.Play(AnimationsMaps.Bullet.shoot);
                return;
            }

            _animator.Play(AnimationsMaps.Bullet.Default);
        }

        private void _TimersController(double delta)
        {
            _animationTimer.Update(
                whenToOperation: _shooting,
                actionOfOperation: null,
                whenToStop: _animationTimer.OnLimit(),
                actionOfStop: () => _shooting = false,
                delta:delta
                );

            _destroyTimer.Update(
                whenToOperation: true,
                actionOfOperation: null,
                whenToStop: _destroyTimer.OnLimit(),
                actionOfStop: () => QueueFree(),
                delta: delta,
                actionReset: false
                );
        }

        private void _TimerConfiguration()
        {
            _animationTimer = new();
            _animationTimer.SetLimit(0.1f);

            _destroyTimer = new();
            _destroyTimer.SetLimit(1f);
        }
    }
}
