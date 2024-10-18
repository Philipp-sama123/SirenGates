
using UnityEngine;

namespace KrazyKatGames
{
    public class StaticCharacterEffect : ScriptableObject
    {
        [Header("Effect I.D")]
        public int staticEffectID;

        public virtual void ProcessStaticEffect(CharacterManager character)
        {
        }

        public virtual void RemoveStaticEffect(CharacterManager character)
        {
        }
    }
}