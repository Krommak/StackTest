using Game.Ecs.Components;
using Game.Ecs.Factory;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class PumpkinSpawnSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private PumpkinFactory _factory;

        private EcsFilter _newPumpkinFilter;
        private EcsPool<NewPumpkin> _newPumpkinPool;
        private EcsPool<View> _viewPool;

        [Inject]
        public PumpkinSpawnSystem(EcsWorld world, PumpkinFactory factory)
        {
            _world = world;
            _factory = factory;

            _newPumpkinFilter = _world.Filter<View>().Inc<PumpkinPoint>().Inc<NewPumpkin>().End();
            _newPumpkinPool = _world.GetPool<NewPumpkin>();
            _viewPool = _world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var item in _newPumpkinFilter)
            {
                ref var component = ref _newPumpkinPool.Get(item);
                component.SpawnTimer -= Time.deltaTime;

                if(component.SpawnTimer < 0)
                {
                    _factory.SetPumpkin(_viewPool.Get(item).Value);
                    _newPumpkinPool.Del(item);
                }
            }
        }
    }
}