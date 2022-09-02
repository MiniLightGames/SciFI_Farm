using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGun : BaseGun
{
    Ray ray;
    RaycastHit hitData;
    bool _menuShow;
    float _currentTime;

    [SerializeField] SearchGunMenu _menu;
    [SerializeField] ParticleSystem _particle;

    void Update()
    {
        if(_menuShow)
        {
            return;
        }

        ray = new Ray(_firePosition.position, _transform.forward);
        if (Physics.Raycast(ray, out hitData, 5f))
        {
            if (hitData.collider.gameObject.layer == 12 )
            {
                if (_inputManager.LeftClick)
                {
                    if(!_particle.isPlaying)
                    {
                        _particle.Play();
                    }

                    StartSearch(hitData.collider.gameObject);
                }
                else
                {
                    StopSearch();
                }
            }
            else
            {
                StopSearch();
            }
        }
        else
        {
            StopSearch();
        }
    }

    void StartSearch(GameObject plant)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= 3f)
        {
            _particle.Stop();
            _currentTime = 0;
            _menuShow = true;
            Plant seed = plant.GetComponent<Plant>();
            seed.PlantInfo.Explored = true;
            _menu.Show(seed.PlantInfo);
            _inputManager.SetCursorState(false);
        }
    }

    public void ExitMenu()
    {
        _inputManager.SetCursorState(true);
        _menu.gameObject.SetActive(false);
        _menuShow = false;
    }

    public void StopSearch()
    {
        if(_currentTime > 0)
        {
            _currentTime = 0;
            _particle.Stop(true);
        }
    }
}
