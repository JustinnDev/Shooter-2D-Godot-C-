using System;
using Godot;

namespace Game
{
    public struct InputMap
    {
        public static readonly Input Move = new("Left" , "Right");
        public static readonly Input Up = new("Up");
        public static readonly Input Down = new("Down");
        public static readonly Input Shoot = new("Shoot");

        public static void Process()
        {
            Move.Process();
            Up.Process();
            Down.Process();
            Shoot.Process();
        }

        [Serializable]
        public class Input
        {
            [Export] private bool _wasUnpressed = true;
            [Export] private float _value;
            [Export] private string[] _name;
            [Export] private Type type;

            public Input(params string[] _name)
            {
                this._name = _name;
            }
            
            public void Process()
            {
                if (_name.Length > 1)
                {
                    _value = Godot.Input.GetAxis(_name[0], _name[1]);
                    return;
                }

                _value = Godot.Input.GetActionStrength(_name[0]);
            }

            public float GetAxis() => _value;

            public bool GetActionDown()
            {
                if (_wasUnpressed && GetAction())
                {
                    _wasUnpressed = false;
                    return true;
                }

                else if (!_wasUnpressed && !GetAction())
                {
                    _wasUnpressed = true;
                    return false;
                }

                return false;
            }

            public bool GetAction() => Mathf.Abs(_value) > 0;

            public static implicit operator float(Input a) => a._value;
            public static implicit operator Input(float a) => new Input() { _value = a };
        }

    }


}
