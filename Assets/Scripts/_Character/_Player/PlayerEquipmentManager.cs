using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slots")]
        [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
        [HideInInspector] public WeaponModelInstantiationSlot backSlot;
        // For each new Weapon Type add here a new Slot:

        [Header("Weapon Models")]
        [HideInInspector] public GameObject rightHandWeaponModel;
        [HideInInspector] public GameObject leftHandWeaponModel;

        [Header("Weapon Managers")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        [Header("Equipment Models")]
        public GameObject underwearObject;
        [HideInInspector] public GameObject[] underwearObjects;

        public GameObject maskObject;
        [HideInInspector] public GameObject[] maskObjects;

        public GameObject attachmentObject;
        [HideInInspector] public GameObject[] attachmentObjects;

        public GameObject pantsObject;
        [HideInInspector] public GameObject[] pantsObjects;

        public GameObject outfitObject;
        [HideInInspector] public GameObject[] outfitObjects;

        public GameObject hoodObject;
        [HideInInspector] public GameObject[] hoodObjects;

        public GameObject cloakObject;
        [HideInInspector] public GameObject[] cloakObjects;

        public GameObject bagpackObject;
        [HideInInspector] public GameObject[] bagpackObjects;

        public GameObject shoesAndGlovesObject;
        [HideInInspector] public GameObject[] shoesAndGlovesObjects;

        [Header("DEBUGGING")]
        [SerializeField]
        private bool equipNewItem = false;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();

            List<GameObject> underwearObjectsList = new List<GameObject>();

            foreach (Transform child in underwearObject.transform)
            {
                underwearObjectsList.Add(child.gameObject);
            }
            underwearObjects = underwearObjectsList.ToArray();

            List<GameObject> maskObjectsList = new List<GameObject>();

            foreach (Transform child in maskObject.transform)
            {
                maskObjectsList.Add(child.gameObject);
            }
            maskObjects = maskObjectsList.ToArray();

            List<GameObject> attachmentObjectsList = new List<GameObject>();

            foreach (Transform child in attachmentObject.transform)
            {
                attachmentObjectsList.Add(child.gameObject);
            }
            attachmentObjects = attachmentObjectsList.ToArray();

            List<GameObject> pantsObjectsList = new List<GameObject>();

            foreach (Transform child in pantsObject.transform)
            {
                pantsObjectsList.Add(child.gameObject);
            }
            pantsObjects = pantsObjectsList.ToArray();

            List<GameObject> outfitObjectsList = new List<GameObject>();

            foreach (Transform child in outfitObject.transform)
            {
                outfitObjectsList.Add(child.gameObject);
            }
            outfitObjects = outfitObjectsList.ToArray();

            List<GameObject> hoodObjectsList = new List<GameObject>();

            foreach (Transform child in hoodObject.transform)
            {
                hoodObjectsList.Add(child.gameObject);
            }
            hoodObjects = hoodObjectsList.ToArray();

            List<GameObject> cloakObjectsList = new List<GameObject>();

            foreach (Transform child in cloakObject.transform)
            {
                cloakObjectsList.Add(child.gameObject);
            }
            cloakObjects = cloakObjectsList.ToArray();

            List<GameObject> bagpackObjectsList = new List<GameObject>();

            foreach (Transform child in bagpackObject.transform)
            {
                bagpackObjectsList.Add(child.gameObject);
            }
            bagpackObjects = bagpackObjectsList.ToArray();

            List<GameObject> shoesAndGlovesObjectsList = new List<GameObject>();

            foreach (Transform child in shoesAndGlovesObject.transform)
            {
                shoesAndGlovesObjectsList.Add(child.gameObject);
            }
            shoesAndGlovesObjects = shoesAndGlovesObjectsList.ToArray();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void Update()
        {
            if (equipNewItem)
            {
                equipNewItem = false;
                EquipWearables();
            }
        }
        public void EquipWearables()
        {
            LoadCloakEquipment(player.playerInventoryManager.cloakWearable);
            LoadPantsEquipment(player.playerInventoryManager.pantsWearable);
            LoadOutfitEquipment(player.playerInventoryManager.outfitWearable);
            LoadUnderwearEquipment(player.playerInventoryManager.underwearWearable);
            LoadHoodEquipment(player.playerInventoryManager.hoodWearable);
            LoadShoesAndGlovesEquipment(player.playerInventoryManager.shoesAndGlovesWearable);
        }
        public void LoadShoesAndGlovesEquipment(ShoesAndGlovesWearableItem wearable)
        {
            UnloadShoesAndGlovesEquipment();

            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.shoesAndGlovesEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.shoesAndGlovesWearable = null;
                return;
            }
            // 3. if you have an "onitemsequippedcall" on equipment run here (!) 
            // TODO

            player.playerInventoryManager.shoesAndGlovesWearable = wearable;

            // 5. If you need to check for head equipment type to disable certain body features (hoods disabling hair f.e.(!)) do it now
            // TODO

            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }

            // 7. calculate total equipment load 
            // TODO

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.shoesAndGlovesEquipmentID.Value = wearable.itemID;
        }
        private void UnloadShoesAndGlovesEquipment()
        {
            foreach (var model in shoesAndGlovesObjects)
            {
                model.SetActive(false);
            }
        }
        public void LoadHoodEquipment(HoodWearableItem wearable)
        {
            UnloadHoodEquipment();
            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.hoodEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.hoodWearable = null;
                return;
            }
            // ToDo: if you have an "onitemsequippedcall" on equipment run here (!) 

            player.playerInventoryManager.hoodWearable = wearable;
            player.playerBodyManager.DisableHairForHood();

            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }
            player.playerStatsManager.CalculateTotalArmorAbsorption();
            if (player.IsOwner)
                player.playerNetworkManager.hoodEquipmentID.Value = wearable.itemID;
        }
        private void UnloadHoodEquipment()
        {
            player.playerBodyManager.EnableHairForHood();
            foreach (var model in hoodObjects)
            {
                model.SetActive(false);
            }
        }
        public void LoadPantsEquipment(PantsWearableItem wearable)
        {
            UnloadPantsEquipment();

            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.pantsEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.cloakWearable = null;
                return;
            }
            player.playerInventoryManager.pantsWearable = wearable;

            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }
            player.playerStatsManager.CalculateTotalArmorAbsorption();
            if (player.IsOwner)
                player.playerNetworkManager.pantsEquipmentID.Value = wearable.itemID;
        }
        private void UnloadPantsEquipment()
        {
            foreach (var model in pantsObjects)
            {
                model.SetActive(false);
            }
        }
        // Equipment
        public void LoadCloakEquipment(CloakWearableItem wearable)
        {
            UnloadCloakEquipment();

            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.cloakEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.cloakWearable = null;
                return;
            }
            player.playerInventoryManager.cloakWearable = wearable;
            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }
            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.cloakEquipmentID.Value = wearable.itemID;
        }
        public void UnloadCloakEquipment()
        {
            foreach (var model in cloakObjects)
            {
                model.SetActive(false);
            }
        }
        public void LoadUnderwearEquipment(UnderwearWearableItem wearable)
        {
            UnloadUnderwearEquipment();

            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.underwearEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.underwearWearable = null;
                return;
            }
            player.playerInventoryManager.underwearWearable = wearable;
            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.underwearEquipmentID.Value = wearable.itemID;
        }
        private void UnloadUnderwearEquipment()
        {
            foreach (var model in underwearObjects)
            {
                model.SetActive(false);
            }
        }
        public void LoadOutfitEquipment(OutfitWearableItem wearable)
        {
            UnloadOutfitEquipment();

            if (wearable == null)
            {
                if (player.IsOwner)
                    player.playerNetworkManager.outfitEquipmentID.Value = -1; // will never be an ID so null (!)

                player.playerInventoryManager.outfitWearable = null;
                return;
            }
            player.playerInventoryManager.outfitWearable = wearable;

            foreach (var model in wearable.wearableModels)
            {
                model.LoadModel(player);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
                player.playerNetworkManager.outfitEquipmentID.Value = wearable.itemID;
        }
        private void UnloadOutfitEquipment()
        {
            foreach (var model in outfitObjects)
            {
                model.SetActive(false);
            }
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
                {
                    leftHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
                {
                    leftHandShieldSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon(); // Main Weapon 
            LoadLeftWeapon(); // Off Hand Weapon
        }

        //  RIGHT WEAPON

        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            //  ELDEN RINGS WEAPON SWAPPING
            //  1. Check if we have another weapon besides our main weapon, if we do, NEVER swap to unarmed, rotate between weapon 1 and 2
            //  2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not process both empty slots before returning to main weapon

            WeaponItem selectedWeapon = null;

            //  DISABLE TWO HANDING IF WE ARE TWO HANDING

            //  ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            //  IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (0)
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                //  WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;
                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                //  IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID !=
                    WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    //  ASSIGN THE NETWORK WEAPON ID SO IT SWITCHES FOR ALL CONNECTED CLIENTS
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.
                        weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2) // Todo: make a member variable
            {
                SwitchRightWeapon();
            }
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                //  REMOVE THE OLD WEAPON
                rightHandWeaponSlot.UnloadWeapon();

                //  BRING IN THE NEW WEAPON
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

                // Animator Controller is always depending on the Right Weapon (!) its the Main Weapon (!)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }
        }

        //  LEFT WEAPON

        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            //  ELDEN RINGS WEAPON SWAPPING
            //  1. Check if we have another weapon besides our main weapon, if we do, NEVER swap to unarmed, rotate between weapon 1 and 2
            //  2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not process both empty slots before returning to main weapon

            WeaponItem selectedWeapon = null;

            //  DISABLE TWO HANDING IF WE ARE TWO HANDING

            //  ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            //  IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (0)
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                //  WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;
                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                //  IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID !=
                    WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    //  ASSIGN THE NETWORK WEAPON ID SO IT SWITCHES FOR ALL CONNECTED CLIENTS
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.
                        weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2) // Todo: make a member variable
            {
                SwitchLeftWeapon();
            }
        }

        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                //  remove the old weapon
                if (leftHandWeaponSlot.currentWeaponModel != null)
                    leftHandWeaponSlot.UnloadWeapon();
                if (leftHandShieldSlot.currentWeaponModel != null)
                    leftHandShieldSlot.UnloadWeapon();

                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

                switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
                {
                    case WeaponModelType.Weapon:
                        leftHandWeaponSlot.LoadWeapon(leftHandWeaponModel);
                        break;
                    case WeaponModelType.Shield:
                        leftHandShieldSlot.LoadWeapon(leftHandWeaponModel);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        #region Animation Events
        /**
         * Animation Events
         */
        public void OpenDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(
                    WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
            }
            // Play whoosh sfx (!)
        }
        public void CloseDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
        #endregion
        // Two Handing (!)
        public void UnTwoHandWeapon()
        {
            Debug.LogWarning("PLAYER EQUIPMENT MANAGER: UnTwoHandWeapon");
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
            {
                leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }
            else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
            {
                leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }

            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
        public void TwoHandRightWeapon()
        {
            Debug.LogWarning("PLAYER EQUIPMENT MANAGER: TwoHandRightWeapon");

            // 1. Check for untwohandable item (like unarmed) --> if attempt to two hand unarmed RETURN;
            if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                // 2. If returning and not two handing -> Reset BOOL status 
                if (player.IsOwner)
                {
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = false;
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                }
                return;
            }
            // Update Animator
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            // add to hand strength bonus

            backSlot.PlaceWeaponModelInUnequippedSlot(leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
        public void TwoHandLeftWeapon()
        {
            Debug.LogWarning("PLAYER EQUIPMENT MANAGER: TwoHandLeftWeapon");
            // 1. Check for untwohandable item (like unarmed) --> if attempt to two hand unarmed RETURN;
            if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                // 2. If returning and not two handing -> Reset BOOL status 
                if (player.IsOwner)
                {
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = false;
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                }
                return;
            }
            // Update Animator
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);

            // add to hand strength bonus

            // Place the non two handed weapon model in the back slot or hip slot
            backSlot.PlaceWeaponModelInUnequippedSlot(rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);
            leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            // 4. Place the two handed weapon model in the main (right hand) 

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
}