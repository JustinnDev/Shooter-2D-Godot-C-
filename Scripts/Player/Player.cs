using Godot;

namespace Game
{
    public partial class Player : CharacterBody2D
    {
        [ExportGroup("Prefabs")]
        private PackedScene _bullet;

        [ExportGroup("Area2D/Collision2D")]
        private Area2D _area;

        [ExportGroup("Physics2D")]
        private Vector2 _velocity;
        private int _aceleration = 1000;
        private int _velocityLimit = 120;
        private int _jump = 5000;
        private int _gravity = 300;

        [ExportGroup("Animations")]
        private AnimatedSprite2D _animator;
        private SpriteFrames _spriteFrames;
        private Vector2I _faceDirection;

        [ExportGroup("States")]
        public State state;
        private bool _isShooting;
        private bool _isRunning;
        private bool _isJumping;

        private bool _isReadyForShooting;
        private bool _isReadyForJumping;

        [ExportGroup("Timers")]
        private Timer _jumpTimer;
        private Timer _shootTimer;

        public override void _Ready()
        {
            _animator = GetNode<AnimatedSprite2D>("Sprite2D/AnimatedSprite2D");
            _area = GetNode<Area2D>("Area2D");
            _bullet = GD.Load<PackedScene>("res://Prefabs/Consumable/Bullet.tscn");
            _spriteFrames = _animator.SpriteFrames;

            _TimerConfiguration();

            _area.AreaEntered += _OnArea2DBodyEntered;
            _area.AreaExited += _OnArea2DBodyExit;
        }

        public override void _PhysicsProcess(double delta)
        {
            InputMap.Process();

            _ActionController();
            _TimersController(delta);
            _PhysicsController(delta);
            _ShootController();
            _AnimationsController();
        }

        // Sprites Behaviour
        #region
        private void _AnimationsController()
        {
            if(Mathf.Abs(_velocity.X) > 0)
            {
                _animator.FlipH = Mathf.Sign(_velocity.X) == -1;
                _faceDirection = _animator.FlipH ? Vector2I.Left : Vector2I.Right;
            }

            if (_isShooting)
            {
                _animator.Play(AnimationsMaps.Player.shoot);
                return;
            }

            if (_isRunning)
            {
                _animator.Play(state == State.Weaponed ? AnimationsMaps.Player.run_weapon : AnimationsMaps.Player.run);
                return;
            }

            _animator.Play(state == State.Weaponed ? AnimationsMaps.Player.Default_Weapon : AnimationsMaps.Player.Default);
        }
        #endregion

        // Physics, Areas, Actions Controller Behaviour
        #region
        private void _ShootController()
        {
            if (!_isShooting)
                return;

            Node2D currentBullet = _bullet.Instantiate<Node2D>();
            currentBullet.GetNode<Bullet>(".").SetDirection(_faceDirection);
            AddChild(currentBullet);
        }

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

        private void _OnArea2DBodyEntered(Area2D area)
        {
            area.GetNode<IConsumable>(".").Consume(this);
        }
        
        private void _OnArea2DBodyExit(Area2D area)
        {

        }

        private void _ActionController()
        {
            if(InputMap.Shoot.GetActionDown() && _isReadyForShooting)
            {
                _isShooting = state == State.Weaponed ? true : false;
            }

            _isReadyForShooting = _shootTimer.count == 0;

            if (InputMap.Up.GetActionDown() && _isReadyForJumping)
            {
                _isJumping = true;
            }

            _isReadyForJumping = IsOnFloor();

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

            _shootTimer.Update(
                whenToOperation: _isShooting,
                actionOfOperation: () => _isReadyForShooting = false,
                whenToStop: _shootTimer.OnLimit(),
                actionOfStop: () => _isShooting = false,
                delta:delta
                );
        }

        private void _TimerConfiguration()
        {
            _jumpTimer = new();
            _jumpTimer.SetLimit(0.05f);

            _shootTimer = new();
            _shootTimer.SetLimit(0.15f);

        }
        #endregion

        // Structs and Enums
        #region
        public enum State
        {
            Default,
            Weaponed
        }
        #endregion
    }
}
