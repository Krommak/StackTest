using Game.Ecs.Components;
using Leopotam.EcsLite;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class AnimationSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerFilter;
        private EcsPool<PlayerComponent> _playerPool;

        [Inject]
        public AnimationSystem(EcsWorld world)
        {
            _world = world;

            _playerFilter = _world.Filter<PlayerComponent>().End();
            _playerPool = _world.GetPool<PlayerComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var item in _playerFilter)
            {
                var playerComponent = _playerPool.Get(item);
                if(!playerComponent.PlayerRb.IsSleeping())
                {
                    playerComponent.Animator.SetFloat("Speed_f", playerComponent.PlayerRb.velocity.magnitude);
                }
            }
        }
    }
}