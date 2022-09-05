using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBullet : MonoBehaviour
{
    [SerializeField] float _speed;

    Seed _seedParameters;
    Rigidbody _rigidbody;
    BoxCollider _boxCollider;
    Vector3 _endPoint;
    bool _isMove;
    SeedBedController _seedBed;

    public Seed SeedParameters => _seedParameters;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void InitBullet(Seed seed, Vector3 endPoint, Vector3 direction, SeedBedController seedBed = null)
    {
        _seedParameters = seed;
        _endPoint = endPoint;
        _rigidbody.AddForce(direction * _speed, ForceMode.Impulse);
        _isMove = true;
        _seedBed = seedBed;
        StartCoroutine(LifeTime());
    }

    void FixedUpdate()
    {
        if(_isMove)
        {
            float distance = (_endPoint - _rigidbody.position).magnitude;
            if (distance <= 0.85f)
            {
                Stop();
                if (_seedBed)
                {
                    _seedBed.Planted(this);
                }
            }
        }
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _boxCollider.enabled = false;
        _isMove = false;
    }
}
