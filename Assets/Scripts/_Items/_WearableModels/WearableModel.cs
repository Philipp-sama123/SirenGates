using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatgames
{
    [CreateAssetMenu(menuName = "Wearable Model")]
    public class WearableModel : ScriptableObject
    {
        public WearableModelType wearableModelType;
        public string equipmentName;

        public void LoadModel(PlayerManager player)
        {
            switch (wearableModelType)
            {
                case WearableModelType.Underwear:
                    foreach (var model in player.playerEquipmentManager.underwearObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Mask:
                    foreach (var model in player.playerEquipmentManager.maskObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Attachment: // ToDo: make multiple
                    foreach (var model in player.playerEquipmentManager.attachmentObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Pants:
                    foreach (var model in player.playerEquipmentManager.pantsObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Outfit:
                    foreach (var model in player.playerEquipmentManager.outfitObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Hood:
                    foreach (var model in player.playerEquipmentManager.hoodObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Cloak:
                    foreach (var model in player.playerEquipmentManager.cloakObjects)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                case WearableModelType.Bagpack:
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