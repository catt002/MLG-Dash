using UnityEngine;
namespace GravityDash
{
    public class RGBColorCycle : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public float speed = 0.5f;
        private Texture2D _gradientTex;
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer != null)
            {
                _gradientTex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
                _gradientTex.filterMode = FilterMode.Bilinear;
                _gradientTex.wrapMode = TextureWrapMode.Clamp;
                Sprite newSprite = Sprite.Create(_gradientTex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64f);
                _spriteRenderer.sprite = newSprite;
                _spriteRenderer.color = Color.white;
            }
        }
        void Update()
        {
            if (_spriteRenderer != null && _gradientTex != null)
            {
                float timeOffset = Time.time * speed;
                Color32[] colors = new Color32[64 * 64];
                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        float uv = x / 63f;
                        float hue = (timeOffset + uv) % 1f;
                        Color32 pixelColor = Color.HSVToRGB(hue, 1f, 1f);
                        colors[y * 64 + x] = pixelColor;
                    }
                }
                _gradientTex.SetPixels32(colors);
                _gradientTex.Apply();
            }
        }
        void OnDestroy()
        {
            if (_gradientTex != null)
            {
                Destroy(_gradientTex);
            }
        }
    }
}

