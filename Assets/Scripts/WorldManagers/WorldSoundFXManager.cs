using UnityEngine;

namespace KrazyKatgames
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;
        
        [Header("Action Sounds")]
        public AudioClip rollSFX;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

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
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }

        public AudioClip ChooseRandomFootStepSoundBasedOnGround(GameObject steppedOnOnject, CharacterManager character)
        {
            if (steppedOnOnject.tag == "Untagged")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footSteps);
            }
            else if (steppedOnOnject.tag == "Dirt")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsDirt);
            }
            else if (steppedOnOnject.tag == "Stone")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsStone);
            }
            return null;
        }
    }
}