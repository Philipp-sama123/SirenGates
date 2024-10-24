using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        public string attackAnimation;
        public bool isParryable = true;

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction; // The combo action of this attack action

        [Header("Action Values")]
        [SerializeField] AttackType attackType;
        public int attackWeight = 50;
        // attack can be repeated (!)
        public float actionRecoveryTime = 1.5f; //  The time before the character can make another attack after performing this one
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);
            aiCharacter.aiCharacterNetworkManager.isParryable.Value = isParryable;
        }
    }
}