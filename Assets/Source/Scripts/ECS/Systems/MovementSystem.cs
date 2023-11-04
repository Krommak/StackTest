using Game.Ecs.Services;
using Leopotam.EcsLite;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class MovementSystem : IEcsRunSystem
    {
        IInputService _inputService;

        [Inject]
        public MovementSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}