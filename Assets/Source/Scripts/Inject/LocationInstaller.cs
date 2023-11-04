using Game.Ecs.Components;
using Game.Ecs.Factory;
using Game.Ecs.Services;
using Leopotam.EcsLite;
using System;
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
        [SerializeField]
        private PumpkinSpawnSettings _pumpkinSpawnSettings;

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
            BindPumpkins();
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

        private void BindPumpkins()
        {
            Container.Bind<PumpkinFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<PumpkinSpawnSettings>()
                .FromInstance(_pumpkinSpawnSettings)
                .AsSingle()
                .NonLazy();
        }
    }

    [Serializable]
    public class PumpkinSpawnSettings
    {
        public GameObject[] PumpkinPrefabs;

        [SerializeField]
        private bool _isFixedSpawnTime;

        [SerializeField]
        private float _spawnTime;

        [SerializeField]
        private float _minRandomSpawn;
        [SerializeField]
        private float _maxRandomSpawn;

        public float GetSpawnTime()
        {
            if (_isFixedSpawnTime)
                return _spawnTime;

            return UnityEngine.Random.Range(_minRandomSpawn, _maxRandomSpawn);
        }
    }
}