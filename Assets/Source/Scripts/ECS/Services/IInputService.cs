namespace Game.Ecs.Services
{
    public interface IInputService
    {
        public bool IsPressed => Vertical != 0 || Horizontal != 0;
        public float Vertical { get; protected set; }
        public float Horizontal { get; protected set; }
    }
}