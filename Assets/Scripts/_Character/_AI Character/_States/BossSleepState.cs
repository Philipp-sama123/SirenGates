using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "A.I/States/Boss State Sleep")]
    public class BossSleepState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }
    }
}