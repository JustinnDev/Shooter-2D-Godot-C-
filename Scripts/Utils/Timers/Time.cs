using Godot;

namespace Game
{
    public partial class Time : Node
    {
        public static double deltaTime { get; private set; }
        public static double time { get; private set; }
        public static double unscaledDeltaTime { get; private set; }

        public override void _Ready()
        {
            deltaTime = 0;
            time = 0;
            unscaledDeltaTime = 0;
        }

        public override void _Process(double delta)
        {
            unscaledDeltaTime = delta;
        }

        public override void _PhysicsProcess(double delta)
        {
            deltaTime = delta;

            
        }
    }
}