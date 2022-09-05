using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GrabInfo))]
public class Plant : MonoBehaviour, IGrab
{
    public Action OnPlantGrown;
    public Action OnPlantPluck;
    [SerializeField] Seed _seed;

    GrabInfo _grabInfo;
    Transform _transform;
    Vector3 _pluckSize;
    Vector3 _minSize;
    float _currentGrowTime;
    bool _isGrow;

    public Seed PlantInfo => _seed;
    public GrabInfo GrabInfo => _grabInfo;

    public bool CanGrab { get => _seed.Explored ? !_isGrow : false; }

    public void Awake()
    {
        _grabInfo = GetComponent<GrabInfo>();
        _grabInfo.GrabHandler += PlantPluck;
        _grabInfo.Init($"{_seed.Name}Plant", this, _seed.PlantSprite);
    }

    public void OnUpdate()
    {
        if(_isGrow)
        {
            _currentGrowTime += Time.deltaTime;
            Vector3 currentSize = Vector3.Lerp(_minSize, _pluckSize, _currentGrowTime / _seed.GrowthTime);
            _transform.localScale = currentSize;

            if (_currentGrowTime >= _seed.GrowthTime)
            {
                PlantGrown();
            }
        }
    } 

    public void InitPlant()
    {
        _transform = GetComponent<Transform>();

        _isGrow = true;
        _currentGrowTime = 0;

        _pluckSize = _transform.localScale;
        _minSize = _transform.localScale / 4;
        _transform.localScale = _minSize;

        _grabInfo.Init($"{_seed.Name}Plant", this, _seed.PlantSprite);
    }

    void PlantGrown()
    {
        _isGrow = false;
        _grabInfo.Init($"{_seed.Name}Plant", this, _seed.PlantSprite);
        OnPlantGrown?.Invoke();
    }

    void PlantPluck()
    {
        OnPlantPluck?.Invoke();
    }

    void OnDestroy()
    {
        _grabInfo.GrabHandler -= PlantPluck;
    }
}
