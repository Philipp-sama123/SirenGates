using System;
using UnityEngine;

namespace KrazyKatGames
{
    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass SpellClass;

        [Header("Spell Modifiers")]
        public float fullChargeEffectMultiplier = 1.4f;
        [Header("Spell Cost")]
        public int spellSlotsUsed = 1;
        public int staminaCost = 5;
        public int focusPointCost = 10;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellChargeFX;
        [SerializeField] protected GameObject spellCastReleaseFX;
        [SerializeField] protected GameObject spellCastReleaseFXFullCharged;
        // ToDo: full charge vfx of the spell

        [Header("Animations")]
        [SerializeField] protected string mainHandSpellAnimation;
        [SerializeField] protected string offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip warmUpSoundFX;
        public AudioClip releaseSoundFX;

        public virtual void AttemptToCastSpell(PlayerManager player)
        {
        }
        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= focusPointCost;
                player.playerNetworkManager.currentStamina.Value -= staminaCost;
            }
        }
        public virtual void SuccessfullyChargeSpell(PlayerManager player)
        {
        }
        public virtual void SuccessfullyCastSpellFullCharged(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= Mathf.RoundToInt(focusPointCost * fullChargeEffectMultiplier);
                player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaCost * fullChargeEffectMultiplier);;
            }
        }
        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {
        }
        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            return true;
        }
    }
}