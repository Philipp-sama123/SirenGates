using System;
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
                    foreach (var model in player.playerEquipmentManager.hairObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Underwear:
                    foreach (var model in player.playerEquipmentManager.underwearObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Mask:
                    foreach (var model in player.playerEquipmentManager.maskObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Attachment: // ToDo: make multiple
                    foreach (var model in player.playerEquipmentManager.attachmentObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Pants:
                    foreach (var model in player.playerEquipmentManager.attachmentObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Outfit:
                    foreach (var model in player.playerEquipmentManager.outfitObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Hood:
                    foreach (var model in player.playerEquipmentManager.hoodObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Cloak:
                    foreach (var model in player.playerEquipmentManager.cloakObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case EquipmentModelType.Bagpack:
                    foreach (var model in player.playerEquipmentManager.bagpackObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}