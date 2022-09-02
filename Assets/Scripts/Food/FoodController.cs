using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodController : MonoBehaviour
{
    int _currentFood;
    int _colonyCount;
    float _currentTime;
    float _foodTime;
    float _currentFoodTime;
    int _currentDay;
    int _credits;

    [SerializeField] float _dayTime;
    [SerializeField] TextMeshProUGUI _foodText;
    [SerializeField] TextMeshProUGUI _peapleCount;
    [SerializeField] TextMeshProUGUI _creditsCount;

    public int CurrentFood => _currentFood;
    public int ColonyCount => _colonyCount;
    public float EeatTime => _dayTime / (_colonyCount * 5);
    public float Credits => _credits;

    void Awake()
    {
        _colonyCount = 5;
        _currentFood = 30;
        _currentDay = 1;
        _credits = 1000;
        _foodTime = _dayTime / (_colonyCount * 5);
        _foodText.text = $"{_currentFood}";
        _peapleCount.text = $"{_colonyCount}";
        _creditsCount.text = $"{_credits}";
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        _currentFoodTime += Time.deltaTime;

        if (_currentFoodTime >= _foodTime)
        {
            _currentFoodTime = 0;
            _currentFood--;
            if(_currentFood > 0)
            {
                _foodText.text = $"{_currentFood}";
            }
            else
            {
                _colonyCount--;
                _foodTime = _dayTime / (_colonyCount * 5);
                _peapleCount.text = $"{_colonyCount}";
            }
        }

        if(_currentTime >= _dayTime)
        {
            _currentTime = 0;
            _currentDay++;
            _colonyCount++;
            _peapleCount.text = $"{_colonyCount}";
            _foodTime = _dayTime / (_colonyCount * 5);
        }
    }

    public void FoodAdd(float count)
    {
        _currentFood += (int)count;
        _foodText.text = $"{_currentFood}";
    }

    public void AddCredits(float count)
    {
        _credits += (int)count;
        _creditsCount.text = $"{_credits}";
    }

    public void RemoveCredits(float count)
    {
        _credits -= (int)count;
        _creditsCount.text = $"{_credits}";
    }
}
