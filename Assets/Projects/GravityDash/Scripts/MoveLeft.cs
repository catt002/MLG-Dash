using UnityEngine;
namespace GravityDash
{
    public class MoveLeft : MonoBehaviour
    {
        [SerializeField] private float speed = 7f;
        void Update()
        {
            float currentSpeed = speed;
            if (GameManager.Instance != null)
            {
                currentSpeed *= GameManager.Instance.GameSpeedMultiplier;
            }
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);
            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
        }
    }
}

