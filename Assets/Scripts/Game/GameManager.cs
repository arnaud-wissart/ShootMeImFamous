using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceRocks
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private bool _isGameOver;
        private int _score;

        public int Score => _score;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            if (_isGameOver && Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        public void PlayerDied()
        {
            if (_isGameOver)
            {
                return;
            }

            _isGameOver = true;
            Time.timeScale = 0f;
            Debug.Log("[GAME] Game Over - appuyez sur R pour recommencer");
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            var activeScene = SceneManager.GetActiveScene();
            Debug.Log("[GAME] Restarting scene");
            SceneManager.LoadScene(activeScene.buildIndex);
        }

        public void RegisterAsteroidDestroyed(AsteroidSize asteroidSize)
        {
            var points = asteroidSize switch
            {
                AsteroidSize.Small => 10,
                AsteroidSize.Medium => 5,
                AsteroidSize.Large => 2,
                _ => 0,
            };

            _score += points;
            Debug.Log($"[SCORE] +{points} ({asteroidSize}) => total {_score}");
        }
    }
}
