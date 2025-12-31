using System.Collections.Generic;
using UnityEngine;

public class BlockTheme : MonoBehaviour
{
    [Header("Themes")]
    public List<Sprite> Theme1;
    public List<Sprite> Theme2;
    public List<Sprite> Theme3;
    public List<Sprite> Theme4;

    private List<Sprite> currentTheme; 
    private SpriteRenderer spriteRenderer;
    private bool isHit = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RandomTheme();
    }

    void RandomTheme()
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                currentTheme = Theme1;
                break;
            case 1:
                currentTheme = Theme2;
                break;
            case 2:
                currentTheme = Theme3;
                break;
            case 3:
                currentTheme = Theme4;
                break;
        }

        if (currentTheme != null && currentTheme.Count > 0)
        {
            spriteRenderer.sprite = currentTheme[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !isHit)
        {
            isHit = true;
            ChangeToHitSprite();
        }
    }




    void ChangeToHitSprite()
    {
        if (currentTheme != null && currentTheme.Count > 1)
        {
            spriteRenderer.sprite = currentTheme[1];
        }
    }
}
