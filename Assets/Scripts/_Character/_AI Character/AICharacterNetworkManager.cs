using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        private AICharacterManager aiCharacter;
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>(); 
        }
        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);
            
            aiCharacter.aiCharacterInventoryManager.DropItem();
        }
    }
}