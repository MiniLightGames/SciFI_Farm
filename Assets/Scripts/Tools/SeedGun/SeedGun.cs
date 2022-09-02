using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SeedGun : BaseGun
{
    Seed—lip _seedClip;

    [SerializeField] Seed _seed;
    [SerializeField] SeedBullet _seedBullet;
    [SerializeField] SeedMenu _seedMenu;
    [SerializeField] SeedClipIcon _seedClipIcon;

    override public void Awake()
    {
        base.Awake();
        _seedMenu.Seed—lipTake += SeedClipTake;
    }

    void Update()
    {
        if(_inputManager.LeftClick)
        {
            if(!isReload && _seedClip && _inputManager.cursorLocked)
            {
                Seed bullet = _seedClip.GetBullet();
                if (bullet)
                {
                    SeedBullet seedBullet = Instantiate(_seedBullet, _firePosition.position, Quaternion.identity);
                    seedBullet.InitBullet(bullet, transform.forward);
                    _seedClipIcon.UpdateCount(_seedClip.Count);
                    StartCoroutine(Reload());
                }
                else
                {
                    _seedClip.GrabInfo.CountUpdate -= UpdateCount;
                    Destroy(_seedClip.GrabInfo.GameObject);
                }
            }
        }
        else if(_inputManager.RightClick)
        {
            if (!_seedMenu.gameObject.activeSelf)
            {
                _seedMenu.gameObject.SetActive(true);
            }
        }
    }

    void SeedClipTake(Seed—lip seed—lip)
    {
        _seedClip = seed—lip;
        _seedClipIcon.Init(seed—lip.SeedParametrs.SeedSprite, seed—lip.Count);
        _seedClip.GrabInfo.CountUpdate += UpdateCount;
        _seedClipIcon.gameObject.SetActive(true);
        StartCoroutine(Reload());
    }

    void UpdateCount()
    {
        _seedClipIcon.UpdateCount(_seedClip.Count);
    }

    IEnumerator Reload()
    {
        isReload = true;
        yield return new WaitForSeconds(_fireRate);
        isReload = false;
    }
}
