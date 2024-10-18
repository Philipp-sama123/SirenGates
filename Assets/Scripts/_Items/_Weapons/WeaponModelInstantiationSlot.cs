using System;
using UnityEngine;

namespace KrazyKatGames
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
            Debug.LogWarning(
                "WEAPON MODEL INSTANTIATION SLOT: PlaceWeaponModelInUnequippedSlot "
                + weaponModel.name
                + " Weapon Class "
                + weaponClass);

            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch (weaponClass)
            {
                case WeaponClass.Blade:
                    weaponModel.transform.localPosition = new Vector3();
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3();
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case WeaponClass.Shield:
                    weaponModel.transform.localPosition = new Vector3(0.1f,0.25f,0f);
                    weaponModel.transform.localRotation = Quaternion.Euler(320, 15, 40);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponClass), weaponClass, null);
            }
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;
            Debug.LogWarning("WEAPON MODEL INSTANTIATION SLOT: PlaceWeaponModelIntoSlot " + gameObject.name);
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}