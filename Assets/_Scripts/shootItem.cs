using UnityEngine;
using UnityEngine.UI;

public class ShootItem : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("UI Display (Optional)")]
    public Image iconImage;
    public Text weaponNameText;

    private void Start()
    {
        UpdateDisplay();
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

        if (iconImage != null && weaponData.projectileVisualPrefab != null)
        {
            SpriteRenderer spriteRenderer = weaponData.projectileVisualPrefab.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                iconImage.sprite = spriteRenderer.sprite;
                iconImage.color = Color.white;
            }
        }
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
}
