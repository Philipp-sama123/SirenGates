using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //  PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)

        //  PROCESS TIMED EFFECTS (POISON, BUILD UPS)

        //  PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM TALISMANS ECT)

        CharacterManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        [SerializeField] GameObject criticalBloodSplatterVFX;

        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticEffects;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
        //
        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
        public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
        {
            if (criticalBloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalBloodSplatterVFX, contactPoint,
                    Quaternion.identity);
            }
        }
        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            // If syncing effects across network? --> if owner launch a server RPC to process the effect
            staticEffects.Add(effect);

            effect.ProcessStaticEffect(character);

            // Check for null entries and remove them
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);
            }
        }
        public void RemoveStaticEffect(int effectID)
        {
            // If syncing effects across network? --> if owner launch a server RPC to process the effect
            StaticCharacterEffect effect;

            for (int i = 0; i < staticEffects.Count; i++)
            {
                if (staticEffects[i] != null)
                {
                    if (staticEffects[i].staticEffectID == effectID)
                    {
                        effect = staticEffects[i];
                        // Remove Effect from Character
                        effect.RemoveStaticEffect(character);
                        // Remove Effect from List
                        staticEffects.Remove(effect);
                    }
                }
            }

            // Check for null entries and remove them
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);
            }
        }
    }
}