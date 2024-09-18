using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatgames
{
    public class Interactable : NetworkBehaviour
    {
        // What are interactables (?)
        public string interactableText;

        [SerializeField] protected Collider interactableCollider;
        [SerializeField] protected bool hostOnlyInteractable = true;

        protected virtual void Awake()
        {
            // 
            if (interactableCollider == null)
                interactableCollider = GetComponent<Collider>();
        }
        protected virtual void Start()
        {
        }

        public virtual void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            Debug.Log("YOU HAVE INTERACTED!");

            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                player.playerInteractionManager.AddInteractionToList(this);
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();
            }
        }
    }
}