using Game.Ecs.Systems;
using Leopotam.EcsLite;
using Zenject;

public class EcsSystemsInstaller : MonoInstaller
{
    private EcsWorld _world;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    public override void InstallBindings()
    {
        BindSystems();
    }

    private void BindSystems()
    {
        var systems = new EcsSystems(_world);

        systems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Add(Container.Instantiate<MovementSystem>())
            .Add(Container.Instantiate<CameraMovementSystem>())
            .Add(Container.Instantiate<PumpkinSpawnSystem>())
            .Add(Container.Instantiate<AnimationSystem>())
            .Add(Container.Instantiate<StackSystem>())
            .Add(Container.Instantiate<CollectStackSystem>())
            .Init();

        Container.Bind<IEcsSystems>()
            .To<EcsSystems>()
            .FromInstance(systems)
            .AsSingle()
            .NonLazy();
    }
}