using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] InventoryController _inventoryController;
    [SerializeField] Canvas _inventoryCanvas;

    public override void InstallBindings()
    {
        Container.Bind<InventoryController>().FromInstance(_inventoryController).AsCached();
        Container.Bind<Canvas>().WithId("InventoryCanvas").FromInstance(_inventoryCanvas).AsCached();
    }
}