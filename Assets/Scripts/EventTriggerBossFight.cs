using UnityEngine;

namespace KrazyKatgames
{
    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] int bossID;
        private void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("Player Enters Boss AREA: " + other.name);
            AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);
            if (boss != null)
            {
                boss.WakeBoss();
            }
        }
    }
}