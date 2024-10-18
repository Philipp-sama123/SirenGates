using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KrazyKatGames
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new();

        [Header("Cloak Equipment")]
        [SerializeField] List<CloakWearableItem> cloakEquipment = new();

        [Header("Pants Equipment")]
        [SerializeField] List<PantsWearableItem> pantsEquipment = new();

        [Header("Outfit Equipment")]
        [SerializeField] List<OutfitWearableItem> outfitWearable = new();

        [Header("Underwear Equipment")]
        [SerializeField] List<UnderwearWearableItem> underwearEquipment = new();

        [Header("Hood Equipment")]
        [SerializeField] List<HoodWearableItem> hoodEquipment = new();

        [Header("Shoes And Gloves Equipment")]
        [SerializeField] List<ShoesAndGlovesWearableItem> shoesAndGlovesEquipment = new();

        //  A List of all the items in Game
        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            //  add all weapons to weapons list
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }
            //  add all headEquipment to weapons list
            foreach (var item in cloakEquipment)
            {
                items.Add(item);
            }
            //  add all handEquipment to weapons list
            foreach (var item in pantsEquipment)
            {
                items.Add(item);
            }
            //  add all bodyEquipment to weapons list
            foreach (var item in outfitWearable)
            {
                items.Add(item);
            }
            //  add all legEquipment to weapons list
            foreach (var item in underwearEquipment)
            {
                items.Add(item);
            }
            //  add all hoodEquipment to weapons list
            foreach (var item in hoodEquipment)
            {
                items.Add(item);
            }

            //  assign all of the items a unique ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
        public CloakWearableItem GetCloakEquipmentByID(int ID)
        {
            return cloakEquipment.FirstOrDefault(wearable => wearable.itemID == ID);
        }
        public PantsWearableItem GetPantsEquipmentByID(int ID)
        {
            return pantsEquipment.FirstOrDefault(wearable => wearable.itemID == ID);
        }
        public OutfitWearableItem GetOutfitEquipmentByID(int ID)
        {
            return outfitWearable.FirstOrDefault(wearable => wearable.itemID == ID);
        }
        public UnderwearWearableItem GetUnderwearEquipmentByID(int ID)
        {
            return underwearEquipment.FirstOrDefault(wearable => wearable.itemID == ID);
        }
        public HoodWearableItem GetHoodEquipmentByID(int ID)
        {
            return hoodEquipment.FirstOrDefault(wearable => wearable.itemID == ID);
        }
        public ShoesAndGlovesWearableItem GetShoesAndGlovesEquipmentByID(int ID)
        {
            return shoesAndGlovesEquipment.FirstOrDefault(wearable => wearable.itemID == ID);
        }
    }
}