using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StructureFood : BaseStructure
{
    Plant _currentPlant;

    float _currentTime;

    bool _needConvert;
    bool _structureIsOpen;

    [Inject] InputManager _inputManager;

    [SerializeField] FoodController _controller;
    [SerializeField] StructureFoodMenu _menu;

    void OnItemDrop(ItemBox item)
    {
        Plant plant = item.Item.GameObject.GetComponent<Plant>();

        if (_currentPlant == null)
        {
            _currentPlant = plant;
            _currentTime = 0;
            _needConvert = true;
        }
        else
        {
            _currentPlant = plant;
            _currentTime = 0;
            _needConvert = false;
        }

        _menu.UpdatePlant(_currentPlant.GrabInfo);

        item.Item.GameObject.transform.SetParent(transform);
        item.Item.GameObject.transform.localPosition = Vector3.zero;
        item.Item.GameObject.SetActive(false);
    }

    public void Update()
    {
        if (_needConvert)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= 0.2f)
            {
                _currentTime = 0;
                _currentPlant.GrabInfo.SetCount(_currentPlant.GrabInfo.Count - 1);
                _controller.FoodAdd(_currentPlant.PlantInfo.Energy);

                if (_currentPlant.GrabInfo.Count <= 0)
                {
                    _needConvert = false;
                    _currentPlant = null;
                }

                if (_structureIsOpen)
                {
                    SetCurruntStateMenu();
                }
            }
        }
    }

    public override void OpenStructure()
    {
        _menu.ItemIsDrop += OnItemDrop;
        _menu.PlantBeginDrag += PlantRelese;

        _menu.gameObject.SetActive(true);

        _inputManager.TrySetupInputState(false);
        _inputManager.SetCursorState(false);

        SetCurruntStateMenu();

        _structureIsOpen = true;
    }

    public void MenuExit()
    {
        _menu.ItemIsDrop -= OnItemDrop;

        _menu.PlantBeginDrag += PlantRelese;

        _menu.gameObject.SetActive(false);

        _inputManager.TrySetupInputState(true);
        _inputManager.SetCursorState(true);

        _structureIsOpen = false;
    }

    void PlantRelese()
    {
        _needConvert = false;
        _currentPlant = null;
        _currentTime = 0;
    }

    void SetCurruntStateMenu()
    {
        if (_currentPlant)
        {
            if (_currentPlant.GrabInfo.Count > 0)
            {
                _menu.UpdatePlant(_currentPlant.GrabInfo);
            }
        }
        else
        {
            _menu.UpdatePlant(null);
        }
    }
}
