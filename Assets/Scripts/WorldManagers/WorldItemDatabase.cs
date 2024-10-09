using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KrazyKatgames
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new();

        [Header("Head Equipment")]
        [SerializeField] List<HeadEquipmentItem> headEquipment = new();

        [Header("Hand Equipment")]
        [SerializeField] List<HandEquipmentItem> handEquipment = new();

        [Header("Body Equipment")]
        [SerializeField] List<BodyEquipmentItem> bodyEquipment = new();

        [Header("Leg Equipment")]
        [SerializeField] List<LegEquipmentItem> legEquipment = new();

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
            foreach (var item in headEquipment)
            {
                items.Add(item);
            }
            //  add all handEquipment to weapons list
            foreach (var item in handEquipment)
            {
                items.Add(item);
            }
            //  add all bodyEquipment to weapons list
            foreach (var item in bodyEquipment)
            {
                items.Add(item);
            }
            //  add all legEquipment to weapons list
            foreach (var item in legEquipment)
            {
                items.Add(item);
            }

            //  assign all of the items a unique ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
        public HeadEquipmentItem GetHeadEquipmentByID(int ID)
        {
            return headEquipment.FirstOrDefault(weapon => weapon.itemID == ID);
        }
        public HandEquipmentItem GetHandEquipmentByID(int ID)
        {
            return handEquipment.FirstOrDefault(weapon => weapon.itemID == ID);
        }
        public BodyEquipmentItem GetBodyEquipmentByID(int ID)
        {
            return bodyEquipment.FirstOrDefault(weapon => weapon.itemID == ID);
        }
        public LegEquipmentItem GetLegEquipmentByID(int ID)
        {
            return legEquipment.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }
}