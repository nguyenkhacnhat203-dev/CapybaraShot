using UnityEngine;
using TMPro;
using DG.Tweening;

public class Block : MonoBehaviour
{
    [Header("Máu")]
    public int health = 10;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    private Tween moveTween;

    void Start()
    {
        UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthUI();

        if (health <= 0)
        {
            moveTween?.Kill();
            Destroy(gameObject);
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = health.ToString();
    }

    public void MoveDownTween(float distance, float duration)
    {
        moveTween?.Kill(); 
        moveTween = transform.DOMoveY(transform.position.y - distance, duration)
                              .SetEase(Ease.OutQuad);
    }
}
