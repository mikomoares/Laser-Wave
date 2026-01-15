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
            print("Continue Button Listener Added");
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
        print("Game Started");  
        gameStarted = true;

        if (storePanel != null)
        {
            storePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        AudioManager.StartMusic();
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
}
