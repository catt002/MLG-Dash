using UnityEngine;
namespace GravityDash
{
    public class HitmarkerFX : MonoBehaviour
    {
        public float displayTime = 0.3f;
        void Start()
        {
            Destroy(gameObject, displayTime);
        }
    }
}

