using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class AIDurkCombatManager : AICharacterCombatManager
    {
        private AIDurkCharacterManager aiDurkManager;

        [Header("Damage Collider")]
        [SerializeField] DurkClubDamageCollider clubDamageCollider;
        [SerializeField] DurkStompCollider stompCollider;
        public float stompAttackAOERadius = 1.5f;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] int basePoiseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float attack03DamageModifier = 1.6f;
        [SerializeField]
        public float stompDamage = 25;

        [Header("VFX")]
        public GameObject durkImpactVFX;
        protected override void Awake()
        {
            base.Awake();
            aiDurkManager = GetComponent<AIDurkCharacterManager>();
        }
        public void SetAttack01Damage()
        {
            aiDurkManager.durkSoundFXManager.PlayAttackGruntSoundFX();
            clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            aiDurkManager.durkSoundFXManager.PlayAttackGruntSoundFX();
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;

        }

        public void SetAttack03Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack03DamageModifier;

        }

        public void OpenClubDamageCollider()
        {
            clubDamageCollider.EnableDamageCollider();
            aiDurkManager.durkSoundFXManager.PlaySoundFX(
                WorldSoundFXManager.instance.ChooseRandomSFXFromArray(aiDurkManager.durkSoundFXManager.clubWhooshes));
        }

        public void CloseClubDamageCollider()
        {
            clubDamageCollider.DisableDamageCollider();
        }

        public void ActivateDurkStomp()
        {
stompCollider.StompAttack();
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            //  PLAY A PIVOT ANIMATION DEPENDING ON VIEWABLE ANGLE OF TARGET
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }
        }
    }
}