using Leopotam.EcsLite;
using Zenject;

namespace Game.Inject
{
    public class EcsWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindWorld();
        }

        private void BindWorld()
        {
            var world = new EcsWorld();
            Container.Bind<EcsWorld>()
                .FromInstance(world)
                .AsSingle()
                .NonLazy();
            Container.QueueForInject(world);
        }
    }
}