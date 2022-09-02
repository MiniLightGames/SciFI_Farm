using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SeedPanel : MonoBehaviour
{
    public delegate void TakeHandler(int index);

    TakeHandler _takeAction;

    int _index;

    [SerializeField] Button _takeButton;
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _count;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;

    public void Init(Seed seed, int count, int index, TakeHandler callback)
    {
        _image.sprite = seed.SeedSprite;
        _count.text = $"x{count}";
        _name.text = seed.Name.ToString();
        _description.text = seed.SeedDescription;
        _index = index;
        _takeAction = callback;
        _takeButton.onClick.AddListener(Take);
    }

    void Take()
    {
        _takeAction(_index);
    }
}
