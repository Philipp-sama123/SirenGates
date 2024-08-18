using KrazyKatgames;
using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }
            Debug.Log("The Action has fired!");
        }
    }
}