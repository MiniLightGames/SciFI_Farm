using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedÑlip : MonoBehaviour, IGrab
{
    GrabInfo _grabInfo;

    [SerializeField] Seed _seedParametrs;

    public Seed SeedParametrs => _seedParametrs;
    public GrabInfo GrabInfo => _grabInfo;
    public int Count => _grabInfo.Count;

    public bool CanGrab => true;

    void Awake()
    {
        _grabInfo = GetComponent<GrabInfo>();
    }

    public Seed GetBullet()
    {
        if(_grabInfo.Count > 0)
        {
            _grabInfo.SetCount(_grabInfo.Count - 1);
            return _seedParametrs;
        }

        return null;
    }

    public GrabInfo SetSeed(Seed seed)
    {
        _seedParametrs = seed;
        _grabInfo.Init(_seedParametrs.Name.ToString(), this, _seedParametrs.SeedSprite);
        return _grabInfo;
    }
}
