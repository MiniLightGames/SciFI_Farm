using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaterTrigger))]
[RequireComponent(typeof(SeedTrigger))]
public class SeedBedController : MonoBehaviour
{
    [SerializeField] SeedBedType _type;
    [SerializeField] Color _dryColor;
    [SerializeField] Color _wetColor;

    Plant _plant;
    Renderer _renderer;
    WaterTrigger _waterTrigger;
    SeedTrigger _seedTrigger;
    Transform _transform;

    bool _isWet;
    bool _isPlantPlace;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _waterTrigger = GetComponent<WaterTrigger>();
        _renderer = GetComponent<Renderer>();
        _seedTrigger = GetComponent<SeedTrigger>();

        _renderer.material.color = _dryColor;
        _waterTrigger.OnWater += Watering;
        _seedTrigger.OnSeed += Planted;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWet && _isPlantPlace)
        {
            _plant.OnUpdate();
        }
    }

    void Watering()
    {
        _isWet = true;
        _renderer.material.color = _wetColor;
    }

    void Planted(SeedBullet bullet)
    {
        if(bullet.SeedParameters.SeedBedType == _type && !_isPlantPlace)
        {
            _isPlantPlace = true;
            Vector3 position = _transform.position;
            position.y += _transform.localScale.y / 2;
            _plant = Instantiate(bullet.SeedParameters.PlantPrefab, position, _transform.rotation, _transform);
            
            _plant.InitPlant();
            _plant.OnPlantGrown += PlantGrown;
            _plant.OnPlantPluck += PlantPluck;
        }
    }

    void PlantGrown()
    {
        _isWet = false;
        _renderer.material.color = _dryColor;
    }

    void PlantPluck()
    {
        _isPlantPlace = false;

        _plant.OnPlantGrown -= PlantGrown;
        _plant.OnPlantPluck -= PlantPluck;
    }

    void OnDestroy()
    {
        _waterTrigger.OnWater -= Watering;
        _seedTrigger.OnSeed -= Planted;

        if(_isPlantPlace)
        {
            _plant.OnPlantGrown -= PlantGrown;
            _plant.OnPlantPluck -= PlantPluck;
        }
    }
}
