using Game.Ecs.Components;
using Game.Inject.Installers;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.MonoBehaviours
{
    [RequireComponent(typeof(SphereCollider))]
    public class PumpkinPointMono : MonoBehaviour
    {
        private PumpkinSpawnSettings _spawnSettings;
        private EcsWorld _world;
        private EcsPackedEntity _entity;

        [Inject]
        public void Construct(EcsWorld world, PumpkinSpawnSettings spawnSettings)
        {
            _spawnSettings = spawnSettings;
            _world = world;

            var entity = world.NewEntity();

            ref var viewComponent = ref world.GetPool<View>().Add(entity);
            viewComponent.Value = transform;
            world.GetPool<PumpkinPoint>().Add(entity);

            _entity = world.PackEntity(entity);

            NewPumpkin();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (transform.childCount == 0) return;

            var moveToStack = _world.NewEntity();
            ref var component = ref _world.GetPool<MoveToStack>().Add(moveToStack);
            var pumpkin = transform.GetChild(0);
            component.Value = pumpkin;
            component.OnApplyAction = () => NewPumpkin();
        }

        private void NewPumpkin()
        {
            if (_entity.Unpack(_world, out var entity))
            {
                ref var spawnComponent = ref _world.GetPool<NewPumpkin>().Add(entity);
                spawnComponent.SpawnTimer = _spawnSettings.GetSpawnTime();
            }
        }
    }
}