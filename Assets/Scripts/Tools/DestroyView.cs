using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyView : MonoBehaviour
{
    Renderer[] _renderers;
    Color [] _normalColors;
    float [] _cuttoff;

    [SerializeField] Color _destroyColor;

    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _normalColors = new Color[_renderers.Length];
        _cuttoff = new float[_renderers.Length];
    }

    public void ResetDestroy()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.SetFloat("_CutoffHeight", _cuttoff[i]);
            _renderers[i].material.color = _normalColors[i];
        }
    }

    public void InitDestoy()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _normalColors[i] = _renderers[i].material.color;
            _cuttoff[i] = _renderers[i].material.GetFloat("_CutoffHeight");
            _renderers[i].material.color = _destroyColor;
        }
    }

    public bool TryDestroy()
    {
        float allHeight = 0;
        for (int i = 0; i < _renderers.Length; i++)
        {
           float height = _renderers[i].material.GetFloat("_CutoffHeight");
            _renderers[i].material.SetFloat("_CutoffHeight", height - Time.deltaTime);
            if(height > 0)
            {
                allHeight += height;
            }
        }

        if(allHeight <= 0)
        {
            return true;
        }

        return false;
    }
}
