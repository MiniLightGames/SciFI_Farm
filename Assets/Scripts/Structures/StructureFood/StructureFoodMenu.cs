using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class StructureFoodMenu : MonoBehaviour
{
    public Action<ItemBox> ItemIsDrop;

    public Action PlantBeginDrag;
    public Action SeedBeginDrag;

    [SerializeField] DropArea _plantBox;
    [SerializeField] ItemBox _plant;

    [SerializeField] FoodController _controller;

    [SerializeField] TextMeshProUGUI _foodCount;
    [SerializeField] TextMeshProUGUI _peapleCount;
    [SerializeField] TextMeshProUGUI _eat;
    void OnEnable()
    {
        _plant.DropHandler += OnDrop;
        _plant.BeginDrag += PlantDrag;
        _plantBox.AddFilter(typeof(Plant));
    }

    public void UpdatePlant(GrabInfo info)
    {
        if (_foodCount) { _foodCount.text = $"Хранилище: {_controller.CurrentFood}"; }
        if (_peapleCount) { _peapleCount.text = $"Население: {_controller.ColonyCount}"; }
        if (_eat) { _eat.text = $"Потребление: 1/{_controller.EeatTime} cек"; }
        
        if (info)
        {
            if (_plant.Item)
            {
                _plant.UpdateItem();
            }
            else
            {
                _plant.AddNewItem(info);
            }
        }
        else
        {
            _plant.Release();
        }
    }

    public void OnDrop(ItemBox itemBox)
    {
        ItemIsDrop?.Invoke(itemBox);
    }

    void PlantDrag()
    {
        PlantBeginDrag?.Invoke();
    }

    void OnDisable()
    {
        _plant.DropHandler -= OnDrop;
        _plant.BeginDrag -= PlantDrag;
        _plantBox.ResetFilter();
    }
}
