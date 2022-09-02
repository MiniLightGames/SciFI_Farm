using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HoeGun : BaseGun
{
    [Inject(Id = TransformId.Player)] Transform _playerTransform;
    [Inject(Id = TransformId.SeedBeds)] Transform _seedBedParent;

    [SerializeField] Transform _seedBedView;
    [SerializeField] Transform _seedBedPrefab;

    [SerializeField] float _raycastDistance = 5;
    [SerializeField] LayerMask _placeLayer;
    [SerializeField] LayerMask _seedBedLayer;

    [SerializeField] Sprite _Shoe;
    [SerializeField] Sprite _Hammer;

    [SerializeField] Sprite _ShoePoint;
    [SerializeField] Sprite _HammerPoint;

    [SerializeField] Image _Icon;
    [SerializeField] Image _Point;

    Ray ray;
    RaycastHit hitData;
    bool isDestroy;
    bool wait;
    DestroyView _currentView;

    public override void Awake()
    {
        base.Awake();

        _placeLayer = 9;
        _seedBedView = Instantiate(_seedBedView, _transform);
    }

    void OnEnable()
    {
        SetStateSeedBedView(true);
    }

    void Update()
    {
        if(_inputManager.RightClick && !wait)
        {
            isDestroy = !isDestroy;
            if (isDestroy)
            {
                _Icon.sprite = _Hammer;
                _Point.sprite = _HammerPoint;
            }
            else
            {
                _Icon.sprite = _Shoe;
                _Point.sprite = _ShoePoint;
                ResetDestroy();
            }
            wait = true;
        }
        else if (!_inputManager.RightClick && wait)
        {
            wait = false;
        }

        ray = new Ray(_firePosition.position, _transform.forward);
        if (Physics.Raycast(ray, out hitData, _raycastDistance))
        {
            if(!isDestroy)
            {
                if (hitData.collider.gameObject.layer == _placeLayer && hitData.normal.y >= 0.75f)
                {
                    SetStateSeedBedView(true);
                    TryCreateSeedBed();
                }
                else
                {
                    SetStateSeedBedView(false);
                }
            }
            else
            {
                TryDestroy(hitData.collider.gameObject);
                SetStateSeedBedView(false);
            }
        }
        else
        {
            if (isDestroy)
            {
                ResetDestroy();
            }
            else
            {
                SetStateSeedBedView(false);
            }
        }
    }

    void TryCreateSeedBed()
    {
        Transform nearestSeedBed;
        Vector3 position = hitData.point;
        Quaternion rotation = Quaternion.FromToRotation(_playerTransform.up, hitData.normal) * _playerTransform.rotation;

        bool isFindPlace = TryFindInstantPlace(ref position, out nearestSeedBed);
        position.y = hitData.point.y - (_seedBedPrefab.localScale.y / 2.5f);

        if (isFindPlace)
        {
            TryCreateAnchorSeedBed(nearestSeedBed, ref position, ref rotation);
        }
        else if (_inputManager.LeftClick)
        {
            Instantiate(_seedBedPrefab, position, rotation, _seedBedParent);
        }

        _seedBedView.position = position;
        _seedBedView.rotation = rotation;
    }

    void TryCreateAnchorSeedBed(Transform nearestSeedBed, ref Vector3 position, ref Quaternion rotation)
    {
        RaycastHit hit;
        Vector3 test = position;
        test.y += 2f;
        ray = new Ray(test, Vector3.down);
        rotation = Quaternion.FromToRotation(nearestSeedBed.up, hitData.normal) * nearestSeedBed.rotation;
       
        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            if (hit.collider.gameObject.layer == _placeLayer && hit.normal.y >= 0.75f)
            {
                position.y = hit.point.y - (_seedBedPrefab.localScale.y / 2.5f);
                if (_inputManager.LeftClick)
                {
                    Instantiate(_seedBedPrefab, position, rotation, _seedBedParent);
                }
            }
        }
    }

    bool TryFindInstantPlace(ref Vector3 point, out Transform nearestSeedBed)
    {
        Ray nearestRay;
        RaycastHit nearestHitData;
        point.y -= 0.1f;
        var rotation = Quaternion.FromToRotation(_playerTransform.up, hitData.normal) * _playerTransform.rotation;

        nearestRay = new Ray(point, rotation * Vector3.left);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1F, _seedBedLayer))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(point, rotation * Vector3.right);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1F, _seedBedLayer))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(point, rotation * Vector3.back);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1F, _seedBedLayer))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(point, rotation * Vector3.forward);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1F, _seedBedLayer))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestSeedBed = nearestHitData.transform;
        return false;
    }

    Vector3 FindInstantPosition(Transform point, Vector3 side)
    {
        Vector3 diraction = side - point.position;
        diraction = Quaternion.Inverse(point.rotation) * diraction;
        if (Mathf.Abs(diraction.x) > Mathf.Abs(diraction.z))
        {
            if (diraction.x > 0)
            {
                return point.position + point.right * ((point.localScale.x / 2) + (_seedBedPrefab.localScale.x / 2));
            }
            else
            {
                return point.position - point.right * ((point.localScale.x / 2) + (_seedBedPrefab.localScale.x / 2));
            }
        }
        else
        {
            if (diraction.z < 0)
            {
                return point.position - point.forward * ((point.localScale.z / 2) + (_seedBedPrefab.localScale.z / 2));
            }
            else
            {
                return point.position + point.forward * ((point.localScale.z / 2) + (_seedBedPrefab.localScale.z / 2));
            }
        }
    }

    void SetStateSeedBedView(bool state)
    {
        if (state != _seedBedView.gameObject.activeSelf)
        {
            _seedBedView.gameObject.SetActive(state);
        }
    }

    void TryDestroy(GameObject gameObject)
    {
        if (_currentView)
        {
            if (_currentView.gameObject != gameObject)
            {
                _currentView.ResetDestroy();
                _currentView = gameObject.GetComponent<DestroyView>(); ;
                _currentView?.InitDestoy();
            }
            else if (_inputManager.LeftClick)
            {
               bool needDestroy = _currentView.TryDestroy();
               if (needDestroy)
               {
                    Destroy(_currentView.gameObject);
                    _currentView = null;
               }
            }
        }
        else
        {
            _currentView = gameObject.GetComponent<DestroyView>(); ;
            _currentView?.InitDestoy();
        }
    }

    void ResetDestroy()
    {
        if (_currentView)
        {
            _currentView?.ResetDestroy();
            _currentView = null;
        }
    }

    void OnDisable()
    {
        SetStateSeedBedView(false);
    }
}
