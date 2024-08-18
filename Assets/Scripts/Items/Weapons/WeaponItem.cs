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

        //  WEAPON GUARD ABSORPTIONS (BLOCKING POWER)

        [Header("Weapon Poise")]
        public float poiseDamage = 10;
        //  OFFENSIVE  POISE BONUS WHEN ATTACKING

        [Header("Attack Modifiers")]
        public float light_attack_01_modifier = 1.1f;

        //  LIGHT ATTACK MODIFIER
        //  HEAVY ATTACK MODIFIER
        //  CRITICAL DAMAGE MODIFIER ECT

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        //  RUNNING ATTACK STAMINA COST MODIFIER
        //  LIGHT ATTACK STAMINA COST MODIFIER
        //  HEAVY ATTACK STAMINA COST MODIFIER ECT


        //  ITEM BASED ACTIONS (RB, RT, LB, LT)
        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; // One Handed Right Bumper Action // ToDo: OH Left Mouse Button (?)
        //  ASH OF WAR

        //  BLOCKING SOUNDS
    }
}