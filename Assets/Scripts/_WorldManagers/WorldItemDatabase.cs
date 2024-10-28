using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KrazyKatGames
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        public GameObject pickUpItemPrefab;

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

        [Header("Ashes of War")]
        [SerializeField] List<AshOfWar> ashesOfWar = new();

        [Header("Spells")]
        [SerializeField] List<SpellItem> spells = new();

        [Header("Projectiles")]
        [SerializeField] List<RangedProjectileItem> projectiles = new();

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

            //  add all Weapons to weapons list
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }
            //  add all Cloak Equipment to weapons list
            foreach (var item in cloakEquipment)
            {
                items.Add(item);
            }
            //  add all Pant Equipment to weapons list
            foreach (var item in pantsEquipment)
            {
                items.Add(item);
            }
            //  add all Outfit Equipment to weapons list
            foreach (var item in outfitWearable)
            {
                items.Add(item);
            }
            //  add all Underwear Equipment to weapons list
            foreach (var item in underwearEquipment)
            {
                items.Add(item);
            }
            //  add all Hood Equipment to weapons list
            foreach (var item in hoodEquipment)
            {
                items.Add(item);
            }
            // add all ashes of war
            foreach (var item in ashesOfWar)
            {
                items.Add(item);
            }

            // add spells
            foreach (var item in spells)
            {
                items.Add(item);
            }

            // add projectiles
            foreach (var item in projectiles)
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
        public Item GetItemByID(int ID)
        {
            return items.FirstOrDefault(item => item.itemID == ID);
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
        public AshOfWar GetAshOfWarByID(int ID)
        {
            return ashesOfWar.FirstOrDefault(ashOfWar => ashOfWar.itemID == ID);
        }
        public SpellItem GetSpellByID(int ID)
        {
            return spells.FirstOrDefault(spell => spell.itemID == ID);
        }
        public RangedProjectileItem GetProjectileByID(int ID)
        {
            return projectiles.FirstOrDefault(projectile => projectile.itemID == ID);
        }
    }
}