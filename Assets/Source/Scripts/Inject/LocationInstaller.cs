using Game.Ecs.Components;
using Game.Ecs.Services;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Inject.Installers
{
    [DisallowMultipleComponent]
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _playerUnit;
        [SerializeField]
        private Vector3 _spawnPosition;
        [SerializeField]
        private Joystick _joystick;

        private EcsWorld _world;
        private Canvas _canvas;

        [Inject]
        private void Construct(EcsWorld world, Canvas canvas)
        {
            _world = world;
            _canvas = canvas;
        }

        public override void InstallBindings()
        {
            BindPlayer();
            BindInputService();
        }

        private void BindPlayer()
        {
            var playerInstance =
                Container.InstantiatePrefab(_playerUnit, _spawnPosition, Quaternion.identity, null);
            Container.Bind()
                .FromInstance(playerInstance)
                .AsSingle()
                .NonLazy();

            var entity = _world.NewEntity();
            var viewPool = _world.GetPool<View>();
            ref var view = ref viewPool.Add(entity);
            view.Value = playerInstance.transform;
            _world.GetPool<PlayerComponent>().Add(entity);
        }

        private void BindInputService()
        {
            var joystick = Container.InstantiatePrefabForComponent<Joystick>(_joystick, _canvas.transform);
            Container.Bind<IInputService>()
                .To<Joystick>()
                .FromInstance(joystick)
                .AsSingle();
        }
    }
}