using UnityEngine;

namespace KrazyKatgames
{
    public class WeaponItem : Item
    {
        [Header("Animator Controller Override")]
        public AnimatorOverrideController weaponAnimator;

        [Header("Model Instantiation")]
        public WeaponModelType weaponModelType;

        [Header("Weapon Model")]
        public GameObject weaponModel;
        
        [Header("Weapon Class")]
        public WeaponClass weaponClass;

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

        [Header("Weapon Blocking Absorptions")]
        public float physicalBaseDamageAbsorption = 50;
        public float fireBaseDamageAbsorption = 50;
        public float magicBaseDamageAbsorption = 50;
        public float lightningBaseDamageAbsorption = 50;
        public float holyBaseDamageAbsorption = 50;
        public float stability = 50; // reduces Stamina lost from blocking

        [Header("Weapon Poise")]
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]
        public float light_attack_01_modifier = 1.1f;
        public float light_attack_02_modifier = 1.2f;
        public float light_attack_03_modifier = 1.3f;
        public float light_attack_04_modifier = 1.5f;

        public float heavy_Attack_01_Modifier = 1.5f;
        public float heavy_Attack_02_Modifier = 2f;
        public float heavy_Attack_03_Modifier = 2.5f;
        public float heavy_Attack_04_Modifier = 5f;

        public float charge_Attack_01_Modifier = 3f;
        public float charge_Attack_02_Modifier = 5f;

        public float run_Attack_01_Modifier = 2.5f;
        public float roll_Attack_01_Modifier = 2.5f;
        public float backstep_Attack_01_Modifier = 2.5f;

        //  Crit Damage Modifier, etc

        [Header("Stamina Costs Modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        public float heavyAttackStaminaCostMultiplier = 1.2f;
        public float chargedAttackStaminaCostMultiplier = 1.5f;
        public float rollingAttackStaminaCostMultiplier = .5f;
        public float runningAttackStaminaCostMultiplier = .5f;
        public float backstepAttackStaminaCostMultiplier = .5f;


        //  ITEM BASED ACTIONS (RB, RT, LB, LT)
        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; // One Handed Right Bumper Action // ToDo: OH Left Mouse Button (?)
        public WeaponItemAction oh_RT_Action; // One Handed Right Trigger Action // ToDo: OH Right Mouse Button (?)

        public WeaponItemAction oh_LB_Action; // One Handed LEft Bumper Action //
        //  ASH OF WAR


        [Header("Whooshes")]
        public AudioClip[] whooshes;
        
        [Header("Blocking")]
        public AudioClip[] blocking;
    }
}