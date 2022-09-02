using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SeedMenu : MonoBehaviour
{
    public Action<Seed—lip> Seed—lipTake;

    [Inject] InventoryController _inventory;
    [Inject] InputManager _inputManager;

    List<Seed—lip> _seedClips = new List<Seed—lip>();

    [SerializeField] SeedPanel _seedPanelPrefab;
    [SerializeField] Transform _content;

    void OnEnable()
    {
        _inputManager.SetCursorState(false);
        _seedClips = _inventory.Find<Seed—lip>();
       for(int i = 0; i < _seedClips.Count; i++)
       {
            SeedPanel panel = Instantiate(_seedPanelPrefab, _content);
            panel.Init(_seedClips[i].SeedParametrs, _seedClips[i].Count, i, Take);
       }
    }

    public void Exit()
    {
        _seedClips.Clear();
        for (int i = 0; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);
        _inputManager.SetCursorState(true);
    }

    public void Take(int index)
    {
        Seed—lipTake?.Invoke(_seedClips[index]);
        Exit();
    }
}
