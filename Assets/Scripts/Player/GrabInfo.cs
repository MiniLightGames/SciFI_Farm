using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInfo : MonoBehaviour
{
    public Action CountUpdate;
    public Action GrabHandler;

    [SerializeField] int _count;
    [SerializeField] string _name;
    [SerializeField] bool _canGrab;
    [SerializeField] Sprite _inventorySprite;

    public string ObjectName => _name;
    public bool CanGrab => _canGrab;
    public Sprite InventorySprite => _inventorySprite;
    public GameObject GameObject => gameObject;
    public int Count => _count;

    public void Init(string name, bool canGrab, Sprite inventorySprite)
    {
        _count = 1;
        _name = name;
        _canGrab = canGrab;
        _inventorySprite = inventorySprite;
    }

    public void SetCount(int count)
    {
        _count = count;
        CountUpdate?.Invoke();
    }

    public void Grab()
    {
        GrabHandler?.Invoke();
    }
}
