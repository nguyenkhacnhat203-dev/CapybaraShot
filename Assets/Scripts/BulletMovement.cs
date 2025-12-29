using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BulletSpawner spawner;
    private bool isReturning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        transform.rotation = Quaternion.identity;
    }

    public void Launch(Vector3 targetPos, float speed, BulletSpawner originSpawner)
    {
        spawner = originSpawner;
        isReturning = false;

        Vector2 direction = (targetPos - transform.position).normalized;
        rb.velocity = direction * speed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning) return;

        if (other.CompareTag("Wall"))
        {
            Vector2 closestPoint = other.ClosestPoint(transform.position);
            Vector2 normal = ((Vector2)transform.position - closestPoint).normalized;
            if (normal == Vector2.zero) return;

            Vector2 reflectDir = Vector2.Reflect(rb.velocity, normal);
            rb.velocity = reflectDir;

        }
        else if (other.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            isReturning = true;

            transform.rotation = Quaternion.identity;

            spawner.ReturnBulletToQueue(this.gameObject);
        }
    }
}