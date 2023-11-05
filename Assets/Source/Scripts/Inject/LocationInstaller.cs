using Game.Ecs.Components;
using Game.Ecs.Factory;
using Game.Ecs.Services;
using Game.MonoBehaviours.UI;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
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
        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private StackSettings _stackSettings;
        [SerializeField]
        private ScoreService _scoreService;

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
            BindUI();
        }

        private void BindPlayer()
        {
            var playerInstance =
                Container.InstantiatePrefab(_playerUnit, _spawnPosition, Quaternion.identity, null);
            Container.Bind()
                .FromInstance(playerInstance)
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerSettings>()
                .FromInstance(_playerSettings)
                .AsSingle()
                .NonLazy();

            Container.Bind<StackSettings>()
                .FromInstance(_stackSettings)
                .AsSingle()
                .NonLazy();

            var entity = _world.NewEntity();
            ref var view = ref _world.GetPool<View>().Add(entity);
            view.Value = playerInstance.transform;
            ref var playerComponenet = ref _world.GetPool<PlayerComponent>().Add(entity);
            playerComponenet.PlayerRb = playerInstance.GetComponent<Rigidbody>();
            playerComponenet.Animator = playerInstance.GetComponent<Animator>();
            ref var stack = ref _world.GetPool<Stack>().Add(entity);
            stack.Value = new Stack<Transform>();
            var stackObj = new GameObject("Stack").transform;
            stackObj.parent = playerInstance.transform;
            stackObj.position = Vector3.back;
            stack.StackTransform = stackObj;
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

        private void BindUI()
        {
            Container.Bind<IScoreService>()
                .To<ScoreService>()
                .FromInstance(_scoreService)
                .AsSingle();
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

    [Serializable]
    public class PlayerSettings
    {
        public float MovementSpeed;
        public float RotationSpeed;
        public Vector3 RelativeCameraPosition;
    }

    [Serializable]
    public class StackSettings
    {
        public int StackSize;
        public Vector3 StackElementSize;
        public float StackElementMargin;
        public float JumpPower;
        public float JumpDuration;
        public float TimeForCollectToCauldron;
    }
}