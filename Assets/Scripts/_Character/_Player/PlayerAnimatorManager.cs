
using UnityEngine;
// ToDo: Clean up the animation states of the player -> DEFAULT 1H Locomotion (!) not unarmed (!)
namespace KrazyKatGames
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        protected virtual void OnAnimatorMove()
        {
            if (!player.playerAnimatorManager.applyRootMotion) return;

            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }
}