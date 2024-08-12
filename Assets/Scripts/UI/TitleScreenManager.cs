using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatgames
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
        }

        public void OpenLoadGameMenu()
        {
            //  CLOSE MAIN MENU
            titleScreenMainMenu.SetActive(false);

            //  OPEN LOAD MENU
            titleScreenLoadMenu.SetActive(true);

            //  SELECT THE RETURN BUTTON FIRST
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            //  CLOSE LOAD MENU
            titleScreenLoadMenu.SetActive(false);

            //  OPEN MAIN MENU
            titleScreenMainMenu.SetActive(true);

            //  SELECT THE LOAD BUTTON
            mainMenuLoadGameButton.Select();
        }
    }
}