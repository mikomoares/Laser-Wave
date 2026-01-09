using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [Header("Music Settings")]
    public float bpm = 100f;
    public AudioSource musicSource;

    [Header("Beat Events")]
    public UnityEvent onBeat;

    private float beatInterval;         
    private double nextBeatDspTime;    
    private int currentBeat = 0;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        beatInterval = 60f / bpm;
    }

    private void Update()
    {
        // 🔹 Detecta automaticamente quando a música começa
        if (!isPlaying && musicSource != null && musicSource.isPlaying)
        {
            StartBeats();
        }

        if (!isPlaying)
            return;

        // 🔥 Beat sincronizado com o áudio real
        if (AudioSettings.dspTime >= nextBeatDspTime)
        {
            OnBeat();
            nextBeatDspTime += beatInterval;
        }
    }

    private void OnBeat()
    {
        currentBeat++;
        onBeat?.Invoke();

        Debug.Log($"Beat {currentBeat}");
    }

    public void StartBeats()
    {
        isPlaying = true;
        currentBeat = 0;

        // Começa exatamente no tempo atual do áudio
        nextBeatDspTime = AudioSettings.dspTime;

        Debug.Log("BeatManager started");
    }

    public void StopBeats()
    {
        isPlaying = false;
    }

    public int GetCurrentBeat()
    {
        return currentBeat;
    }

    public float GetBeatInterval()
    {
        return beatInterval;
    }

    public bool ShouldFireOnBeat(int beatDivisor, int offset = 0)
    {
        if (beatDivisor <= 0) return false;
        return (currentBeat + offset) % beatDivisor == 0;
    }
}
