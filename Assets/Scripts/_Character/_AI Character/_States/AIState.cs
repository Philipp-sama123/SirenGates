using UnityEngine;

namespace KrazyKatgames
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            return this;
        }
        protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
        {
//            Debug.LogWarning(aiCharacter.name + " SwitchState to " + newState.name);
            ResetStateFlags(aiCharacter);
            return newState;
        }
        protected virtual void ResetStateFlags(AICharacterManager aiCharacter)
        {
        }
    }
}