using DG.Tweening;
using Game.Ecs.Components;
using Game.Inject.Installers;
using Game.MonoBehaviours.UI;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game.Ecs.Systems
{
    sealed class StackSystem : IEcsRunSystem
    {
        private StackSettings _stackSettings;
        private IScoreService _score;

        private EcsWorld _world;
        private EcsFilter _moveToStackFilter;
        private EcsFilter _playerFilter;
        private EcsPool<MoveToStack> _moveToStackPool;
        private EcsPool<Stack> _stackPool;

        [Inject]
        public StackSystem(EcsWorld world, StackSettings stackSettings, IScoreService score)
        {
            _world = world;
            _stackSettings = stackSettings;
            _score = score;

            _playerFilter = _world.Filter<PlayerComponent>().Inc<Stack>().End();
            _moveToStackFilter = _world.Filter<MoveToStack>().End();

            _moveToStackPool = _world.GetPool<MoveToStack>();
            _stackPool = _world.GetPool<Stack>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var item in _moveToStackFilter)
            {
                foreach (var player in _playerFilter)
                {
                    ref var stack = ref _stackPool.Get(player);
                    if(stack.Value.Count == _stackSettings.StackSize)
                    {
                        continue;
                    }
                    else
                    {
                        var moveToStack = _moveToStackPool.Get(item);
                        AddToStack(ref stack, moveToStack.Value);
                        moveToStack.OnApplyAction.Invoke();
                    }
                }
                _moveToStackPool.Del(item);
            }
        }

        private void AddToStack(ref Stack stack, Transform pumpkin)
        {
            stack.Value.Push(pumpkin);
            pumpkin.parent = stack.StackTransform;
            pumpkin.localScale = _stackSettings.StackElementSize;
            var elementPosition =
                new Vector3(0, (_stackSettings.StackElementSize.y + _stackSettings.StackElementMargin) * stack.Value.Count, 0);

            pumpkin.DOLocalJump(elementPosition, _stackSettings.JumpPower, 1, _stackSettings.JumpDuration);
            pumpkin.DOLocalRotate(Vector3.zero, _stackSettings.JumpDuration);

            _score.IncrementScore(ScoreType.Stack);
        }
    }
}