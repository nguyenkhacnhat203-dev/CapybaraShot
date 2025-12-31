using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("Score UI")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI Text_highestScore;

    [Header("Bullet UI")]
    public TextMeshProUGUI Text_Count_Bullet;

    private int score = 0;
    private int highestScore = 0;

    private const string HIGHEST_SCORE_KEY = "HighestScore";

    private void Start()
    {
        LoadHighestScore();
        UpdateScoreUI();
        UpdateHighestScoreUI();
    }

    // ================= SCORE =================
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score > highestScore)
        {
            highestScore = score;
            SaveHighestScore();
            UpdateHighestScoreUI();
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (ScoreText != null)
            ScoreText.text = "Score : " + score;
    }

    void UpdateHighestScoreUI()
    {
        if (Text_highestScore != null)
            Text_highestScore.text = highestScore.ToString();
    }

    void SaveHighestScore()
    {
        PlayerPrefs.SetInt(HIGHEST_SCORE_KEY, highestScore);
        PlayerPrefs.Save();
    }

    void LoadHighestScore()
    {
        highestScore = PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);
    }

    // ================= BULLET =================
    // ❗ Chỉ hiển thị TỔNG số đạn sở hữu
    public void UpdateBulletCount(int totalBullet)
    {
        if (Text_Count_Bullet != null)
            Text_Count_Bullet.text = totalBullet.ToString();
    }
}
