using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem.XR.Haptics;

namespace KrazyKatgames
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Boss States")]
        [SerializeField] BossSleepState sleepState;

        [Header("Status")]
        public NetworkVariable<bool> hasBeenDefeated = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [SerializeField] List<FogWallInteractable> fogWalls;

        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;
        //  WHEN THIS A.I IS SPAWNED, CHECK OUR SAVE FILE (DICTIONARY)
        //  IF THE SAVE FILE DOES NOT CONTAIN A BOSS MONSTER WITH THIS I.D ADD IT
        //  IF IT IS PRESENT, CHECK IF THE BOSS HAS BEEN DEFEATED
        //  IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS GAMEOBJECT
        //  IF THE BOSS HAS NOT BEEN DEFEATED, ALLOW THIS OBJECT TO CONTINUE TO BE ACTIVE

        [Header("DEBUG")]
        [SerializeField] bool wakeBossUp = false;

        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Update()
        {
            base.Update();

            if (wakeBossUp)
            {
                wakeBossUp = false;
                WakeBoss();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;
            }

            if (IsServer)
            {
                //  IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
                else
                {
                    hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
                }

                //  LOCATE FOG WALLS
                StartCoroutine(GetFogWallsFromWorldObjectManager());

                //  IF THE BOSS HAS BEEN AWAKENED, ENABLE THE FOG WALLS
                if (hasBeenAwakened.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }

                //  IF THE BOSS HAS BEEN DEFEATED DISABLE THE FOG WALLS
                if (hasBeenDefeated.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }

                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }

            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }
        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
                yield return new WaitForEndOfFrame();

            fogWalls = new List<FogWallInteractable>();

            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                    fogWalls.Add(fogWall);
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                //  RESET ANY FLAGS HERE THAT NEED TO BE RESET
                //  NOTHING YET

                //  IF WE ARE NOT GROUNDED, PLAY AN AERIAL DEATH ANIMATION

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated.Value = true;
                //  IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldSaveGameManager.instance.SaveGame();
            }

            //  PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5);

            //  AWARD PLAYERS WITH RUNES

            //  DISABLE CHARACTER
        }

        public void WakeBoss()
        {
            if (IsOwner)
            {
                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
                }
                hasBeenAwakened.Value = true;
                currentState = idle;

                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }

                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }
        }
    }
}