using Game.Ecs.Components;
using Game.Inject.Installers;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class CameraMovementSystem : IEcsRunSystem
    {
        private Vector3 _relativeCameraPosition;
        private EcsWorld _world;
        private Transform _cameraTransform;
        private EcsFilter _playerFilter;
        private EcsPool<View> _viewsPool;

        [Inject]
        public CameraMovementSystem(EcsWorld world, Camera camera, PlayerSettings playerSettings)
        {
            _world = world;
            _cameraTransform = camera.transform;
            _relativeCameraPosition = playerSettings.RelativeCameraPosition;
            _playerFilter = _world.Filter<PlayerComponent>().Inc<View>().End();
            _viewsPool = _world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var item in _playerFilter)
            {
               _cameraTransform.position = _viewsPool.Get(item).Value.position + _relativeCameraPosition;
            }
        }
    }
}