using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuilderBox : MonoBehaviour, IGrab
{
    [Inject(Id = TransformId.Player)] Transform _playerTransform;
    [Inject] InputManager _inputManager;
    [Inject] DiContainer _container;

    bool _isBuild;
    StructureView _structure;
    GrabInfo _grabInfo;
    Transform _transform;

    Vector3 _startPos;
    Quaternion _startRot;
    Transform _parent;

    Ray ray;
    RaycastHit hitData;

    [SerializeField] Transform _startPoint;
    [SerializeField] float _raycastDistance;
    [SerializeField] LayerMask _placeLayer;
    [SerializeField] LayerMask _structureLayer;
    [SerializeField] AnimationCurve _SpeedCurve;
    [SerializeField] float _Speed;

    public bool CanGrab => true;

    public void Awake()
    {
        _grabInfo = GetComponent<GrabInfo>();
        _transform = GetComponent<Transform>();
        _placeLayer = 9;
        _structureLayer = 14;
    }

    public GrabInfo Init(Structure structure)
    {
        _grabInfo.Init(structure.Name, this, structure.StructureSprite);
        var prefab = _container.InstantiatePrefab(structure.StructurePrefab, Vector3.zero, Quaternion.identity, _transform);
        _structure = prefab.GetComponent<StructureView>();
        _structure.InitView();
        return _grabInfo;
    }

    public void RepeateStructure()
    {
        var prefab = _container.InstantiatePrefab(_structure, Vector3.zero, Quaternion.identity, _transform);
        _structure = prefab.GetComponent<StructureView>();
        _structure.InitView();
    }

    void Update()
    {
        if (!_isBuild)
        {
            ray = new Ray(_startPoint.position, _startPoint.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, _raycastDistance);
            bool needDisable = true;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger) { continue; }

                if (hits[i].collider.gameObject.layer == _placeLayer && hits[i].normal.y >= 0.98f)
                {
                    hitData = hits[i];
                    ShowStructureView();
                    _structure.Show(true);
                    needDisable = false;
                }
            }

            if (needDisable)
            {
                _structure.Show(false);
            }
        }
    }

    void ShowStructureView()
    {
       // Transform nearestStructure;
        Vector3 position = hitData.point;
        Quaternion rotation = Quaternion.FromToRotation(_playerTransform.up, hitData.normal) * _playerTransform.rotation;

        bool isFindPlace = false;// TryFindInstantPlace(ref position, out nearestStructure);

        if (isFindPlace)
        {
            // ShowStructureAnchor(nearestStructure, ref position, ref rotation);
        }
        else if (_inputManager.LeftClick && _structure.CanBuild)
        {
            StartCoroutine(Coroutine_Throw(position, rotation));
        }

        _structure.Transform.position = position;
        _structure.Transform.rotation = Quaternion.AngleAxis(180, _playerTransform.up) * rotation;
    }

    void ShowStructureAnchor(Transform nearestSeedBed, ref Vector3 position, ref Quaternion rotation)
    {
        RaycastHit hit;
        Vector3 test = position;
        ray = new Ray(test, Vector3.down);
        rotation = Quaternion.FromToRotation(nearestSeedBed.up, hitData.normal) * nearestSeedBed.rotation;

        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            if (hit.collider.gameObject.layer == _placeLayer && hit.normal.y >= 0.98f)
            {
                if (_inputManager.LeftClick)
                {
                    StartCoroutine(Coroutine_Throw(position, rotation));
                }
            }
        }
    }

    bool TryFindInstantPlace(ref Vector3 point, out Transform nearestSeedBed)
    {
        Ray nearestRay;
        RaycastHit nearestHitData;
        var rotation = Quaternion.FromToRotation(_playerTransform.up, hitData.normal) * _playerTransform.rotation;
        Vector3 pos = point;
        pos.y += 0.1f;

        nearestRay = new Ray(pos, rotation * Vector3.left);
        Debug.DrawRay(pos, rotation * Vector3.left, Color.white);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1f))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(pos, rotation * Vector3.right);
        Debug.DrawRay(pos, rotation * Vector3.right, Color.blue);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1f))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(pos, rotation * Vector3.back);
        Debug.DrawRay(pos, rotation * Vector3.back, Color.green);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1f))
        {
            nearestSeedBed = nearestHitData.transform;
            point = FindInstantPosition(nearestHitData.transform, point);
            return true;
        }

        nearestRay = new Ray(pos, rotation * Vector3.forward);
        Debug.DrawRay(pos, rotation * Vector3.forward, Color.yellow);
        if (Physics.Raycast(nearestRay, out nearestHitData, 1f))
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
                return point.position + point.right * ((point.localScale.x / 2) + (_structure.Transform.localScale.x / 2));
            }
            else
            {
                return point.position - point.right * ((point.localScale.x / 2) + (_structure.Transform.localScale.x / 2));
            }
        }
        else
        {
            if (diraction.z < 0)
            {
                return point.position - point.forward * ((point.localScale.z / 2) + (_structure.Transform.localScale.z / 2));
            }
            else
            {
                return point.position + point.forward * ((point.localScale.z / 2) + (_structure.Transform.localScale.z / 2));
            }
        }
    }

    IEnumerator Coroutine_Throw(Vector3 target, Quaternion rotation)
    {
        _startPos = _transform.localPosition;
        _startRot = _transform.localRotation;
        _parent = _transform.parent;

        _grabInfo.SetCount(_grabInfo.Count - 1);
        _transform.SetParent(null);
        _structure.Transform.SetParent(null);
        _isBuild = true;
        float lerp = 0;
        Vector3 startPos = _transform.position;
        while (lerp < 1)
        {
            _transform.position = Vector3.Lerp(startPos, target, _SpeedCurve.Evaluate(lerp));
            _transform.rotation = Quaternion.Euler(_transform.rotation.eulerAngles.x + _SpeedCurve.Evaluate(lerp), 0, 0);
            float magnitude = (_transform.position - target).magnitude;
            if (magnitude < 0.4f)
            {
                break;
            }
            lerp += Time.deltaTime * _Speed;
            yield return null;
        }

        _structure.StartBuild();
        if (_grabInfo.Count > 0)
        {
            _transform.SetParent(_parent);
            _transform.localPosition = _startPos;
            _transform.localRotation = _startRot;
            _isBuild = false;
            RepeateStructure();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
