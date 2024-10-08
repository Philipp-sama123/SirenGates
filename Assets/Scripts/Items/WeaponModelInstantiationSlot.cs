using System;
using UnityEngine;

namespace KrazyKatgames
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponSlot;
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
        {
            // TODO: Move Weapon on Back closer or move outward depending on chest equipment

            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;
            
            switch (weaponClass)
            {
                case WeaponClass.Blade:
                    weaponModel.transform.localPosition = new Vector3(0.065f, 0f, -0.05f);
                    weaponModel.transform.localRotation = Quaternion.Euler(195, 90, -0.25f);
                    break;
                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3(0.065f, 0f, -0.05f);
                    weaponModel.transform.localRotation = Quaternion.Euler(195, 90, -0.25f);
                    break;
                case WeaponClass.MediumShield:
                    weaponModel.transform.localPosition = new Vector3(0.065f, 0f, -0.05f);
                    weaponModel.transform.localRotation = Quaternion.Euler(195, 90, -0.25f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponClass), weaponClass, null);
            }
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}