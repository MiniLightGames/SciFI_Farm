using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SeedShop : BaseStructure
{
    [Inject] InputManager _inputManager;
    [Inject] InventoryController _inventory;
    [Inject] DiContainer _container;

    [SerializeField] Transform _instantPoint;
    [SerializeField] SeedShopPanel _seedPanel;
    [SerializeField] Seed—lip _box;
    [SerializeField] GameObject _canvas;
    [SerializeField] Transform _content;
    [SerializeField] FoodController _foodController;

    [Space]
    [SerializeField] List<Seed> _seeds;

    public override void OpenStructure()
    {
        for (int i = 0; i < _seeds.Count; i++)
        {
            if (_seeds[i].Explored)
            {
                SeedShopPanel panel = Instantiate(_seedPanel, _content);
                panel.Init(_seeds[i], i, Take);
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
        if (_seeds[index].Energy <= _foodController.Credits)
        {
            _foodController.RemoveCredits(_seeds[index].Energy);
            GameObject box = _container.InstantiatePrefab(_box, _instantPoint.position, Quaternion.identity, null);
            Seed—lip seedClip = box.GetComponent<Seed—lip>();
            GrabInfo grab = seedClip.SetSeed(_seeds[index]);
            _inventory.AddItem(grab);
        }
    }

    public void AddSeed(Seed seed)
    {
        if(!_seeds.Contains(seed))
        {
            _seeds.Add(seed);
        }
    }
}
