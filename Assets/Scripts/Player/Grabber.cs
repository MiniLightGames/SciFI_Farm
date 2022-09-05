using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Grabber : MonoBehaviour
{
    RaycastHit _hit;

    [Inject] InputManager _input;
    [Inject] InventoryController _inventory;
    [Inject(Id = TransformId.GrabMenu)] Transform _grabMenu;

    Transform _transform;
    GameObject _currentItem;
    GrabInfo _currentInfo;

    [SerializeField] float _radius;
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _mask;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _grabMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Physics.SphereCast(_transform.position, _radius, _transform.forward, out _hit, _maxDistance))
        {
            if (_hit.collider.gameObject.layer == 12 || _hit.collider.gameObject.layer == 13)
            {
                GrabItems();
            }
            else if (_hit.collider.gameObject.layer == 14)
            {
                StructureShow();
            }
            else
            {
                SetMenuActive(false);
            }
        }
        else
        {
            SetMenuActive(false);
        }
    }

    void GrabItems()
    {
        if (_currentItem != _hit.collider.gameObject)
        {
            _currentItem = _hit.collider.gameObject;
            _currentInfo = _hit.collider.GetComponent<GrabInfo>();
        }

        if (_currentInfo)
        {
            if (_currentInfo.CanGrab)
            {
                ShowGrab();
                Grab(_currentInfo);
            }
        }
    }

    void StructureShow()
    {
        ShowGrab();
        TryTakStructure();
    }

    void TryTakStructure()
    {
        if (_input.Grab)
        {
            BaseStructure structure = _hit.collider.GetComponent<BaseStructure>();
            structure.OpenStructure();
        }
    }

    void ShowGrab()
    {
        Transform trans = _hit.collider.transform;
        Vector3 pos = trans.position;

        pos.y += trans.localScale.y / 4;
        float dist = (_hit.point - pos).magnitude + 0.25f;

        _grabMenu.position = Vector3.MoveTowards(pos, _transform.position, dist);
        _grabMenu.LookAt(_transform.position);
        SetMenuActive(true);
    }

    void Grab(GrabInfo item)
    {
        if (_input.Grab)
        {
            item.Grab();
            _inventory.AddItem(item);
        }
    }

    void SetMenuActive(bool state)
    {
        if(_grabMenu.gameObject.activeSelf != state)
        {
            _grabMenu.gameObject.SetActive(state);
        }
    }
}
