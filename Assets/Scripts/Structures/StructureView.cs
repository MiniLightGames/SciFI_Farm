using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureView : MonoBehaviour
{
    Renderer[] _renderers;
    Collider[] _colliders;
    Transform _transform;
    List<Material> _normalMaterial;

    bool _build;
    float _buildProgress;

    [SerializeField] float _startCutoff;
    [SerializeField] float _maxCutoff;
    [SerializeField] Material _viewMaterial;

    public Transform Transform => _transform;

    void Update()
    {
        if (_build)
        {
            Building();
        }
    }

    public void Show(bool show)
    { 
        if(gameObject.activeSelf != show)
        {
            gameObject.SetActive(show);
        }
    }

    public void InitView()
    {
        _normalMaterial = new List<Material>();
        _transform = GetComponent<Transform>();
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _normalMaterial.Add(_renderers[i].material);
            _renderers[i].material = _viewMaterial;
        }

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }
    }

    public void StartBuild()
    {
        SetNormalView();
        _buildProgress = _startCutoff;
        _build = true;
    }

    void SetNormalView()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = _normalMaterial[i];
            _renderers[i].material.SetFloat("_CutoffHeight", _startCutoff);
        }
    }

    void Building()
    {
        _buildProgress += Time.deltaTime;

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.SetFloat("_CutoffHeight", _buildProgress);
        }

        if(_buildProgress >= _maxCutoff)
        {
            _build = false;
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = true;
            }
        }
    }
}
