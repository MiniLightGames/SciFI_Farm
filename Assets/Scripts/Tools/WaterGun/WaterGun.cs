using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class WaterGun : BaseGun
{
    [SerializeField] float _maxAmount;
    [SerializeField] Transform _waterPool;
    [SerializeField] WaterBall _waterBall;
    [SerializeField] Image _amountImage;

    List<WaterBall> balls = new List<WaterBall>();
    float _currentAmount;

    Ray ray;
    RaycastHit hitData;

    public override void Awake()
    {
        base.Awake();
        for(int i = 0; i < _maxAmount; i++)
        {
            balls.Add(Instantiate(_waterBall, _firePosition.position, Quaternion.identity, _waterPool));
        }
        _amountImage.fillAmount = _currentAmount / _maxAmount;
    }

    void Update()
    {
        if(_currentAmount > 0 && balls.Count>0)
        {
            if (_inputManager.LeftClick && !isReload)
            {
                balls[(int)_currentAmount -1].Throw(_transform.forward, _firePosition.position);
                StartCoroutine(Reload());
                _currentAmount--;
                _amountImage.fillAmount = _currentAmount / _maxAmount;
            }
        }

        if (_inputManager.RightClick && _currentAmount < _maxAmount && !isReload)
        {
            ray = new Ray(_firePosition.position, _firePosition.forward);
            if (Physics.Raycast(ray, out hitData, 5f))
            {
                if (hitData.collider.gameObject.layer == 4)
                {
                    if(_currentAmount < _maxAmount)
                    {
                        balls[(int)_currentAmount].Get(_firePosition, hitData.point);
                        _currentAmount++;
                        _amountImage.fillAmount = _currentAmount / _maxAmount;
                        StartCoroutine(Reload());
                    }
                }
            }
        }
    }

    IEnumerator Reload()
    {
        isReload = true;
        yield return new WaitForSeconds(_fireRate);
        isReload = false;   
    }
}
