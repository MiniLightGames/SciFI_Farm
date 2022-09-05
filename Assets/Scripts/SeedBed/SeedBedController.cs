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
    [SerializeField] Color _selectColor;
    [SerializeField] ParticleSystem _particle;
    [SerializeField] ParticleSystem _seedParticle;

    Plant _plant;
    Renderer _renderer;
    WaterTrigger _waterTrigger;
    Transform _transform;
    Color _currentColor;

    bool _isWet;
    bool _isPlantPlace;


    void Awake()
    {
        _transform = GetComponent<Transform>();
        _waterTrigger = GetComponent<WaterTrigger>();
        _renderer = GetComponent<Renderer>();

        _renderer.material.color = _dryColor;
        _currentColor = _dryColor;
        _waterTrigger.OnWater += Watering;
    }

    void OnEnable()
    {
        if(_particle != null)
        {
            _particle?.Play();
        }
    }

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
        _currentColor = _wetColor;
    }

    public void Planted(SeedBullet bullet)
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
        else
        {
            _seedParticle.Play();
        }
    }

    public void Select()
    {
        _renderer.material.color = _selectColor;
    }

    public void DeSelect()
    {
        _renderer.material.color = _currentColor;
    }

    void PlantGrown()
    {
        _isWet = false;
        _renderer.material.color = _dryColor;
        _currentColor = _dryColor;
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

        if(_isPlantPlace)
        {
            _plant.OnPlantGrown -= PlantGrown;
            _plant.OnPlantPluck -= PlantPluck;
        }
    }
}
