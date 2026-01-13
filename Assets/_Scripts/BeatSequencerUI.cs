using UnityEngine;

public class BeatSequencerUI : MonoBehaviour
{
    [Header("Beat Slots")]
    [Tooltip("Assign all 8 beat slot UI elements in order (0-7)")]
    public BeatSlot[] beatSlots = new BeatSlot[8];

    [Header("References")]
    public PlayerWeaponSystem playerWeaponSystem;

    private void Start()
    {
        if (playerWeaponSystem == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerWeaponSystem = player.GetComponent<PlayerWeaponSystem>();
            }
        }

        InitializeSlots();
        LoadCurrentBeatLoop();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < beatSlots.Length && i < 8; i++)
        {
            if (beatSlots[i] != null)
            {
                beatSlots[i].slotIndex = i;
            }
        }
    }

    private void LoadCurrentBeatLoop()
    {
        if (playerWeaponSystem == null)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            if (i < beatSlots.Length && beatSlots[i] != null)
            {
                WeaponData weapon = playerWeaponSystem.GetBeatSlot(i);
                if (weapon != null)
                {
                    beatSlots[i].AssignWeapon(weapon);
                }
            }
        }
    }

    public void ClearAllSlots()
    {
        foreach (BeatSlot slot in beatSlots)
        {
            if (slot != null)
            {
                slot.ClearSlot();
            }
        }
    }

    public void RandomizeSlots(WeaponData[] availableWeapons)
    {
        if (availableWeapons == null || availableWeapons.Length == 0)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            if (Random.value > 0.3f)
            {
                WeaponData randomWeapon = availableWeapons[Random.Range(0, availableWeapons.Length)];
                if (i < beatSlots.Length && beatSlots[i] != null)
                {
                    beatSlots[i].AssignWeapon(randomWeapon);
                }
            }
        }
    }
}
