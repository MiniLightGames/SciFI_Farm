using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructurePanel : MonoBehaviour
{
    public delegate void TakeHandler(int index);

    TakeHandler _takeAction;

    int _index;

    [SerializeField] Button _takeButton;
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _coust;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;

    public void Init(Structure structure, int index, TakeHandler callback)
    {
        _image.sprite = structure.StructureSprite;
        _coust.text = $"{structure.Price}";
        _name.text = structure.Name;
        _description.text = structure.StructureDescription;
        _index = index;
        _takeAction = callback;
        _takeButton.onClick.AddListener(Take);
    }

    void Take()
    {
        _takeAction(_index);
    }
}
