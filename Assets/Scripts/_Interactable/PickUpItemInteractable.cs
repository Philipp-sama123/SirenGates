using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("World Spawn Pick Up")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int itemID;
        [SerializeField] bool hasBeenLooted = false;

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            player.playerSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);

            player.playerInventoryManager.AddItemToInventory(item);

            // Display a UI Pop Up 
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1); // TODO:

            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);
                }

                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);
            }
            Destroy(gameObject);
        }
        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false); // disables items for clients ... not sure if this makes so much sense but its rlden ring logic
                return;
            }

            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }
            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

            if (hasBeenLooted)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }
}