using UnityEngine;

namespace KrazyKatgames
{
    public class WeaponItem : Item
    {
        //  ANIMATOR CONTROLLER OVERRIDE (Change attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        //  Weapon Guard Absorption (Blocking power)

        [Header("Weapon Poise")]
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]
        public float light_attack_01_modifier = 1.1f;
        public float heavy_Attack_01_Modifier = 1.5f;
        public float charge_Attack_01_Modifier = 3f;

        //  Crit Damage Modifier, etc

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        //  RUNNING ATTACK STAMINA COST MODIFIER
        //  LIGHT ATTACK STAMINA COST MODIFIER
        //  HEAVY ATTACK STAMINA COST MODIFIER ECT


        //  ITEM BASED ACTIONS (RB, RT, LB, LT)
        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; // One Handed Right Bumper Action // ToDo: OH Left Mouse Button (?)
        public WeaponItemAction oh_RT_Action; // One Handed Right Trigger Action // ToDo: OH Right Mouse Button (?)
        //  ASH OF WAR

        //  ToDo: blocking sfx
    }
}