using UnityEngine;

namespace KrazyKatgames
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
        private int finalDamageDealt = 0;

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
            base.ProcessEffect(character);
            Debug.Log("ProcessEffect " + character.name);
            Debug.Log("character.isDead.Value " + character.isDead.Value);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            // ToDo: Check for invulnerability

            // Calculate Damage
            // Check Damage Direction
            // Check for Buildups (poison,bleed, ....) 
            // Play Damage sound fx 
            // Play Damage VFX (Blood) 

            // If Character is A.I. check for new target if character causing damage is present 
        }
        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner) 
                return;

            if (characterCausingDamage != null)
            {
                // Check for Damage modifiers and modify base damage
            }
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.Log("ProcessEffect--> CalculateDamage --> finalDamageDealt: " + finalDamageDealt);

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        }
    }
}