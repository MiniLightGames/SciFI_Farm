using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class SeedConverterMenu : MonoBehaviour
{
    public Action<ItemBox> ItemIsDrop;
    public Action<ItemBox> SeedIsDrop;

    public Action PlantBeginDrag;
    public Action SeedBeginDrag;

    [SerializeField] DropArea _plantBox;
    [SerializeField] Image _arrow;
    [SerializeField] ItemBox _plant;
    [SerializeField] ItemBox _seed;

    void OnEnable()
    {
        _plant.DropHandler += OnDrop;
        _seed.DropHandler += OnSeedDrop;

        _plant.BeginDrag += PlantDrag;
        _seed.BeginDrag += SeedDrag;

        _plantBox.AddFilter(typeof(Plant));
    }

    public void UpdatePlant(GrabInfo info)
    {
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

    public void UpdateSeed(GrabInfo info)
    {
        if(info)
        {
            if (_seed.Item)
            {
                _seed.UpdateItem();
            }
            else
            {
                _seed.AddNewItem(info);
            }
        }
        else
        {
            _seed.Release();
        }
    }

    public void SetArrowFill(float fill)
    {
        _arrow.fillAmount = fill;
    }

    public void OnDrop(ItemBox itemBox)
    {
        ItemIsDrop?.Invoke(itemBox);
    }

    public void OnSeedDrop(ItemBox itemBox)
    {
        SeedIsDrop?.Invoke(itemBox);
    }

    void SeedDrag()
    {
        SeedBeginDrag?.Invoke();
    }

    void PlantDrag()
    {
        PlantBeginDrag?.Invoke();
    }

    void OnDisable()
    {
        _plant.DropHandler -= OnDrop;
        _seed.DropHandler -= OnSeedDrop;

        _plant.BeginDrag -= PlantDrag;
        _seed.BeginDrag -= SeedDrag;

        _plantBox.ResetFilter();
    }
}
