using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager character;
        [Header("Is Active")]
        public NetworkVariable<bool> isActive = new(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Position")]
        public NetworkVariable<bool> isMoving = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> networkPosition =
            new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation =
            new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Target")]
        public NetworkVariable<ulong> currentTargetNetworkObjectID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Flags")]
        public NetworkVariable<bool> isBlocking = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParrying = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParryable = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAttacking = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isInvulnerable = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isLockedOn = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSprinting = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingAttack = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isRipostable = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isBeingCriticallyDamaged =
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Resources")]
        public NetworkVariable<float> currentStamina = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentHealth = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> endurance = new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> vitality = new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> strength = new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Modifiers")]
        public NetworkVariable<int> strengthModifier = new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void CheckHP(int oldValue, int newValue)
        {
            if (currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }
            // Avoid Overhealing(!)
            if (character.IsOwner)
            {
                if (currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }
        public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsDead", character.isDead.Value);
        }
        public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            if (!IsOwner)
            {
                character.characterCombatManager.currentTarget =
                    NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
            }
        }
        public void OnIsLockedOnChanged(bool old, bool isLockedOn)
        {
            if (!isLockedOn)
            {
                character.characterCombatManager.currentTarget = null;
            }
        }
        public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsChargingAttack", isChargingAttack.Value);
        }
        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsMoving", isMoving.Value);
        }
        public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            gameObject.SetActive(isActive.Value);
        }
        public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsBlocking", isBlocking.Value);
        }

        #region Action Animations
        // Is a function called from a client, to the Server
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientId, animationId, applyRootMotion);
            }
        }

        // A client RPC is sent to all clients present, from the Server
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            // make sure to not run the function on the character who sent it ( so it doesnt get played twice) 
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationId, applyRootMotion);
            }
        }
        private void PerformActionAnimationFromServer(string animationId, bool applyRootMotion)
        {
            character.animator.CrossFade(animationId, 0.2f);
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        }
        #endregion

        #region Attack Animations
        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayAttackActionAnimationForAllClientsClientRpc(clientId, animationId, applyRootMotion);
            }
        }
        [ClientRpc]
        public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformAttackActionAnimationFromServer(animationId, applyRootMotion);
            }
        }

        private void PerformAttackActionAnimationFromServer(string animationId, bool applyRootMotion)
        {
            character.animator.CrossFade(animationId, 0.2f);
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        }
        [ServerRpc]
        public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayInstantActionAnimationForAllClientsClientRpc(clientId, animationId, applyRootMotion);
            }
        }
        [ClientRpc]
        public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientId, string animationId, bool applyRootMotion)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                PerformInstantActionAnimationFromServer(animationId, applyRootMotion);
            }
        }
        private void PerformInstantActionAnimationFromServer(string animationId, bool applyRootMotion)
        {
            character.animator.Play(animationId);
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        }
        #endregion

        #region Damage
        // Character Damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
        )
        {
            if (IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage,
                    holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
            }
        }
        [ClientRpc]
        public void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
        )
        {
            ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, holyDamage,
                poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
        }
        public void ProcessCharacterDamageFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
        )
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.
                GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.
                GetComponent<CharacterManager>();
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        // Riposte
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfRiposteServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            if (IsServer)
            {
                NotifyTheServerOfRiposteClientRpc(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage,
                    magicDamage, fireDamage,
                    holyDamage, poiseDamage);
            }
        }
        [ClientRpc]
        public void NotifyTheServerOfRiposteClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            ProcessRiposteFromServer(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage,
                fireDamage, holyDamage,
                poiseDamage);
        }
        public void ProcessRiposteFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.
                GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.
                GetComponent<CharacterManager>();
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            
            if (damagedCharacter.IsOwner)
                damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);

            // Move the enemy to a proper riposte position
            StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition(
                characterCausingDamage,
                WorldUtilityManager.Instance.GetRipostingPositionBasedOnWeaponClass(weapon.weaponClass))
            );
        }

        // BackStab
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfBackStabServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            if (IsServer)
            {
                NotifyTheServerOfBackStabClientRpc(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage,
                    magicDamage, fireDamage,
                    holyDamage, poiseDamage);
            }
        }
        [ClientRpc]
        public void NotifyTheServerOfBackStabClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            ProcessBackStabFromServer(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage,
                fireDamage, holyDamage,
                poiseDamage);
        }
        public void ProcessBackStabFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage
        )
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.
                GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.
                GetComponent<CharacterManager>();
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);

            // Move the enemy to a proper backstab position
            // StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToBackStabPosition(
            //     characterCausingDamage,
            //     WorldUtilityManager.Instance.GetBackStabPositionBasedOnWeaponClass(weapon.weaponClass))
            // );
        }
        // Parry
        [ServerRpc(RequireOwnership = false)]
        public void NotifyServerOfParryServerRpc(ulong parriedClientID)
        {
            if (IsServer)
            {
                NotifyServerOfParryClientRpc(parriedClientID);
            }
        }
        [ClientRpc]
        protected void NotifyServerOfParryClientRpc(ulong parriedClientID)
        {
            ProcessParryFromServer(parriedClientID);
        }

        protected void ProcessParryFromServer(ulong parriedClientID)
        {
            CharacterManager parriedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parriedClientID].gameObject.
                GetComponent<CharacterManager>();

            if (parriedCharacter == null)
                return;
            // close all damage collders
            if (parriedCharacter.IsOwner)
            {
                parriedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parried_01", true);
            }
        }
        #endregion
    }
}