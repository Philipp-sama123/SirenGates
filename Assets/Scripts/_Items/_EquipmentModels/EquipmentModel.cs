using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        public EquipmentModelType equipmentModelType;
        public string equipmentName;

        public void LoadModel(PlayerManager player)
        {
            switch (equipmentModelType)
            {
                case EquipmentModelType.Hair:
                    break;
                case EquipmentModelType.Underwear:
                    break;
                case EquipmentModelType.Shirt:
                    break;
                case EquipmentModelType.Mask:
                    break;
                case EquipmentModelType.Attachment:
                    break;
                case EquipmentModelType.Pants:
                    break;
                case EquipmentModelType.Outfit:
                    break;
                case EquipmentModelType.Food:
                    break;
                case EquipmentModelType.Cloak:
                    break;
                case EquipmentModelType.Bagpack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

   
    }
}