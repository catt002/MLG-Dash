using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
namespace GravityDash
{
    public class PlayerMover : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private AudioSource _audioSource;
        private bool _isDead = false;
        private bool _isGrounded = false;
        public AudioClip doritoHitSound;
        public GameObject hitmarkerPrefab;
        public AudioClip deathExplosionSound;
        public UnityEngine.Video.VideoClip explosionVideo;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = 0.7f;
            _rigidbody.gravityScale = Mathf.Abs(_rigidbody.gravityScale);
        }
        void Update()
        {
            if (_isDead) return;
            bool isPressed = false;
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) isPressed = true;
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) isPressed = true;
            if (isPressed)
            {
                _rigidbody.gravityScale *= -1;
                _isGrounded = false;
                Vector3 localScale = transform.localScale;
                localScale.y = Mathf.Sign(_rigidbody.gravityScale) * Mathf.Abs(localScale.y);
                transform.localScale = localScale;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isDead) return;
            if (collision.gameObject.name == "Ground" || collision.gameObject.name == "Ceiling")
            {
                _isGrounded = true;
            }
            if (collision.gameObject.GetComponent<MoveLeft>() != null)
            {
                _isDead = true;
                var sr = GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
                if (_rigidbody != null) _rigidbody.simulated = false;
                if (deathExplosionSound != null && _audioSource != null)
                {
                    _audioSource.PlayOneShot(deathExplosionSound);
                }
                if (explosionVideo != null)
                {
                    GameObject videoObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    Destroy(videoObj.GetComponent<Collider>());
                    videoObj.transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
                    videoObj.transform.localScale = new Vector3(6f, 6f, 1f);
                    var renderer = videoObj.GetComponent<MeshRenderer>();
                    renderer.material = new Material(Shader.Find("Sprites/Default"));
                    var vp = videoObj.AddComponent<UnityEngine.Video.VideoPlayer>();
                    vp.playOnAwake = true;
                    vp.clip = explosionVideo;
                    vp.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
                    vp.targetMaterialRenderer = renderer;
                    vp.targetMaterialProperty = "_MainTex";
                }
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var dorito = collider.gameObject.GetComponent<DoritoPickup>();
            if (dorito != null)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(50);
                }
                if (hitmarkerPrefab != null)
                {
                    var hit = Instantiate(hitmarkerPrefab, collider.transform.position, Quaternion.identity);
                    hit.transform.localScale = collider.transform.localScale * 0.12f;
                }
                if (doritoHitSound != null && _audioSource != null)
                {
                    _audioSource.PlayOneShot(doritoHitSound);
                }
                Destroy(collider.gameObject);
            }
        }
    }
}

