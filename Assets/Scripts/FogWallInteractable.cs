using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatgames
{
    public class FogWallInteractable : Interactable
    {
        [Header("Active")]
        public NetworkVariable<bool> isActive = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Fog")]
        [SerializeField] GameObject[] fogGameObjects;

        [Header("I.D")]
        public int fogWallID;
        [Header("Collision")]
        [SerializeField] Collider fogWallCollider;

        [Header("Sound")]
        private AudioSource fogWallAudioSource;
        [SerializeField] AudioClip fogWallSFX;
        protected override void Awake()
        {
            base.Awake();
            fogWallAudioSource = GetComponent<AudioSource>();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);


            // Face the fog wall 

            Quaternion targetRotation = Quaternion.LookRotation(transform.right);
            player.transform.rotation = targetRotation;

            // disable 
            AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
            player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.instance.AddFogWallToList(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }

        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (isActive.Value)
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(false);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
        {
            if (IsServer)
            {
                AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
            }
        }
        [ClientRpc]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX);

            if (player != null)
            {
                StartCoroutine(DisableCollisionForTime(player));
            }
        }
        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);
            yield return new WaitForSeconds(3);
            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
        }
    }
}