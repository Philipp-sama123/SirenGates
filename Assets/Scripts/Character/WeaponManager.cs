using UnityEngine;

namespace KrazyKatgames
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }
        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;

            meleeDamageCollider.light_Attack_01_Modifier = weapon.light_attack_01_modifier;
            meleeDamageCollider.light_Attack_02_Modifier = weapon.light_attack_02_modifier;
            meleeDamageCollider.light_Attack_03_Modifier = weapon.light_attack_03_modifier;
            meleeDamageCollider.light_Attack_04_Modifier = weapon.light_attack_04_modifier;

            meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
            meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;
            meleeDamageCollider.heavy_Attack_03_Modifier = weapon.heavy_Attack_03_Modifier;
            meleeDamageCollider.heavy_Attack_04_Modifier = weapon.heavy_Attack_04_Modifier;

            meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
            meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;
            
            meleeDamageCollider.run_Attack_01_Modifier = weapon.run_Attack_01_Modifier;
            meleeDamageCollider.roll_Attack_01_Modifier = weapon.roll_Attack_01_Modifier;
            meleeDamageCollider.backstep_Attack_01_Modifier = weapon.backstep_Attack_01_Modifier;
        }
    }
}