using UnityEngine;

namespace SpaceRocks
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField]
        private AsteroidSize size = AsteroidSize.Medium;

        [SerializeField]
        private float driftSpeed = 3f;

        [SerializeField]
        private Asteroid asteroidPrefab;

        private Rigidbody _rigidbody;
        private float _baseDriftSpeed;
        private bool _initialized;

        public float DefaultDriftSpeed => _baseDriftSpeed > 0f ? _baseDriftSpeed : driftSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _baseDriftSpeed = driftSpeed;
        }

        private void Start()
        {
            // On s'assure que l'astéroïde est configuré avant de dériver.
            if (!_initialized)
            {
                ApplySizeSettings();
                LaunchDrift();
                _initialized = true;
            }
        }

        public void Initialize(AsteroidSize newSize, float inheritedDriftSpeed)
        {
            size = newSize;
            _baseDriftSpeed = inheritedDriftSpeed;
            ApplySizeSettings();
            LaunchDrift();
            _initialized = true;
        }

        public void Hit()
        {
            switch (size)
            {
                case AsteroidSize.Small:
                    Destroy(gameObject);
                    break;
                case AsteroidSize.Medium:
                    SpawnFragments(AsteroidSize.Small, 2);
                    Destroy(gameObject);
                    break;
                case AsteroidSize.Large:
                    SpawnFragments(AsteroidSize.Medium, 2);
                    Destroy(gameObject);
                    break;
            }
        }

        private void SpawnFragments(AsteroidSize targetSize, int count)
        {
            if (asteroidPrefab == null)
            {
                Destroy(gameObject);
                return;
            }

            for (var i = 0; i < count; i++)
            {
                var offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
                var spawnPosition = transform.position + offset;
                var fragment = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
                fragment.Initialize(targetSize, _baseDriftSpeed);
            }
        }

        private void ApplySizeSettings()
        {
            transform.localScale = Vector3.one * GetScale(size);
            driftSpeed = _baseDriftSpeed * GetSpeedMultiplier(size);
        }

        private void LaunchDrift()
        {
            var randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0f;
            randomDirection.Normalize();
            _rigidbody.velocity = randomDirection * driftSpeed;
        }

        private static float GetScale(AsteroidSize asteroidSize)
        {
            return asteroidSize switch
            {
                AsteroidSize.Small => 0.6f,
                AsteroidSize.Medium => 1f,
                AsteroidSize.Large => 1.6f,
                _ => 1f,
            };
        }

        private static float GetSpeedMultiplier(AsteroidSize asteroidSize)
        {
            return asteroidSize switch
            {
                AsteroidSize.Small => 1.25f,
                AsteroidSize.Medium => 1f,
                AsteroidSize.Large => 0.85f,
                _ => 1f,
            };
        }
    }
}
