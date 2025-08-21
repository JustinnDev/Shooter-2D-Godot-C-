using Godot;

namespace Game
{
    public partial class Player : CharacterBody2D
    {

        [ExportGroup("Physics2D")]
        private Vector2 _velocity;
        private int _speed = 100;
        private int _gravity = 100;

        [ExportGroup("Animations")]
        private AnimatedSprite2D _animator;
        private SpriteFrames _spriteFrames;

        [ExportGroup("States")]
        private bool _isRunning => Mathf.Abs(_velocity.X) > 0;
        private bool _isJumping;

        [ExportGroup("Timers")]
        [Export] private Timer _jumpTimer = new();

        public override void _Ready()
        {
            _animator = GetNode<AnimatedSprite2D>("Sprite2D/AnimatedSprite2D");
            _spriteFrames = _animator.SpriteFrames;
        }

        public override void _PhysicsProcess(double delta)
        {
            _AnimationsController();
            _PhysicsController(delta);
        }

        // Sprites Behaviour
        #region
        private void _AnimationsController()
        {
            _animator.FlipH = Mathf.Sign(_velocity.X) == -1;

            if (_isRunning)
            {
                _animator.Play(AnimationsMaps.Player.run);
                return;
            }

            _animator.Play(AnimationsMaps.Player.Default);
        }
        #endregion

        // Physics Controller Behaviour
        #region
        private void _PhysicsController(double delta)
        {
            _velocity.X = (Input.GetAxis(InputMap.Left, InputMap.Right) * _speed) * (float)delta;

            if (Input.GetActionRawStrength(InputMap.Up) > 0)
            {
                GD.Print("Salto");
            }

            Position += _velocity;
        }
        #endregion


        // Timers Controller 
        #region
        private void _TimersController()
        {
            _jumpTimer.Update(
                whenToOperation: false,
                actionOfOperation: null,
                whenToStop: false,
                actionOfStop: null
                );
        }
        #endregion
    }
}
