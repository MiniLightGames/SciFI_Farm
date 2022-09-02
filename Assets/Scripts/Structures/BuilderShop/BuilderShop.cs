using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuilderShop : BaseStructure
{
    [Inject] InputManager _inputManager;
    [Inject] InventoryController _inventory;
    [Inject] DiContainer _container;

    [SerializeField] Transform _instantPoint;
    [SerializeField] StructurePanel _structurePanel;
    [SerializeField] BuilderBox _box;
    [SerializeField] GameObject _canvas;
    [SerializeField] Transform _content;
    [SerializeField] FoodController _foodController;

    [Space]
    [SerializeField] List<Structure> _structures;

    public override void OpenStructure()
    {
        for (int i = 0; i < _structures.Count; i++)
        {
            if(_structures[i].Explored)
            {
                StructurePanel panel = Instantiate(_structurePanel, _content);
                panel.Init(_structures[i], i, Take);
            }
        }
        _canvas.SetActive(true);
        _inputManager.TrySetupInputState(false);
        _inputManager.SetCursorState(false);
    }

    public void Exit()
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
        _canvas.SetActive(false);
        _inputManager.TrySetupInputState(true);
        _inputManager.SetCursorState(true);
    }

    public void Take(int index)
    {
        if(_structures[index].Price <= _foodController.Credits)
        {
            _foodController.RemoveCredits(_structures[index].Price);
            GameObject box = _container.InstantiatePrefab(_box, _instantPoint.position, Quaternion.identity, null);
            BuilderBox builderBox = box.GetComponent<BuilderBox>();
            GrabInfo grab = builderBox.Init(_structures[index]);
            _inventory.AddItem(grab);
        }
    }
}
