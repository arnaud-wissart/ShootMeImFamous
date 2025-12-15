using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceRocks
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private bool _isGameOver;

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
            Debug.Log("Game Over - appuyez sur R pour recommencer");
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}
