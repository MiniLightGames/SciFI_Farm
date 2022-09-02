using StarterAssets;
using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] InputManager _inputManager; 

    public override void InstallBindings()
    {
        Container.Bind<InputManager>().FromInstance(_inputManager).AsCached();
    }
}