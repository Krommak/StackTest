using Leopotam.EcsLite;
using Zenject;

namespace Game.Inject
{
    public class EcsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var world = BindWorld();
            BindSystems(world);
        }

        private EcsWorld BindWorld()
        {
            var world = new EcsWorld();
            Container.Bind<EcsWorld>()
                .FromInstance(world)
                .AsSingle()
                .NonLazy();
            Container.QueueForInject(world);

            return world;
        }

        private void BindSystems(EcsWorld world)
        {
            var systems = new EcsSystems(world);

            systems
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();

            Container.Bind<IEcsSystems>()
                .To<EcsSystems>()
                .FromInstance(systems)
                .AsSingle()
                .NonLazy();
        }
    }
}