using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatgames
{
    public class SiteOfGraceInteractable : Interactable
    {
        [Header("Site Of Grace Info")]
        [SerializeField] int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Vfx")]
        [SerializeField] GameObject activatedParticles;
        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site Of Grace";
        [SerializeField] string activatedInteractionText = "Rest";

        protected override void Start()
        {
            base.Start();

            if (IsOwner)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
                }
                else
                {
                    isActivated.Value = false;
                }
            }
            if (isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                OnIsActivatedChanged(false, isActivated.Value);
            }
            isActivated.OnValueChanged += OnIsActivatedChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }

        private void RestoreSiteOfGrace(PlayerManager player)
        {
            isActivated.Value = true;

            if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);

            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);
            // Hide Weapon Models (!)
            PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("SITE OF GRACE RESTORED");

            StartCoroutine(WaitForAnimationAndPopupThenRestoreCollider());
        }
        private IEnumerator WaitForAnimationAndPopupThenRestoreCollider()
        {
            yield return new WaitForSeconds(5);
            interactableCollider.enabled = true;
        }
        private void RestAtSiteOfGrace(PlayerManager player)
        {
            WorldAIManager.instance.ResetAllCharacters();
            
            player.playerAnimatorManager.PlayTargetActionAnimation("Rest_Site_Of_Grace_01", true);

            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value; 
            player.playerNetworkManager.maxStamina.Value = player.playerNetworkManager.maxStamina.Value; 
            
            StartCoroutine(WaitForAnimationAndPopupThenRestoreCollider()); // Temporary re enable after some time so you can redo it !

            // Refill Flasks
            // Update / Force Move Quest Characters
            // Reset Monsters 
        }
        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if (isActivated.Value)
            {
                activatedParticles.SetActive(true);
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }
    }
}