using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace GravityDash
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Text scoreText;
        public Text gameOverScoreText;
        public Text bestScoreText;
        public GameObject gameOverPanel;
        private float _score;
        private bool _isGameOver = false;
        public float GameSpeedMultiplier { get; private set; } = 1f;
        private float _elapsedTime;
        private float _timeToMaxSpeed = 30f;
        private float _maxTimeScale = 1.5f;
        private float _delayBeforeAcceleration = 7f;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }
        private void Update()
        {
            if (_isGameOver)
            {
                if (UnityEngine.InputSystem.Keyboard.current != null && 
                    UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.02f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                return;
            }
            _elapsedTime += Time.deltaTime;
            float timeAfterDelay = Mathf.Max(0f, _elapsedTime - _delayBeforeAcceleration);
            GameSpeedMultiplier = Mathf.Lerp(1f, _maxTimeScale, Mathf.Clamp01(timeAfterDelay / _timeToMaxSpeed));
            _score += Time.deltaTime * 10f * GameSpeedMultiplier;
            if (scoreText != null)
            {
                scoreText.text = "Score: " + Mathf.FloorToInt(_score).ToString();
            }
        }
        public void AddScore(float amount)
        {
            if (!_isGameOver)
            {
                _score += amount;
                if (scoreText != null)
                {
                    scoreText.text = "Score: " + Mathf.FloorToInt(_score).ToString();
                }
            }
        }
        public void GameOver()
        {
            _isGameOver = true;
            Time.timeScale = 0f;
            GameObject bgm = GameObject.Find("BackgroundMusic");
            if (bgm != null)
            {
                var audio = bgm.GetComponent<AudioSource>();
                if (audio != null) audio.Stop();
            }
            int finalScore = Mathf.FloorToInt(_score);
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (finalScore > bestScore)
            {
                bestScore = finalScore;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
            }
            if (gameOverScoreText != null) gameOverScoreText.text = "Score: " + finalScore;
            if (bestScoreText != null) bestScoreText.text = "Best: " + bestScore;
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
    }
}

