using UnityEngine;

namespace KrazyKatgames
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        /**
         * AnimationEvent *
         */
    
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }
    }
}
