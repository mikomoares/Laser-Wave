using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject storePanel;
    public Button continueButton;

    [Header("Game Systems")]
    public BeatManager beatManager;
    public GM enemySpawner;
    public PlayerController playerController;

    [Header("State")]
    public bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        if (storePanel != null)
        {
            storePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void OnContinueClicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameStarted = true;

        if (storePanel != null)
        {
            storePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        if (beatManager != null && beatManager.musicSource != null)
        {
            AudioManager.StartMusic();
            // beatManager.musicSource.isPlaying = true;
        }
        else
        {
            AudioManager.StartMusic();
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
}
