using UnityEngine;

namespace KrazyKatGames
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0; // break in its subtypes (Slashing, Piercing, Striking) 
        // ToDo: look at Baldur's Gate 3 (!)

        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        public int finalDamageDealt = 0;

        [Header("Poise")] // Also a future ToDo: 
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        // ToDo: Build Ups, like Bleeding, 

        [Header("Animations")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SoundFx")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // used on top of regular SFX if there is elemental Damage present (Magic/Fire/Lightning/Holy) 

        [Header("Damage Direction")]
        public float angleHitFrom; // used to determine what damage animation to play (which direction)
        public Vector3 contactPoint;


        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            PlayDirectionalBasedDamageAnimation(character);
            // ToDo: Check for Buildups (poison,bleed, ....) 
            PlayDamageSFX(character);
            PlayDamageVFX(character);

            CalculateStanceDamage(character); // Run this after all other functions that would attempt to play an animation (!)
            // If Character is A.I. check for new target if character causing damage is present 
        }

        protected virtual void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null)
            {
                // ToDo: Check for Damage modifiers and modify base damage (Physical or Elemental Damage Buff)
            }

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt; // subtract poise damage from the character total 
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage; // store previous poise damage taken for future interactions

            float remainingPoise = character.characterStatsManager.basePoiseDefense
                                   + character.characterStatsManager.offensivePoiseBonus
                                   + character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            // since Character is hit --> Reset Poise Timer (!)
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }
        protected void CalculateStanceDamage(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            // You can optionally give Weapons their own stance damage values, or use poise damage
            int stanceDamage = Mathf.RoundToInt(poiseDamage);

            if (aiCharacter != null)
            {
                aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
            }
        }
        protected void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }
        protected void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntSoundFX();
        }
        protected void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            if (character.isDead.Value)
                return;

            if (poiseIsBroken)
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
                }
            }
            else
            {
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);
                }
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);
                }
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation =
                        character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
                }
            }
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

            if (poiseIsBroken)
            {
                // restrict Movement and Actions 
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
                character.characterCombatManager.DestroyAllCurrentActionFX(); 
            }
            else
            {
                //if not poise broken just play an upperbody animation without restricting movement
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
            }
        }
    }
}