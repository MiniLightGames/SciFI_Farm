using UnityEngine;
using Zenject;

public class TransformInstaller : MonoInstaller
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _hand;
    [SerializeField] Transform _seedBeds;
    [SerializeField] Transform _grab;

    public override void InstallBindings()
    {
        Container.Bind<Transform>().WithId(TransformId.Player).FromInstance(_player).AsCached();
        Container.Bind<Transform>().WithId(TransformId.Hand).FromInstance(_hand).AsCached();
        Container.Bind<Transform>().WithId(TransformId.SeedBeds).FromInstance(_seedBeds).AsCached();
        Container.Bind<Transform>().WithId(TransformId.GrabMenu).FromInstance(_grab).AsCached();
    }
}