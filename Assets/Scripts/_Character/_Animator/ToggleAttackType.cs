using UnityEngine;

namespace KrazyKatGames
{
    public class ToggleAttackType : StateMachineBehaviour
    {
        private CharacterManager character;
        [SerializeField] AttackType attackType;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }
            character.characterCombatManager.currentAttackType = attackType;
        }
    }
}