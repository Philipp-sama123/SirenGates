using UnityEngine;

namespace KrazyKatGames
{
    [System.Serializable]
    //  SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD SERIALIZABLE
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        // QUESTION: WHY NOT USE A VECTOR3?
        // ANSWER: WE CAN ONLY SAVE DATA FROM "BASIC" VARIABLE TYPES (Float, Int, String, Bool, ect)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        public int currentFocusPoints;

        [Header("Character Stats")]
        public int endurance = 1;
        public int vitality = 1;
        public int mind = 1;

        [Header("Sites Of Grace")]
        public SerializableDictionary<int, bool> sitesOfGrace; // int is the id, bool the activation status

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened;
        public SerializableDictionary<int, bool> bossesDefeated;

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted; // world items and looted status

        [Header("Equipment")]
        public int underwearEquipment;
        public int maskEquipment;
        public int pantsEquipment;
        public int outfitEquipment;
        public int hoodEquipment;
        public int cloakEquipment;
        public int bagpackEquipment;
        public int shoesAndGlovesEquipment;

        [Header("Weapons")]
        public int rightWeaponIndex;
        public int rightWeapon01;
        public int rightWeapon02;
        public int rightWeapon03;

        public int leftWeaponIndex;
        public int leftWeapon01;
        public int leftWeapon02;
        public int leftWeapon03;

        public int currentSpell;

        // also a list maybe ... 
        //  public List<int> rightWeapons; 
        //  public List<int> leftWeapons; 
        public CharacterSaveData()
        {
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            sitesOfGrace = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();
        }
    }
}