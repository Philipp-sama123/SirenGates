using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatgames
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private MeleeWeaponDamageCollider meleeDamageCollider;

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
        }
    }
}