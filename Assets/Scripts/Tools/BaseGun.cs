using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(GrabInfo))]
public class BaseGun : MonoBehaviour
{
    [Inject] protected InputManager _inputManager;

    [SerializeField] protected Gun _gunParameters;
    [SerializeField] protected Transform _firePosition;

    protected float _fireRate;
    protected Transform _transform;
    protected bool isReload;

    GrabInfo _grabInfo;

    public virtual void Awake()
    {
        _transform = GetComponent<Transform>();
        _grabInfo = GetComponent<GrabInfo>();
        _grabInfo.Init(_gunParameters.name, _gunParameters.Explored, _gunParameters.InventorySprite);
        _fireRate = _gunParameters.FireRate;
    }
}
