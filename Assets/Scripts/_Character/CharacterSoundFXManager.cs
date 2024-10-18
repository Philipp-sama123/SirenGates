using UnityEngine;

namespace KrazyKatGames
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("Foot Steps")]
        [SerializeField] public AudioClip[] footSteps;
        [SerializeField] public AudioClip[] footStepsDirt;
        [SerializeField] public AudioClip[] footStepsStone;
        [SerializeField] public AudioClip[] footStepsWood;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }
        /**
         * AnimationEvent *
         */
        public virtual void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }
        public virtual void PlayDamageGruntSoundFX()
        {
            if (damageGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
            else
                Debug.Log("MISSING SFX: damageGrunt!");
        }
        public virtual void PlayAttackGruntSoundFX()
        {
            if (attackGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
            else
                Debug.Log("MISSING SFX: attackGrunt!");
        }
        public virtual void PlayStanceBreakSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.stanceBreakSFX);
        }

        public virtual void PlayBlockSoundFX()
        {
        }
        public void PlayCriticalStrikeSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.criticalStrikeSFX);
        }
    }
}