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

        // Khóa hoàn toàn việc xoay của Object bằng vật lý
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Đảm bảo khi bắt đầu, góc xoay là 0
        transform.rotation = Quaternion.identity;
    }

    public void Launch(Vector3 targetPos, float speed, BulletSpawner originSpawner)
    {
        spawner = originSpawner;
        isReturning = false;

        Vector2 direction = (targetPos - transform.position).normalized;
        rb.velocity = direction * speed;

        // Đã xóa phần tính toán angle và transform.rotation ở đây
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

            // Đã xóa phần cập nhật rotation khi nảy tường
        }
        else if (other.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            isReturning = true;

            // Giữ nguyên góc xoay 0 khi chạm đất
            transform.rotation = Quaternion.identity;

            spawner.ReturnBulletToQueue(this.gameObject);
        }
    }
}