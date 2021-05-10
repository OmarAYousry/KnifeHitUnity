using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [SerializeField]
    private KnifeBehaviour knifePrefab = null;
    [SerializeField]
    private int knivesLeft = 7;
    [SerializeField]
    [Range(0f, 1.25f)]
    private float lodgeFactor = 1.0f;
    [SerializeField]
    private int scorePerKnife = 25;
    [SerializeField]
    private int scorePerApple = 100;

    private KnifeBehaviour currentKnifeBehaviour = null;

    public delegate void ScoreIncrementHandler(int scoreIncrease);
    public event ScoreIncrementHandler OnScoreIncrease;    
    
    public delegate void knifeCountHandler(int newKnifeCount);
    public event knifeCountHandler OnKnifeCountChange;

    public delegate void GameOverHandler();
    public event GameOverHandler OnGameOver;

    private void Start()
    {
        GenerateNewKnife();
    }

    public void LaunchCurrentKnife()
    {
        currentKnifeBehaviour.Launch();
    }

    public void GenerateNewKnife()
    {
        if (GameController.IsQuitting)
            return;

        currentKnifeBehaviour = Instantiate(knifePrefab);
        currentKnifeBehaviour.Init(knifeController: this, lodgeFactor);
    }

    public void lessknives()
    {
        knivesLeft--;
        OnKnifeCountChange?.Invoke(knivesLeft);
    }

    public void AddNewKnifeScore()
    {
        OnScoreIncrease?.Invoke(scoreIncrease: scorePerKnife);
    }

    public void AddNewAppleScore()
    {
        OnScoreIncrease?.Invoke(scoreIncrease: scorePerApple);
    }
    
    public void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }
}
