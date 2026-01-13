using UnityEngine;
using UnityEngine.UI;

public class SlotLoopManager : MonoBehaviour
{
    public Image[] slotImages;
    public Sprite selectedSlotSprite, regularSlotSprite;

    void Start()
    {
        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.onBeat.AddListener(OnBeat);
        }
    }

    void OnBeat()
    {
        UpdateSlots(BeatManager.Instance.GetCurrentLoopBeat());
    }

    public void UpdateSlots(int currentBeat)
    {
        int loopLength = BeatManager.Instance.loopLength;

        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].sprite = (i == currentBeat) ? selectedSlotSprite : regularSlotSprite;
        }
    }
}
