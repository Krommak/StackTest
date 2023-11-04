namespace Game.Ecs.Services
{
    public interface IInputService
    {
        public float Vertical { get; protected set; }
        public float Horizontal { get; protected set; }
    }
}