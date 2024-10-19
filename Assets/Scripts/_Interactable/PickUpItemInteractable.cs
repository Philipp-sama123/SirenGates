using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("Creature loot Pick Up")]
        public NetworkVariable<int> itemID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> networkPosition =
            new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<ulong> droppingCreatureID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public bool trackDroppingCreaturesPosition = true;

        [Header("World Spawn Pick Up")]
        [SerializeField] int worldSpawnInteractableID;
        [SerializeField] bool hasBeenLooted = false;

        [Header("Drop SFX")]
        [SerializeField] AudioClip itemDropSFX;
        private AudioSource audioSource;
        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
        }
        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            itemID.OnValueChanged += OnItemIDChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged += OnDroppingCreatureIDChanged;

            if (pickUpType == ItemPickUpType.CharacterDrop)
                audioSource.PlayOneShot(itemDropSFX);
            
            if (!IsOwner)//for joining players(!)
            {
                OnItemIDChanged(0,itemID.Value);
                OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
                OnDroppingCreatureIDChanged(0, droppingCreatureID.Value);
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            itemID.OnValueChanged -= OnItemIDChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged -= OnDroppingCreatureIDChanged;
        }
        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            base.Interact(player);
            
            // 1. Play a SFX
            player.playerSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);
            // 2. Play an Animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Pick_Up_Item_01", true);
            // 3. Add Item to inventory
            player.playerInventoryManager.AddItemToInventory(item);
            // 4. Display a UI Pop Up 
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1); // TODO:
            // 5. Save Loot status if its a World Spawn
            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(worldSpawnInteractableID);
                }

                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, true);
            }
            // 6. Destroy GameObject
            DestroyThisNetworkObjectServerRpc();
        }
        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false); // disables items for clients ... not sure if this makes so much sense but its rlden ring logic
                return;
            }

            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, false);
            }
            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpawnInteractableID];

            if (hasBeenLooted)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
        protected void OnItemIDChanged(int oldValue, int newValue)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            item = WorldItemDatabase.Instance.GetItemByID(itemID.Value);
        }
        protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            transform.position = networkPosition.Value;
        }
        protected void OnDroppingCreatureIDChanged(ulong oldValue, ulong newValue)
        {
            if (trackDroppingCreaturesPosition)
            {
                StartCoroutine(TrackDroppingCreaturesPosition());
            }
        }
        protected IEnumerator TrackDroppingCreaturesPosition()
        {
            AICharacterManager droppingCreature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[droppingCreatureID.Value].gameObject.
                GetComponent<AICharacterManager>();

            bool trackCreature = droppingCreature != null;

            if (trackCreature)
            {
                while (gameObject.activeInHierarchy)
                {
                    transform.position = droppingCreature.characterCombatManager.lockOnTransform.position;
                    yield return null;
                }
            }
            yield return null;
        }
        [ServerRpc(RequireOwnership = false)]
        protected void DestroyThisNetworkObjectServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
        
        
    }
}