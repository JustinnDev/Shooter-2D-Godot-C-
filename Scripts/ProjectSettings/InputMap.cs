using System;
using Godot;

namespace Game
{
    public struct InputMap
    {
        public static readonly string Right = "Right";
        public static readonly string Left = "Left";
        public static readonly string Up = "Up";
        public static readonly string Down = "Down";
        public static readonly string Shoot = "Shoot";
    }


//Estructuras 
[Serializable]
    public class Buttons
    {
        public Input dashAxis; //Comprueba si se apreta el boton de dash
        public Input upAxis; //Comprueba si se apreta la flecha de arriba
        public Input downAxis; //Comprueba si se apreta la flecha de abajo
        public Input southAxis; //Comprueba si se apreta el boton sur del mando
        public Input eastAxis; //Comprueba si se apreta el boton este del mando
        public Input powerAxis; //Comprueba si se apreta el boton de super poder
        public Input missileAxis; //Comrpueba si se apreta el boton de disparar misil
        public Input axis; //Obtiene la referencia de la direccion de _axis


        [Serializable]
        public class Input
        {
            [Export] private bool wasUnpressed = true;
            [Export] private float value;

            public bool GetDown()
            {
                if (wasUnpressed && Get())
                {
                    wasUnpressed = false;
                    return true;
                }

                else if (!wasUnpressed && !Get())
                {
                    wasUnpressed = true;
                    return false;
                }

                return false;
            }

            public bool Get() => Mathf.Abs(value) > 0;

            public static implicit operator float(Input a) => a.value;
            public static implicit operator Input(float a) => new Input() { value = a };
        }
    }
}