using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchGunMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI _description;

    public void Show(Seed info)
    {
        Name.text = info.Name.ToString();
        _description.text = info.PlantDescription;
        gameObject.SetActive(true);
    }
}
