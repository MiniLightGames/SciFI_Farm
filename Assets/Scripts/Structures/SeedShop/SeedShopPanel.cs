using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedShopPanel : MonoBehaviour
{
    public delegate void TakeHandler(int index);

    TakeHandler _takeAction;

    int _index;

    [SerializeField] Button _takeButton;
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _coust;
    [SerializeField] TextMeshProUGUI _description;

    public void Init(Seed seed, int index, TakeHandler callback)
    {
        _image.sprite = seed.SeedSprite;
        _coust.text = $"{seed.Energy}";
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
