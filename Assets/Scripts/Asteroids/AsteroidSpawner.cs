using UnityEngine;

namespace SpaceRocks
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform player;

        [SerializeField]
        private Asteroid asteroidPrefab;

        [SerializeField]
        private int initialCount = 10;

        [SerializeField]
        private float spawnRadiusMin = 20f;

        [SerializeField]
        private float spawnRadiusMax = 45f;

        [SerializeField]
        private float despawnDistance = 70f;

        [SerializeField]
        private float minimumSpawnDistanceFromPlayer = 5f;

        private void Start()
        {
            for (var i = 0; i < initialCount; i++)
            {
                SpawnAsteroid();
            }
        }

        private void Update()
        {
            if (player == null)
            {
                return;
            }

            CleanupDistantAsteroids();
            MaintainPopulation();
        }

        private void SpawnAsteroid()
        {
            if (asteroidPrefab == null || player == null)
            {
                return;
            }

            var position = GetSpawnPosition();
            var asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);
            var baseSpeed = asteroidPrefab.DefaultDriftSpeed;
            asteroid.Initialize(GetRandomSize(), baseSpeed);
        }

        private Vector3 GetSpawnPosition()
        {
            const int maxAttempts = 10;
            var lastPosition = player.position;

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var angle = Random.Range(0f, Mathf.PI * 2f);
                var radius = Random.Range(spawnRadiusMin, spawnRadiusMax);
                var offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
                var candidate = player.position + offset;
                lastPosition = candidate;

                if (Vector3.Distance(player.position, candidate) >= minimumSpawnDistanceFromPlayer)
                {
                    return candidate;
                }
            }

            var fallbackDirection = (lastPosition - player.position).normalized;
            if (fallbackDirection == Vector3.zero)
            {
                fallbackDirection = Random.insideUnitSphere;
                fallbackDirection.y = 0f;
            }

            fallbackDirection.y = 0f;
            fallbackDirection.Normalize();
            return player.position + fallbackDirection * Mathf.Max(minimumSpawnDistanceFromPlayer, spawnRadiusMin);
        }

        private AsteroidSize GetRandomSize()
        {
            var roll = Random.value;
            if (roll < 0.55f)
            {
                return AsteroidSize.Small;
            }

            if (roll < 0.9f)
            {
                return AsteroidSize.Medium;
            }

            return AsteroidSize.Large;
        }

        private void CleanupDistantAsteroids()
        {
            if (player == null)
            {
                return;
            }

            var asteroids = FindObjectsOfType<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                if (asteroid == null)
                {
                    continue;
                }

                var distance = Vector3.Distance(player.position, asteroid.transform.position);
                if (distance > despawnDistance)
                {
                    Destroy(asteroid.gameObject);
                }
            }
        }

        private void MaintainPopulation()
        {
            var asteroids = FindObjectsOfType<Asteroid>();
            if (asteroids.Length >= initialCount)
            {
                return;
            }

            var toSpawn = initialCount - asteroids.Length;
            for (var i = 0; i < toSpawn; i++)
            {
                SpawnAsteroid();
            }
        }
    }
}
