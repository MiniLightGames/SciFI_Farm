using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBullet : MonoBehaviour
{
    [SerializeField] float _speed;

    Seed _seedParameters;
    Rigidbody _rigidbody;
    BoxCollider _boxCollider;

    public Seed SeedParameters => _seedParameters;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void InitBullet(Seed seed, Vector3 direction)
    {
        _seedParameters = seed;
        _rigidbody.AddForce(direction * _speed, ForceMode.Impulse);
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if(_boxCollider.enabled)
        {
            _boxCollider.enabled = false;
        }
    }
}
