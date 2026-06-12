using UnityEngine;
namespace GravityDash
{
    public class DoritoPickup : MonoBehaviour
    {
        private float _animTimer = 0f;
        private bool _isRotatedRight = true;
        void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, 30f);
        }
        void Update()
        {
            _animTimer += Time.deltaTime;
            if (_animTimer >= 0.5f)
            {
                _animTimer = 0f;
                _isRotatedRight = !_isRotatedRight;
                transform.rotation = Quaternion.Euler(0, 0, _isRotatedRight ? 30f : -30f);
            }
        }
    }
}

