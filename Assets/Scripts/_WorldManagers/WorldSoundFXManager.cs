using System.Collections;
using UnityEngine;

namespace KrazyKatGames
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Boss Track")]
        [SerializeField] AudioSource bossIntroPlayer;
        [SerializeField] AudioSource bossLoopPlayer;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;
        public AudioClip pickUpItemSFX;
        public AudioClip stanceBreakSFX;
        public AudioClip criticalStrikeSFX;

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
        public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
        {
            bossIntroPlayer.clip = introTrack;
            bossIntroPlayer.volume = 1.0f;
            bossIntroPlayer.loop = false;
            bossIntroPlayer.Play();

            bossLoopPlayer.clip = loopTrack;
            bossLoopPlayer.volume = 1.0f;
            bossLoopPlayer.loop = true;
            bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
        }
        public void StopBossMusic()
        {
            StartCoroutine(FadeOutBossMusicThenStop());
        }
        private IEnumerator FadeOutBossMusicThenStop()
        {
            while (bossLoopPlayer.volume > 0)
            {
                bossLoopPlayer.volume -= Time.deltaTime;
                bossIntroPlayer.volume -= Time.deltaTime;
                yield return null;
            }

            bossIntroPlayer.Stop();
            bossLoopPlayer.Stop();
        }
    }
}