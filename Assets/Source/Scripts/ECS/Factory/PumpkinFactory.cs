using Game.Ecs.Components;
using Game.Inject.Installers;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Factory
{
    public class PumpkinFactory : IDisposable
    {
        private List<Pumpkin> _pumpkins;
        private EcsWorld _world;
        private GameObject[] _pumpkinsPrefabs;
        private Transform _containerForDisable;

        [Inject]
        public PumpkinFactory(EcsWorld world, PumpkinSpawnSettings spawnSettings)
        {
            _world = world;
            _pumpkinsPrefabs = spawnSettings.PumpkinPrefabs;

            _pumpkins = new List<Pumpkin>();
            _containerForDisable = new GameObject("PoolOfPumpkins").transform;
        }

        public void SetPumpkin(Transform parent)
        {
            foreach (var item in _pumpkins)
            {
                if(!item.IsActive)
                {
                    item.Enable(parent);
                    return;
                }
            }
            CreateNewPumpkin(parent);
        }

        public void Return(int entity)
        {
            _pumpkins.ForEach(x=>
            {
                if(x.Object.activeInHierarchy && x.CompareEntity(entity))
                {
                    x.Disable(_containerForDisable);
                    return;
                }
            });
        }

        public void Dispose()
        {
            _pumpkins = null;
        }

        private void CreateNewPumpkin(Transform parent)
        {
            _pumpkins.Add(new Pumpkin(
                _pumpkinsPrefabs[UnityEngine.Random.Range(0, _pumpkinsPrefabs.Length)],
                _world,
                parent));
        }

        private class Pumpkin
        {
            public GameObject Object { get; private set; }
            public EcsPackedEntity Entity { get; private set; }
            public bool IsActive => Object.activeInHierarchy;
            private EcsWorld _world { get; set; }

            public Pumpkin(GameObject gameObject, EcsWorld world, Transform parent)
            {
                Object = GameObject.Instantiate(gameObject, parent);
                _world = world;
                Enable(parent);
            }

            public void Enable(Transform parent)
            {
                Object.transform.parent = parent;
                Object.SetActive(true);

                var entity = _world.NewEntity();
                ref var viewComponent = ref _world.GetPool<View>().Add(entity);
                viewComponent.Value = Object.transform;
                Entity = _world.PackEntity(entity);
            }

            public bool CompareEntity(int otherEntity)
            {
                if (Entity.Unpack(_world, out var entity))
                    return entity == otherEntity;

                return false;
            }

            public void Disable(Transform poolTransform)
            {
                if(Entity.Unpack(_world, out var entity))
                    _world.DelEntity(entity);

                Object.transform.parent = poolTransform;
                Object.transform.position = Vector3.zero;
                Object.SetActive(false);
            }
        }
    }
}