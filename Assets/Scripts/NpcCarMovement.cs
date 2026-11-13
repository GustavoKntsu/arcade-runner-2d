using UnityEngine;

public class EnemyCarMovement : MonoBehaviour
{
    public float speed = 5f;
    public float destroyYPosition = -7f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPitStopping) return;

        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (transform.position.y <= destroyYPosition)
        {
            Destroy(gameObject);
        }
    }
}