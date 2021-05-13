using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    KnifeController knifeController = null;

    [SerializeField]
    GameObject PlaystorePopup = null;

    [SerializeField]
    private Text scoreText = null;
    private int score = 0;

    [SerializeField]
    private Text knifeCountText = null;

    [SerializeField]
    private Text storePopupText = null;

    public static bool IsQuitting { get; private set; } = false;

    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);

    void Start()
    {
        knifeController.OnScoreIncrease += IncrementScore;
        knifeController.OnGameOver += EndGame;
        knifeController.OnKnifeCountChange += adjustKnifeCountStatus;
    }

    public void IncrementScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = $"Score: {score}";
    }

    public void adjustKnifeCountStatus(int newKnifeCount)
    {
        knifeCountText.text = $"x{newKnifeCount}";
        if (newKnifeCount <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0.0f;
        storePopupText.text = $"You scored {score} points! To continue playing, get the app for free now!";
        PlaystorePopup.SetActive(true);
    }

    public void OpenDownloadLink()
    {
        string url = "https://play.google.com/store/apps/details?id=com.ketchapp.knifehit";
#if !UNITY_EDITOR && UNITY_WEBGL
                OpenNewTab(url);
#else
        Application.OpenURL(url);
#endif
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }
}
