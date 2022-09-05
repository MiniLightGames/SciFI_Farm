using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class StructureView : MonoBehaviour
{
    Renderer[] _renderers;
    Collider[] _colliders;
    Transform _transform;
    List<Material> _normalMaterial;
    Rigidbody _rigidbody;
    BoxCollider _boxCollider;

    bool _canBuild;
    bool _build;
    float _buildProgress;

    [SerializeField] float _startCutoff;
    [SerializeField] float _maxCutoff;
    [SerializeField] Material _viewMaterial;

    [SerializeField] Color _cantBuildColor;
    [SerializeField] Color _buildColor;

    public Transform Transform => _transform;
    public bool CanBuild => _canBuild;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody.isKinematic = true;
        _boxCollider.isTrigger = true;
        _canBuild = true;
    }

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
            if (!show)
            {
                _canBuild = true;
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].material.color = _viewMaterial.color;
                }
            }
        }
    }

    public void InitView()
    {
        _normalMaterial = new List<Material>();
        _transform = GetComponent<Transform>();
        _renderers = GetComponentsInChildren<Renderer>(true);
        _colliders = GetComponentsInChildren<Collider>(true);
        gameObject.layer = 0;

        for (int i = 0; i < _renderers.Length; i++)
        {
            _normalMaterial.Add(_renderers[i].material);
            _renderers[i].material = _viewMaterial;
        }

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }

        _boxCollider.enabled = true;
    }

    public void StartBuild()
    {
        _boxCollider.enabled = false;
        gameObject.layer = 14;

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
            _boxCollider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(_boxCollider.enabled)
        {
            if (other.gameObject.layer != 9)
            {
                _canBuild = false;
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].material.color = _cantBuildColor;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(_boxCollider.enabled)
        {
            if (other.gameObject.layer != 9)
            {
                _canBuild = false;
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].material.color = _cantBuildColor;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_boxCollider.enabled)
        {
            if (other.gameObject.layer != 9)
            {
                _canBuild = true;
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].material.color = _buildColor;
                }
            }
        }
    }
}
