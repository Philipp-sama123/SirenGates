using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("Take Damage Effect")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;
        
        [Header("Instant Effects")]
        [SerializeField] List<InstantCharacterEffect> instantEffects;
        
        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticEffects;

        [Header("Two Hand")]
        public TwoHandingEffect twoHandingEffect;

        [Header("VFX")]
        [SerializeField] public GameObject bloodSplatterVFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            // instant effects
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
            // static effects
            for (int i = 0; i < staticEffects.Count; i++)
            {
                staticEffects[i].staticEffectID = i;
            }
        }
    }
}