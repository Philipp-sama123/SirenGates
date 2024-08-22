
using UnityEngine;

namespace KrazyKatgames
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }


        public override void EnableCanDoCombo()
        {
            base.EnableCanDoCombo();

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                // Enable off hand combo
            }
        }
        public override void DisableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = false;
            }
            else
            {
                // Enable off hand combo
                // canComboWithOffHandWeapon = false; 
            }
        }
        
        protected virtual void OnAnimatorMove()
        {
            if (!player.applyRootMotion) return;

            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }
}