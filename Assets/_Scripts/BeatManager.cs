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
    public int loopLength = 8;
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
        if (!isPlaying && musicSource != null && musicSource.isPlaying)
        {
            Debug.Log("Certo");
            StartBeats();
        }

        if (!isPlaying)
            return;

        if (AudioSettings.dspTime >= nextBeatDspTime)
        {
            Debug.Log("Errado");
            OnBeat();
            nextBeatDspTime += beatInterval;
        }
    }

    private void OnBeat()
    {

        currentBeat++;
        onBeat?.Invoke();

        Debug.Log($"Beat {GetCurrentLoopBeat()}");
    }

    public void StartBeats()
    {
        isPlaying = true;
        currentBeat = 0;

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

    public int GetCurrentLoopBeat()
    {
        return currentBeat % loopLength;
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
