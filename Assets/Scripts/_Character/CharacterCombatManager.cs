using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Previous Poise Damage Taken")]
        public float previousPoiseDamageTaken;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Attack Flags")]
        public bool canPerformRollingAttack = false;
        public bool canPerformBackstepAttack = false;
        public bool canBlock = true;
        public bool canBeBackStabbed = true;

        [Header("Critical Attack")]
        private Transform riposteReceiverTransform;
        private Transform backStabReceiverTransform;
        [SerializeField] private float criticalAttackDistanceCheck = 1f;
        [SerializeField] private int minMaxAngleToRiposte = 90;
        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    // tell the Network 
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
        // used to attempt riposte
        public virtual void AttemptCriticalAttack()
        {
            if (character.isPerformingAction)
                return;

            if (character.characterNetworkManager.currentStamina.Value >= 0)
            {
                RaycastHit[] hits = Physics.RaycastAll(
                    character.characterCombatManager.lockOnTransform.transform.position,
                    character.transform.TransformDirection(Vector3.forward),
                    criticalAttackDistanceCheck,
                    WorldUtilityManager.Instance.GetCharacterLayers()
                );

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];

                    CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

                    if (targetCharacter != null)
                    {
                        if (targetCharacter == character)
                            continue;

                        if (!WorldUtilityManager.Instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                            continue;

                        Vector3 directionFromCharacterToTarget = character.transform.position - targetCharacter.transform.position;
                        float targetViewableAngle =
                            Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                        if (targetCharacter.characterNetworkManager.isRipostable.Value)
                        {
                            if (targetViewableAngle >= -minMaxAngleToRiposte && targetViewableAngle <= minMaxAngleToRiposte)
                            {
                                AttemptRiposte(hit);
                                return;
                            }
                        }
                        // ToDo: Backstab check
                        if (targetCharacter.characterCombatManager.canBeBackStabbed)
                        {
                            if (targetViewableAngle <= 180 && targetViewableAngle >= 145) // ToDo: make variables for this (!)
                            {
                                AttemptBackStab(hit);
                                return;
                            }
                            if (targetViewableAngle >= -180 && targetViewableAngle <= -145)
                            {
                                AttemptBackStab(hit);
                                return;
                            }
                        }
                    }
                }
            }
        }
        public virtual void AttemptBackStab(RaycastHit hit)
        {
            Debug.LogWarning("Attempting BackStab");
        }
        public virtual void AttemptRiposte(RaycastHit hit)
        {
            Debug.LogWarning("Attempting Riposte");
        }

        public virtual void ApplyCriticalDamage()
        {
            character.characterEffectsManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.lockOnTransform.transform.position);
            character.characterSoundFXManager.PlayCriticalStrikeSoundFX();

            if (character.IsOwner)
            {
                character.characterNetworkManager.currentHealth.Value -= pendingCriticalDamage;
            }
        }
        public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 ripostePosition)
        {
            float timer = 0;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                if (riposteReceiverTransform == null)
                {
                    GameObject riposteTransformObject = new GameObject("Riposte Transform");
                    riposteTransformObject.transform.parent = transform;
                    riposteTransformObject.transform.position = Vector3.zero;
                    riposteReceiverTransform = riposteTransformObject.transform;
                }
                riposteReceiverTransform.localPosition = ripostePosition;
                enemyCharacter.transform.position = riposteReceiverTransform.position;

                transform.rotation = Quaternion.LookRotation(-enemyCharacter.transform.forward);
                yield return null;
            }
        }

        public IEnumerator ForceMoveEnemyCharacterToBackStabPosition(CharacterManager enemyCharacter, Vector3 backStabPosition)
        {
            float timer = 0;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;
                if (backStabReceiverTransform == null)
                {
                    GameObject backStabTransformObject = new GameObject("Backstab Transform");
                    backStabTransformObject.transform.parent = transform;
                    backStabTransformObject.transform.position = Vector3.zero;
                    backStabReceiverTransform = backStabTransformObject.transform;
                }
                backStabReceiverTransform.localPosition = backStabPosition;
                enemyCharacter.transform.position = backStabReceiverTransform.position;

                transform.rotation = Quaternion.LookRotation(enemyCharacter.transform.forward);
                yield return null;
            }
        }
        #region Animation Events
        public void EnableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;
        }
        public void DisableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = false;
        }
        public void EnableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = true;
        }
        public void DisableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = false;
        }      
        public void EnableIsRipostable()
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.isRipostable.Value = true;
            }
        }
        public void EnableCanDoRollingAttack()
        {
            canPerformRollingAttack = true;
        }
        public void DisableCanDoRollingAttack()
        {
            canPerformRollingAttack = false;
        }
        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }
        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }

        public virtual void DisableCanDoCombo()
        {
        }

        public virtual void EnableCanDoCombo()
        {
        }
        #endregion
        public virtual void CloseAllDamageColliders()
        {
            
        }
        // destroy things like "Arrow", "Spell WarmUp FX" when the Character is Poise broken
        public void CancelAllAttemptedActions()
        {
            character.characterNetworkManager.CancelAllAttemptedActionsServerRpc();
        }
    }
}