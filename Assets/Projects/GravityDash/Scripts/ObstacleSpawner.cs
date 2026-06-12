using UnityEngine;
namespace GravityDash
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject obstaclePrefab;
        public Sprite[] illuminatiSprites;
        public Sprite airIlluminatiSprite;
        [SerializeField] private float startMinInterval = 0.8f;
        [SerializeField] private float startMaxInterval = 1.8f;
        [SerializeField] private float endMinInterval = 0.4f;
        [SerializeField] private float endMaxInterval = 0.8f;
        [SerializeField] private float timeToMaxDensity = 20f;
        [SerializeField] private float topY = 4.625f;
        [SerializeField] private float bottomY = -4.625f;
        [SerializeField] private float spawnX = 16f;
        private float _timer;
        private float _currentSpawnInterval;
        private float _elapsedTime;
        private float _lastAirSpawnTime = -10f;
        public float LastAirSpawnTime => _lastAirSpawnTime;
        [SerializeField] private float minAirSpawnCooldown = 4.0f;
        void Start()
        {
            _currentSpawnInterval = Random.Range(startMinInterval, startMaxInterval);
        }
        void Update()
        {
            _elapsedTime += Time.unscaledDeltaTime;
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnInterval)
            {
                SpawnObstacle();
                _timer = 0f;
                float t = Mathf.Clamp01(_elapsedTime / timeToMaxDensity);
                float currentMin = Mathf.Lerp(startMinInterval, endMinInterval, t);
                float currentMax = Mathf.Lerp(startMaxInterval, endMaxInterval, t);
                float mult = GameManager.Instance != null ? GameManager.Instance.GameSpeedMultiplier : 1f;
                _currentSpawnInterval = Random.Range(currentMin, currentMax) / mult;
            }
        }
        private void SpawnObstacle()
        {
            if (obstaclePrefab == null) return;
            bool spawnOnTop = Random.value > 0.5f;
            float yPos = spawnOnTop ? topY : bottomY;
            Sprite chosenSprite = (illuminatiSprites != null && illuminatiSprites.Length > 0) ? illuminatiSprites[Random.Range(0, illuminatiSprites.Length)] : null;
            bool isAir = false;
            if (_elapsedTime >= 7f && (_elapsedTime - _lastAirSpawnTime) >= minAirSpawnCooldown)
            {
                float rand = Random.value;
                if (rand < 0.30f && airIlluminatiSprite != null)
                {
                    isAir = true;
                    chosenSprite = airIlluminatiSprite;
                    yPos = Random.Range(-2.5f, 2.5f);
                    _lastAirSpawnTime = _elapsedTime;
                }
                else
                {
                    ApplyStandardOffset(chosenSprite, spawnOnTop, ref yPos);
                }
            }
            else
            {
                ApplyStandardOffset(chosenSprite, spawnOnTop, ref yPos);
            }
            GameObject obs = Instantiate(obstaclePrefab, new Vector3(spawnX, yPos, 0), Quaternion.identity);
            if (chosenSprite != null)
            {
                var sr = obs.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = chosenSprite;
                    sr.color = Color.white;
                }
                var oldCol = obs.GetComponent<Collider2D>();
                if (oldCol != null) Destroy(oldCol);
                obs.AddComponent<PolygonCollider2D>();
            }
            if (spawnOnTop && !isAir)
            {
                Vector3 scale = obs.transform.localScale;
                scale.y = -Mathf.Abs(scale.y);
                obs.transform.localScale = scale;
            }
        }
        private void ApplyStandardOffset(Sprite chosenSprite, bool spawnOnTop, ref float yPos)
        {
            if (chosenSprite != null && chosenSprite.name == "illuminati_2")
            {
                if (spawnOnTop) yPos += 0.275f;
                else yPos -= 0.275f;
            }
            else if (chosenSprite != null && chosenSprite.name == "illuminati_3")
            {
                if (spawnOnTop) yPos += 0.275f;
                else yPos -= 0.275f;
            }
        }
    }
}

