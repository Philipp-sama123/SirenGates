using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private PlayerManager player;

        private List<Interactable> currentInteractableActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
                return;

            // If UI is not open, and we don't have a popup --> current interaction Message 
            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popupWindowIsOpen)
            {
                CheckForInteractable();
            }
        }
        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0);
                return;
            }

            // if we have an interactible action and not notified player --> do here 
            if (currentInteractableActions[0] != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopup(currentInteractableActions[0].interactableText);
            }
        }
        public void Interact()
        {
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();
            
            if (currentInteractableActions.Count == 0)
                return;
            
            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }
        private void RefreshInteractionList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                {
                    currentInteractableActions.RemoveAt(i);
                }
            }
        }
        public void AddInteractionToList(Interactable interactable)
        {
            RefreshInteractionList();

            if (!currentInteractableActions.Contains(interactable))
                currentInteractableActions.Add(interactable);
        }
        public void RemoveInteractionFromList(Interactable interactable)
        {
            if (currentInteractableActions.Contains(interactable))
                currentInteractableActions.Remove(interactable);

            RefreshInteractionList();
        }
    }
}