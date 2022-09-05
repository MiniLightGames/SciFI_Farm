using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryController : MonoBehaviour
{
    [Inject] InputManager _input;
    [Inject(Id = TransformId.Hand)] Transform _hand;

    [SerializeField] GrabInfo _seedGun;
    [SerializeField] GrabInfo _hoeGun;
    [SerializeField] GrabInfo _waterGun;

    List<ItemBox> _items = new List<ItemBox>();

    int _currentIndex;

    void Start()
    {
        GetComponentsInChildren<ItemBox>(true, _items);
        _input.InvenotyKeyPress += OnInvenotyKeyPress;

        _items[0].AddNewItem(_seedGun);
        _items[1].AddNewItem(_hoeGun);
        _items[2].AddNewItem(_waterGun);

        for(int i = 0; i < _items.Count; i++)
        {
            _items[i].DropHandler += ItemDrop;
        }
    }

    public bool AddItem(GrabInfo item)
    {
        ItemBox itembox = _items.Find(t => t.TryAdd(item));

        if(!itembox)
        {
            for (int i = 3; i < _items.Count; i++)
            {
                if(_items[i].IsEmpty)
                {
                    item.transform.SetParent(_hand);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;
                    _items[i].AddNewItem(item);
                    return true;
                }
            }
        }
        else
        {
            Destroy(item.gameObject);
            return true;
        }

        return false;
    }

    void OnInvenotyKeyPress(int number)
    {
        _items[_currentIndex].Disable();
        _currentIndex = number-1;
        _items[_currentIndex].Enable();
    }

    void ItemDrop(ItemBox item)
    {
        item.Item.GameObject.transform.SetParent(_hand);
        item.Item.GameObject.transform.localPosition = Vector3.zero;
    }

    public List<T> Find<T>()
    {
        List<T> items = new List<T>();
        for (int i = 3; i < _items.Count; i++)
        {
            if (!_items[i].IsEmpty)
            {
               var component = _items[i].Item.GameObject.GetComponent<T>();
               if(component != null)
               {
                    items.Add(component);
               }
            }
        }
        return items;
    }

    void OnDestroy()
    {
        _input.InvenotyKeyPress += OnInvenotyKeyPress;

        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].DropHandler -= ItemDrop;
        }
    }
}
