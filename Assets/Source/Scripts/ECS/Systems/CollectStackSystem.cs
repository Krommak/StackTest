using DG.Tweening;
using Game.Ecs.Components;
using Game.Ecs.Factory;
using Game.Inject.Installers;
using Game.MonoBehaviours.UI;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class CollectStackSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private StackSettings _stackSettings;
        private PumpkinFactory _factory;
        private IScoreService _score;

        private EcsFilter _collectStackFilter;
        private EcsFilter _stackFilter;
        private EcsPool<Stack> _stackPool;
        private EcsPool<CollectStack> _collectStackPool;
        private float _lastCollect;

        [Inject]
        public CollectStackSystem(EcsWorld world, StackSettings settings, PumpkinFactory factory, IScoreService score)
        {
            _world = world;
            _stackSettings = settings;
            _factory = factory;
            _score = score;

            _collectStackFilter = _world.Filter<CollectStack>().End();
            _stackFilter = _world.Filter<Stack>().End();
            _stackPool = _world.GetPool<Stack>();
            _collectStackPool = _world.GetPool<CollectStack>();
        }

        public void Run(IEcsSystems systems)
        {
            _lastCollect += Time.deltaTime;

            foreach (var item in _collectStackFilter)
            {
                if(_lastCollect > _stackSettings.TimeForCollectToCauldron)
                {
                    foreach (var stackEntity in _stackFilter)
                    {
                        ref var stack = ref _stackPool.Get(stackEntity);
                        if(stack.Value.TryPop(out var pumpkin))
                        {
                            var target = _collectStackPool.Get(item).TargetPosition;
                            pumpkin.parent = target;

                            pumpkin.DOLocalJump(target.up, _stackSettings.JumpPower, 1, _stackSettings.JumpDuration)
                                .OnComplete(()=>
                                {
                                    pumpkin.parent = null;
                                    _factory.Return(pumpkin);
                                    _score.DecrementScore(ScoreType.Stack);
                                    _score.IncrementScore(ScoreType.Total);
                                });

                            _lastCollect = 0;
                        }
                    }
                }
            }
        }
    }
}