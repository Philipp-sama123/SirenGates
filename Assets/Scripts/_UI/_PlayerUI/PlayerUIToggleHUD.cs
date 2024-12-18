using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatGames
{
    public class PlayerUIToggleHUD : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
        }
        private void OnDisable()
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);
        }
    }
}