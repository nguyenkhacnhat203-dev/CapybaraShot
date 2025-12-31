using DG.Tweening;
using UnityEngine;

public class Capypara_AddBulet : MonoBehaviour
{
    private Tween moveTween;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletSpawner spawner = FindFirstObjectByType<BulletSpawner>();
            if (spawner != null)
            {
                spawner.AddBullet();
            }

            Destroy(gameObject);
        }
    }


    public void MoveDownTween(float distance, float duration)
    {
        moveTween?.Kill();
        moveTween = transform.DOMoveY(transform.position.y - distance, duration)
                              .SetEase(Ease.OutQuad);
    }

}