using UnityEngine;

namespace SpaceRocks
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed = 28f;

        [SerializeField]
        private float lifeTime = 2.5f;

        private Rigidbody _rigidbody;
        private bool _hasImpacted;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rigidbody.velocity = transform.forward * speed;
            Destroy(gameObject, lifeTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleImpact(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleImpact(other.gameObject);
        }

        private void HandleImpact(GameObject other)
        {
            if (_hasImpacted)
            {
                return;
            }

            if (other.TryGetComponent(out Asteroid asteroid))
            {
                asteroid.Hit();
            }

            _hasImpacted = true;
            Destroy(gameObject);
        }
    }
}
