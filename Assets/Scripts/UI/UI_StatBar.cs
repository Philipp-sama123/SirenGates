using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatgames
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        // Variable to scale Bar Size depending on Stat (Higher stat = longer bar across Screen) 

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }
    
        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}