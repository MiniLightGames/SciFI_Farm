using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedClipIcon : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _countText;

    public void Init(Sprite sprite, int count)
    {
        if(count > 0)
        {
            _image.sprite = sprite;
            _countText.text = $"{count}x";
        }
    }

    public void UpdateCount(int count)
    {
        if (count > 0)
        {
            _countText.text = $"{count}x";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
