using Game.Ecs.Components;
using Game.Ecs.Factory;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.MonoBehaviours
{
    [RequireComponent(typeof(SphereCollider))]
    public class Сauldron : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsPackedEntity _entity;

        [Inject]
        public void Construct(EcsWorld world)
        {
            _world = world;
        }

        private void OnTriggerEnter(Collider other)
        {
            var entity = _world.NewEntity();

            ref var component = ref _world.GetPool<CollectStack>().Add(entity);
            component.TargetPosition = transform;
            _entity = _world.PackEntity(entity);
        }

        private void OnTriggerExit(Collider other)
        {
            if(_entity.Unpack(_world, out var entity))
            {
                _world.DelEntity(entity);
            }
        }
    }
}
