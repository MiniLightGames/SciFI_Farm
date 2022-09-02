using System.Collections;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    Rigidbody _rigibody;
    SphereCollider _collider;
    Transform _parent;
    Transform _transform;

    [SerializeField] float _speed;
    [SerializeField] ParticleSystem _waterBallParticleSystem;
    [SerializeField] ParticleSystem _splashPrefab;
    [SerializeField] AnimationCurve _SpeedCurve;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigibody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();

        _parent = _transform.parent;
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

    public void Throw(Vector3 diraction, Vector3 startPos)
    {
        _transform.SetParent(null);
        gameObject.SetActive(true);
        _collider.enabled = true;
        _rigibody.useGravity = true;

        _rigibody.AddForce(diraction * _speed, ForceMode.Impulse);
        _waterBallParticleSystem.Play();
        StartCoroutine(LifeTime());
    }

    void OnCollisionEnter(Collision collision)
    {
        _collider.enabled = false;
        _rigibody.useGravity = false;
        _rigibody.velocity = Vector3.zero;
        _rigibody.angularVelocity = Vector3.zero;
        _waterBallParticleSystem.Stop();

        var rotation = Quaternion.FromToRotation(_splashPrefab.transform.up, collision.contacts[0].normal) * _splashPrefab.transform.rotation;
        _splashPrefab.transform.rotation = rotation;

        _splashPrefab.gameObject.SetActive(true);
        _splashPrefab.Play();
    }

    public void Get(Transform transform, Vector3 startPos)
    {
        gameObject.SetActive(true);

        _collider.enabled = false;
        _rigibody.useGravity = false;
        _rigibody.velocity = Vector3.zero;
        _rigibody.angularVelocity = Vector3.zero;
        _waterBallParticleSystem.Stop();
        _splashPrefab.gameObject.SetActive(false);
        _transform.SetParent(_parent);
        _transform.position = startPos;

        StartCoroutine(Coroutine_Throw(transform));
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(3f);
        _transform.SetParent(_parent);
        gameObject.SetActive(false);
    }

    IEnumerator Coroutine_Throw(Transform target)
    {
        bool stop = false;
        _waterBallParticleSystem.Play();
        while (!stop)
        {
            Vector3 diraction = (target.localPosition - _transform.localPosition);
            _transform.localPosition += (diraction * 10f * Time.deltaTime);
            if (diraction.magnitude < 0.4f)
            {
                stop = true;
            }
            yield return null;
        }

        _waterBallParticleSystem.Stop();
        gameObject.SetActive(false);
        yield break;
    }
}
