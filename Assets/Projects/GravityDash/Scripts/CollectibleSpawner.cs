using UnityEngine;
namespace GravityDash
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private GameObject obstaclePrefab;
        public Sprite[] collectibleSprites;
        [SerializeField] private float minInterval = 3.0f;
        [SerializeField] private float maxInterval = 6.0f;
        [SerializeField] private float spawnX = 16f;
        private float _timer;
        private float _currentInterval;
        private float _elapsedTime;
        void Start()
        {
            _currentInterval = Random.Range(minInterval, maxInterval);
        }
        void Update()
        {
            _elapsedTime += Time.unscaledDeltaTime;
            if (_elapsedTime >= 7f)
            {
                _timer += Time.deltaTime;
                if (_timer >= _currentInterval)
                {
                    float lastAirTime = obstacleSpawner != null ? obstacleSpawner.LastAirSpawnTime : -10f;
                    if (_elapsedTime - lastAirTime < 1.0f)
                    {
                        _timer -= 1.0f;
                    }
                    else
                    {
                        SpawnCollectible();
                        _timer = 0f;
                        float mult = GameManager.Instance != null ? GameManager.Instance.GameSpeedMultiplier : 1f;
                        _currentInterval = Random.Range(minInterval, maxInterval) / mult;
                    }
                }
            }
        }
        private void SpawnCollectible()
        {
            if (obstaclePrefab == null || collectibleSprites == null || collectibleSprites.Length == 0) return;
            Sprite chosenSprite = collectibleSprites[Random.Range(0, collectibleSprites.Length)];
            float yPos = Random.Range(-1.5f, 1.5f);
            GameObject obs = Instantiate(obstaclePrefab, new Vector3(spawnX, yPos, 0), Quaternion.identity);
            var sr = obs.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = chosenSprite;
                sr.color = Color.white;
            }
            var oldCol = obs.GetComponent<Collider2D>();
            if (oldCol != null) Destroy(oldCol);
            var box = obs.AddComponent<BoxCollider2D>();
            box.isTrigger = true;
            obs.AddComponent<DoritoPickup>();
        }
    }
}

