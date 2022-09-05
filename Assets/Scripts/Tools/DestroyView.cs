using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyView : MonoBehaviour
{
    Renderer[] _renderers;
    Color [] _normalColors;

    [SerializeField] float _minCutOff;
    [SerializeField] float _maxCutOff;
    [SerializeField] Color _destroyColor;

    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _normalColors = new Color[_renderers.Length];
    }

    public void ResetDestroy()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.SetFloat("_CutoffHeight", _maxCutOff);
            _renderers[i].material.color = _normalColors[i];
        }
    }

    public void InitDestoy()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _normalColors[i] = _renderers[i].material.color;
            _renderers[i].material.SetFloat("_CutoffHeight", _maxCutOff);
            _renderers[i].material.color = _destroyColor;
        }
    }

    public bool TryDestroy()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
           float height = _renderers[i].material.GetFloat("_CutoffHeight");
            _renderers[i].material.SetFloat("_CutoffHeight", height - Time.deltaTime);
            if (height <= _minCutOff)
            {
                return true;
            }
        }

        return false;
    }
}
