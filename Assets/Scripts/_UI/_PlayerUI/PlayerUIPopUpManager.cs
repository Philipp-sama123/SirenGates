using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Pop Up")]
        [SerializeField] TextMeshProUGUI popupMessageText;
        [SerializeField] GameObject popupMessageGameObject;

        [Header("Item Pop Up")]
        [SerializeField] GameObject itemPopupGameObject;
        [SerializeField] Image itemIcon;
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemAmount;

        [Header("YOU DIED Pop Up")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; //  Allows us to set the alpha to fade over time

        [Header("Boss Defeated Pop Up")]
        [SerializeField] GameObject bossDefeatedPopupGameObject;
        [SerializeField] TextMeshProUGUI bossDefeatedPopupBackgroundText;
        [SerializeField] TextMeshProUGUI bossDefeatedPopupText;
        [SerializeField] CanvasGroup bossDefeatedPopupCanvasGroup; //  Allows us to set the alpha to fade over time

        [Header("Site Of Grace Pop Up")]
        [SerializeField] GameObject graceRestoredPopupGameObject;
        [SerializeField] TextMeshProUGUI graceRestoredPopupBackgroundText;
        [SerializeField] TextMeshProUGUI graceRestoredPopupText;
        [SerializeField] CanvasGroup graceRestoredPopupCanvasGroup; //  Allows us to set the alpha to fade over time
        
        public void CloseAllPopupWindows()
        {
            popupMessageGameObject.SetActive(false);
            itemPopupGameObject.SetActive(false);

            PlayerUIManager.instance.popupWindowIsOpen = false;
        }
        public void SendPlayerMessagePopup(string messageText)
        {
            PlayerUIManager.instance.popupWindowIsOpen = true;
            popupMessageText.text = messageText;
            popupMessageGameObject.SetActive(true);
        }

        public void SendItemPopUp(Item item, int amount)
        {
            itemAmount.enabled = false;
            itemIcon.sprite = item.itemIcon;
            itemName.text = item.itemName;

            if (amount > 0)
            {
                itemAmount.enabled = true;
                itemAmount.text = "x" + amount.ToString();
            }
            itemPopupGameObject.SetActive(true);
            PlayerUIManager.instance.popupWindowIsOpen = true;
        }
        public void SendYouDiedPopUp()
        {
            //  ACTIVATE POST PROCESSING EFFECTS

            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;

            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }
        public void SendBossDefeatedPopUp(string bossDefeatedMessage)
        {
            bossDefeatedPopupText.text = bossDefeatedMessage;
            bossDefeatedPopupBackgroundText.text = bossDefeatedMessage;

            bossDefeatedPopupGameObject.SetActive(true);
            bossDefeatedPopupBackgroundText.characterSpacing = 0;

            StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopupBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopupCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopupCanvasGroup, 2, 5));
        }
        public void SendGraceRestoredPopUp(string graceRestoredMessage)
        {
            graceRestoredPopupText.text = graceRestoredMessage;
            graceRestoredPopupBackgroundText.text = graceRestoredMessage;

            graceRestoredPopupGameObject.SetActive(true);
            graceRestoredPopupBackgroundText.characterSpacing = 0;

            StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopupBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(graceRestoredPopupCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopupCanvasGroup, 2, 5));
        }
        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0; //  RESETS OUR CHARACTER SPACING
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }
        
        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;

            yield return null;
        }
        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }
    }
}