using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SeedConverter : BaseStructure
{
    GrabInfo _currentSeed—lip;
    Plant _currentPlant;

    float _currentTime;

    bool _needConvert;
    bool _structureIsOpen;

    [Inject] InputManager _inputManager;

    [SerializeField] SeedConverterMenu _menu;
    [SerializeField] RectTransform _plants;

    void OnItemDrop(ItemBox item)
    {
        Plant plant = item.Item.GameObject.GetComponent<Plant>();

        if (_currentPlant == null)
        {
            _currentPlant = plant;
            _currentTime = 0;
            _needConvert = true;
        }
        else if(_currentSeed—lip)
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
        if(_needConvert)
        {
            _currentTime += Time.deltaTime;

            _menu.SetArrowFill(_currentTime / _currentPlant.PlantInfo.ConvertTime);

            if(_currentTime >= _currentPlant.PlantInfo.ConvertTime)
            {
                if (!_currentSeed—lip)
                {
                    var _seed—lip = Instantiate(_currentPlant.PlantInfo.SeedPrefab, transform.position, Quaternion.identity, transform);
                    _seed—lip.SetSeed(_currentPlant.PlantInfo);
                    _seed—lip.gameObject.SetActive(false);
                    _currentSeed—lip = _seed—lip.GrabInfo;
                    _currentSeed—lip.SetCount(_currentSeed—lip.Count + 1);
                }
                else
                {
                    _currentSeed—lip.SetCount(_currentSeed—lip.Count + 2);
                }

                _currentTime = 0;
                _currentPlant.GrabInfo.SetCount(_currentPlant.GrabInfo.Count - 1);

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
        _menu.SeedBeginDrag += SeedRelese;
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

        _menu.SeedBeginDrag += SeedRelese;
        _menu.PlantBeginDrag += PlantRelese;

        _menu.gameObject.SetActive(false);

        _inputManager.TrySetupInputState(true);
        _inputManager.SetCursorState(true);

        _structureIsOpen = false;
    }

    void SeedRelese()
    {
        _currentSeed—lip = null;
    }

    void PlantRelese()
    {
        _needConvert = false;
        _currentPlant = null;
        _currentTime = 0;
    }

    void SetCurruntStateMenu()
    {
        if(_currentPlant)
        {
            _menu.SetArrowFill(_currentTime / _currentPlant.PlantInfo.ConvertTime);
        }
        else
        {
            _menu.SetArrowFill(0);
        }

        if(_currentPlant)
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

        if(_currentSeed—lip)
        {
            if (_currentSeed—lip.Count > 0)
            {
                _menu.UpdateSeed(_currentSeed—lip);
            }
            else
            {
                _menu.UpdateSeed(null);
            }
        }
        else
        {
            _menu.UpdateSeed(null);
        }
    }
}
