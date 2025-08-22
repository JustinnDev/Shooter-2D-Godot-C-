using Godot;

namespace Game
{
    public partial class Player : CharacterBody2D
    {
        [ExportGroup("Physics2D")]
        private Vector2 _velocity;
        private int _aceleration = 1000;
        private int _velocityLimit = 120;
        private int _jump = 5000;
        private int _gravity = 300;

        [ExportGroup("Animations")]
        private AnimatedSprite2D _animator;
        private SpriteFrames _spriteFrames;

        [ExportGroup("States")]
        private bool _isRunning;
        private bool _isJumping;

        [ExportGroup("Timers")]
        private Timer _jumpTimer;

        public override void _Ready()
        {
            _animator = GetNode<AnimatedSprite2D>("Sprite2D/AnimatedSprite2D");
            _spriteFrames = _animator.SpriteFrames;

            _TimerConfiguration();
        }

        public override void _Process(double delta)
        {
            _TimersController(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            InputMap.Process();

            _ActionController();
            _PhysicsController(delta);
            _AnimationsController();
        }

        // Sprites Behaviour
        #region
        private void _AnimationsController()
        {
            if(Mathf.Abs(_velocity.X) > 0)
            {
                _animator.FlipH = Mathf.Sign(_velocity.X) == -1;
            }

            if (_isRunning)
            {
                _animator.Play(AnimationsMaps.Player.run);
                return;
            }

            _animator.Play(AnimationsMaps.Player.Default);
        }
        #endregion

        // Physics, Actions Controller Behaviour
        #region
        private void _PhysicsController(double delta)
        {
            if(Mathf.Abs(_velocity.X) <= _velocityLimit)
            {
                _velocity.X += ((InputMap.Move.GetAxis() * _aceleration) * (float)delta);
            }
            
            if (_isJumping)
            {
                _velocity.Y = -_jump * (float)delta;                
            }

            else
            {
                _velocity.Y = IsOnFloor() ? 0 : _velocity.Y +  _gravity * (float)delta;
            }

            if (!_isRunning && Mathf.Abs(_velocity.X) > 0)
            {
                if ((int)Mathf.Abs(_velocity.X) == 0)
                {
                    _velocity.X = 0;
                }

                else if(_velocity.X > 0)
                {
                    _velocity.X -= _aceleration * (float)delta;
                    _velocity.X = _velocity.X < 0 ? 0 : _velocity.X;
                }

                else if (_velocity.X < 0)
                {
                    _velocity.X += _aceleration * (float)delta;
                    _velocity.X = _velocity.X > 0 ? 0 : _velocity.X;
                }
            }

            Velocity = _velocity;

            MoveAndSlide();
        }

        private void _ActionController()
        {
            if (InputMap.Up.GetActionDown())
            {
                _isJumping = true;
            }

            _isRunning = InputMap.Move.GetAction();
        }
        #endregion


        // Timers Controller 
        #region
        private void _TimersController(double delta)
        {
            _jumpTimer.Update(
                whenToOperation: _isJumping,
                actionOfOperation: null,
                whenToStop: _jumpTimer.OnLimit(),
                actionOfStop: () => _isJumping = false,
                delta: delta
                );

            GD.Print(_velocity);
        }

        private void _TimerConfiguration()
        {
            _jumpTimer = new();
            _jumpTimer.SetLimit(0.05f);

        }
        #endregion
    }
}
