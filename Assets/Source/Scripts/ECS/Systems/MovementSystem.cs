using Game.Ecs.Components;
using Game.Ecs.Services;
using Game.Inject.Installers;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class MovementSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private IInputService _inputService;
        private EcsFilter _playerFilter;
        private EcsPool<PlayerComponent> _playerPool;
        private float _movementSpeed;

        [Inject]
        public MovementSystem(EcsWorld world, IInputService inputService, PlayerSettings playerSettings)
        {
            _world = world;
            _inputService = inputService;
            _movementSpeed = playerSettings.MovementSpeed;

            _playerFilter = _world.Filter<PlayerComponent>().End();
            _playerPool = _world.GetPool<PlayerComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if(_inputService.IsPressed)
            {
                foreach (var item in _playerFilter)
                {
                    var rb = _playerPool.Get(item).PlayerRb;

                    rb.velocity = new Vector3(_inputService.Horizontal * _movementSpeed, rb.velocity.y, _inputService.Vertical * _movementSpeed);
                    rb.transform.rotation = Quaternion.LookRotation(rb.velocity);
                }
            }
        }
    }
}