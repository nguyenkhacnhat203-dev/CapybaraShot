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

    private int initialHealth;

    void Start()
    {
        SaveInitialHealth();  
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

            UIManager.Instance.AddScore(initialHealth);

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

    public void SetHealth(int amount)
    {
        health = amount;
        SaveInitialHealth();   
        UpdateHealthUI();
    }

    void SaveInitialHealth()
    {
        initialHealth = health;
    }

    public int GetInitialHealth()
    {
        return initialHealth;
    }
}
