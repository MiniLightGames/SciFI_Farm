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
    [SerializeField] IGrab _canGrab;
    [SerializeField] Sprite _inventorySprite;

    Rigidbody _rig;
    Collider _collider;

    public string ObjectName => _name;
    public bool CanGrab => _canGrab.CanGrab;
    public Sprite InventorySprite => _inventorySprite;
    public GameObject GameObject => gameObject;
    public int Count => _count;

    public void Init(string name, IGrab canGrab, Sprite inventorySprite)
    {
        _count = 1;
        _name = name;
        _canGrab = canGrab;
        _inventorySprite = inventorySprite;
        _rig = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        if (_rig)
        {
            _rig.useGravity = false;
        }
    }

    public void SetCount(int count)
    {
        _count = count;
        CountUpdate?.Invoke();
    }

    public void Release()
    {
        gameObject.SetActive(true);
        if(_collider)
        {
            _collider.enabled = true;
            _collider.isTrigger = false;
        }

        if (_rig)
        {
            _rig.useGravity = true;
            _rig.transform.SetParent(null);
            _rig.AddForce(_rig.transform.forward * 3f, ForceMode.Impulse);
        }
    }

    public void ColliderDisable()
    {
        _collider.enabled = false;
    }

    public void Grab()
    {
        _rig.useGravity = false;
        _collider.enabled = false;

        GrabHandler?.Invoke();
    }
}
