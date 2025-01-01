using UnityEngine;

namespace KrazyKatGames
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {
            Debug.Log($"Process Instant Effect ID: {instantEffectID} on character: {character.name}");
        }
    }
}