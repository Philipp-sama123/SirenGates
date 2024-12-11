using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Weapons")]
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;
        public WeaponItem currentTwoHandWeapon;

        [Header("Projectiles")]
        public RangedProjectileItem mainProjectile;
        public RangedProjectileItem secondaryProjectile;

        [Header("Armor")]
        public CloakWearableItem cloakWearable;
        public PantsWearableItem pantsWearable;
        public OutfitWearableItem outfitWearable;
        public UnderwearWearableItem underwearWearable;
        public HoodWearableItem hoodWearable;
        public ShoesAndGlovesWearableItem shoesAndGlovesWearable;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;
        public SpellItem currentSpell;

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            itemsInInventory.Remove(item);

            for (int i = itemsInInventory.Count - 1; i > -1; i--)
            {
                if (itemsInInventory[i] == null)
                {
                    itemsInInventory.RemoveAt(i);
                }
            }
            // ToDo: Add Server RPC that creates it for others when dropped (!)
        }
    }
}