using Unity.Netcode;
using UnityEngine;

namespace KrazyKatGames
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiateGameObject;
      
        private void Start()
        {
            WorldAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiateGameObject = Instantiate(characterGameObject);
                instantiateGameObject.transform.position = transform.position;
                instantiateGameObject.transform.rotation = transform.rotation;

                instantiateGameObject.GetComponent<NetworkObject>().Spawn();
                WorldAIManager.instance.AddCharacterToSpawnedCharactersList(instantiateGameObject.GetComponent<AICharacterManager>());
            }
        }
    }
}