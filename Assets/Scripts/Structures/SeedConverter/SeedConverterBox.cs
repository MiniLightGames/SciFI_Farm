using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedConverterBox : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    [SerializeField] TextMeshProUGUI _countText;

    public void SetImage(Sprite sprite)
    {
        if(sprite)
        {
            _itemImage.sprite = sprite;
            _itemImage.enabled = true;
        }
        else
        {
            _itemImage.enabled = false;
        }
    }

    public void SetCount(int count)
    {
        if(count > 0)
        {
            _countText.text = $"x{count}";
            _countText.enabled = true;
        }
        else
        {
            _countText.enabled = false;
        }
    }
}
