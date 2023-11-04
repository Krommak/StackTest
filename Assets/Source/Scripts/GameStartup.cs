using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Game
{
    sealed class GameStartup : MonoBehaviour
    {
        private EcsWorld _world;

        private IEcsSystems _systems;

        [Inject]
        private void Construct(EcsWorld world, IEcsSystems systems)
        {
            _world = world;
            _systems = systems;
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}