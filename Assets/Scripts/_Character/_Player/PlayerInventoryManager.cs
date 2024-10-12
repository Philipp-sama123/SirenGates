using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatgames
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Weapons")]
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;
        public WeaponItem currentTwoHandWeapon;

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

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory()
        {
            // ToDo: Add Server RPC that creates it for others when dropped (!)
        }

    }
}