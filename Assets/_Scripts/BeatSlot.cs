using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeatSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("Slot Configuration")]
    [Tooltip("Which beat slot is this? (0-7)")]
    [Range(0, 7)]
    public int slotIndex = 0;

    [Header("UI References")]
    public Image weaponIconImage;
    public Text weaponNameText;
    public Image backgroundImage;

    [Header("Visual Feedback")]
    public Color emptyColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public Color filledColor = new Color(1f, 1f, 1f, 1f);
    public Color hoverColor = new Color(0.8f, 0.8f, 1f, 1f);

    private WeaponData currentWeapon;
    private PlayerWeaponSystem weaponSystem;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            weaponSystem = player.GetComponent<PlayerWeaponSystem>();
        }

        UpdateVisuals();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        
        if (droppedObject != null)
        {
            ShootItem shootItem = droppedObject.GetComponent<ShootItem>();
            if (shootItem != null && shootItem.weaponData != null)
            {
                AssignWeapon(shootItem.weaponData);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClearSlot();
        }
    }

    public void AssignWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;

        if (weaponSystem != null)
        {
            weaponSystem.SetBeatSlot(slotIndex, weapon);
        }

        UpdateVisuals();
    }

    public void ClearSlot()
    {
        currentWeapon = null;

        if (weaponSystem != null)
        {
            weaponSystem.ClearBeatSlot(slotIndex);
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (currentWeapon != null)
        {
            if (weaponNameText != null)
            {
                weaponNameText.text = currentWeapon.weaponName;
            }

            if (weaponIconImage != null && currentWeapon.weaponIcon != null)
            {
                weaponIconImage.sprite = currentWeapon.weaponIcon;
                weaponIconImage.color = currentWeapon.color;
                weaponIconImage.enabled = true;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = filledColor;
            }
        }
        else
        {
            if (weaponNameText != null)
            {
                weaponNameText.text = $"Slot {slotIndex}";
            }

            if (weaponIconImage != null)
            {
                weaponIconImage.enabled = false;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = emptyColor;
            }
        }
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
