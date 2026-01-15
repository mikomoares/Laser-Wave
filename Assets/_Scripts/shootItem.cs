using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ShootItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler

{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("UI Display (Optional)")]
    public Image iconImage;
    public Text weaponNameText;
    public Color hoverColor = new Color(0.8f, 0.8f, 1f, 1f);
    Color originalColor;


    private void Start()
    {
        UpdateDisplay();
        originalColor = iconImage.color;
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);
        hoverColor = Color.HSVToRGB(h, s, v * .85f);
        hoverColor.a = originalColor.a;
    }

    public void UpdateDisplay()
    {
        if (weaponData == null)
        {
            return;
        }

        if (weaponNameText != null)
        {
            weaponNameText.text = weaponData.weaponName;
        }

        if (iconImage != null && weaponData.weaponIcon != null)
        {
            iconImage.sprite = weaponData.weaponIcon;
            iconImage.color = weaponData.color;
        }
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iconImage != null)
            iconImage.color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (iconImage != null)
            iconImage.color = originalColor;
    }
}
