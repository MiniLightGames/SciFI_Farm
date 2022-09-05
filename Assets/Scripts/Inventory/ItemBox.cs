using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Zenject;

[RequireComponent (typeof(DropArea))]
public class ItemBox : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action BeginDrag;
    public Action <ItemBox> DropHandler;
    public Action ReleaseHandler;

    [Inject(Id = "InventoryCanvas")] Canvas _canvas;

    bool _isEmpty;
    bool _selected;

    DropArea _dropArea;
    GrabInfo _item;
    Image _backGround;
    RectTransform _imageRectTransform;
   
    [SerializeField] Image _itemImage;
    [SerializeField] TextMeshProUGUI _countText;
    [SerializeField] Sprite _enableSprite;
    [SerializeField] Sprite _disableSprite;

    public bool IsEmpty => _isEmpty;
    public int Count => _item.Count;
    public GrabInfo Item => _item;

    public void Awake()
    {
        _isEmpty = true;
        _backGround = GetComponent<Image>();
        _dropArea = GetComponent<DropArea>();
        _imageRectTransform = _itemImage.GetComponent<RectTransform>();   
        _countText.gameObject.SetActive(false);
        _dropArea.SetItemBox(this);
    }

    void OnEnable()
    {
        _dropArea.OnDropHandler += Drop;
    }

    public void Disable()
    {
        if (!_isEmpty)
        {
            _item.GameObject.SetActive(false);
            _backGround.sprite = _disableSprite;
        }
    }

    public void Enable()
    {
        if(!_isEmpty)
        {
            _item.ColliderDisable();
            _item.GameObject.SetActive(true);
            _backGround.sprite = _enableSprite;
        }
    }

    public void AddNewItem(GrabInfo item)
    {
        if(_isEmpty)
        {
            _isEmpty = false;
            _item = item;
            _item.GameObject.SetActive(false);
            _itemImage.enabled = true;
            _itemImage.sprite = item.InventorySprite;
            _item.CountUpdate += UpdateItem;
            if (item.Count > 0)
            {
                _countText.text = $"x{_item.Count}";
                _countText.gameObject.SetActive(true);
            }
        }
    }

    public void AddNewItem(GrabInfo item, int count)
    {
        if (_isEmpty)
        {
            _isEmpty = false;
            _item = item;
            _item.GameObject.SetActive(false);
            _itemImage.enabled = true;
            _itemImage.sprite = item.InventorySprite;
            _item.SetCount(count);
            _item.CountUpdate += UpdateItem;
            if (item.Count > 0)
            {
                _countText.text = $"x{_item.Count}";
                _countText.gameObject.SetActive(true);
            }
        }
    }

    public bool TryAdd(GrabInfo item)
    {
        if (!_isEmpty)
        {
            if(_item.ObjectName == item.ObjectName)
            {
                _item.SetCount(_item.Count + item.Count);
                if(_item.Count > 0)
                {
                    _countText.text = $"x{_item.Count}";
                    _countText.gameObject.SetActive(true);
                }
                return true;
            }
        }

        return false;
    }

    public bool TryAdd(GrabInfo item, int count)
    {
        if (!_isEmpty)
        {
            if (_item.ObjectName == item.ObjectName)
            {
                _item.SetCount(_item.Count + count);
                if (_item.Count > 0)
                {
                    _countText.text = $"x{_item.Count}";
                    _countText.gameObject.SetActive(true);
                }
                return true;
            }
        }

        return false;
    }

    public void UpdateItem()
    {
        if (!_isEmpty)
        {
            if (_item.Count > 0)
            {
                _countText.text = $"x{_item.Count}";
                _countText.gameObject.SetActive(true);
            }
            else if(_item.Count <= 0)
            {
                Release();
            }
        }
    }

    public void Release(bool needDestroyItem = false)
    {
        if (!_isEmpty)
        {
            _item.CountUpdate -= UpdateItem;

            _isEmpty = true;
            _item = null;
            _itemImage.enabled = false;

            _countText.text = "";
            _countText.gameObject.SetActive(false);

            ReleaseHandler?.Invoke();
        }
    }

    public void Drop(ItemBox item)
    {
        if(_isEmpty)
        {
            _isEmpty = false;
            _item = item.Item;

            _item.GameObject.SetActive(false);
            _itemImage.enabled = true;
            _itemImage.sprite = _item.InventorySprite;
            _item.CountUpdate += UpdateItem;
        }
        else
        {
            if (_item.ObjectName == item.Item.ObjectName)
            {
                _item.SetCount(_item.Count + item.Count);
            }
            else
            {
                _isEmpty = false;
                _item = item.Item;

                _item.GameObject.SetActive(false);
                _itemImage.enabled = true;
                _itemImage.sprite = _item.InventorySprite;
                _item.CountUpdate += UpdateItem;
            }
        }

        if (_item.Count > 0)
        {
            _countText.text = $"x{_item.Count}";
            _countText.gameObject.SetActive(true);
        }

        DropHandler?.Invoke(item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _imageRectTransform.localScale *= 2;
        _imageRectTransform.SetParent(_canvas.transform);
        BeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _imageRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        DropArea dropArea = null;
        results.Remove(results.Find(x => x.gameObject == _itemImage.gameObject));

        foreach (var result in results)
        {
            dropArea = result.gameObject.GetComponent<DropArea>();

            if (dropArea != null)
            {
                if (dropArea.TryAccept(_item.GameObject))
                {
                    GrabInfo info = null;
                    ItemBox box = dropArea.NeedReplace();
                    if(box)
                    {
                        if (box.Item.ObjectName != Item.ObjectName)
                        {
                            info = box.Item;
                        }
                    }

                    dropArea.Drop(this);
                    Release();

                    if(info)
                    {
                        AddNewItem(info);
                    }

                    break;
                }
            }
        }

        _imageRectTransform.localScale /= 2;
        _imageRectTransform.SetParent(transform);
        _imageRectTransform.anchoredPosition = Vector2.zero;

        if (results.Count <= 0)
        {
            _item.Release();
            Release();
        }
    }

    void OnDisable()
    {
        _dropArea.OnDropHandler -= Drop;
    }
}
