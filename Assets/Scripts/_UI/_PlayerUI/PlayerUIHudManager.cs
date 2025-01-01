using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] canvasGroups;

        [Header("Stat Bars")]
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar focusPointsBar;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image spellItemQuickSlotIcon;

        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;
        
        [Header("Crosshair")]
        public GameObject crosshair;

        public void ToggleHUD(bool status)
        {
            // ToDo: Fade in and out (!)
            if (status)
            {
                foreach (var canvasGroup in canvasGroups)
                {
                    canvasGroup.alpha = 1;
                }
            }
            else
            {
                foreach (var canvasGroup in canvasGroups)
                {
                    canvasGroup.alpha = 0;
                }
            }
        }
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
            focusPointsBar.gameObject.SetActive(false);
            focusPointsBar.gameObject.SetActive(true);
        }
        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }
        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }
        public void SetNewFocusPointsBarValue(int oldValue, int newValue)
        {
            focusPointsBar.SetStat(newValue);
        }
        public void SetMaxFocusPointsValue(int maxFocusPoints)
        {
            focusPointsBar.SetMaxStat(maxFocusPoints);
        }
        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is null!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }
        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            if (weapon == null)
            {
                Debug.Log("Item is null!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogWarning("Item has no Icon!");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
        public void SetSpellItemQuickSlotIcon(int spellID)
        {
            SpellItem spell = WorldItemDatabase.Instance.GetSpellByID(spellID);
            if (spell == null)
            {
                Debug.Log("spell is null!");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }
            if (spell.itemIcon == null)
            {
                Debug.LogWarning("spell has no Icon!");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }
            // Check if you meet item requirements (!)
            spellItemQuickSlotIcon.sprite = spell.itemIcon;
            spellItemQuickSlotIcon.enabled = true;
        }
    }
}