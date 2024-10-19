using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KrazyKatGames
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        private AICharacterManager aiCharacter;

        [Header("Loot Chance in [%]")]
        public int dropItemChance = 25;
        public Item[] droppableItems;

        public override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void DropItem()
        {
            if (!aiCharacter.IsOwner)
                return;

            bool willDropItem = false;

            int itemChanceRoll = Random.Range(0, 100);

            if (itemChanceRoll <= dropItemChance)
                willDropItem = true;

            if (!willDropItem)
                return;

            Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];

            if (generatedItem == null)
                return;

            GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.Instance.pickUpItemPrefab);
            PickUpItemInteractable pickUpInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();

            itemPickUpInteractableGameObject.GetComponent<NetworkObject>().Spawn();

            pickUpInteractable.itemID.Value = generatedItem.itemID;
            pickUpInteractable.networkPosition.Value = transform.position;
            pickUpInteractable.droppingCreatureID.Value = aiCharacter.NetworkObjectId;
        }
    }
}