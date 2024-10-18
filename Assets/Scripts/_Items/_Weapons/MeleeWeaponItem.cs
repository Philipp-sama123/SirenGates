using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        public float riposte_Attack_01_Modifier = 2.5f;
        //  WEAPON "DEFLECTION" (If the weapon will bounce off another weapon when it is being guarded against)
        //  CAN BE BUFFED
    }
}