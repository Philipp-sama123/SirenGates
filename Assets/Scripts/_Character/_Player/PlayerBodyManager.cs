using System;
using System.Collections;
using System.Collections.Generic;
using KrazyKatGames;
using UnityEngine;

namespace MyNamespace
{
    public class PlayerBodyManager : MonoBehaviour
    {
        private PlayerManager player;

        [Header("Hair Blend shapes")]
        [SerializeField] private SkinnedMeshRenderer hairSkinnedMeshRenderer;
        [SerializeField] private int hairForCloak_BlendShapeIndex = 0;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void DisableHairForHood()
        {
            Mesh mesh = hairSkinnedMeshRenderer.sharedMesh;

            if (mesh == null)
            {
                Debug.LogError("Mesh not found on the SkinnedMeshRenderer!");
                return;
            }

            SetBlendShape(hairSkinnedMeshRenderer, hairForCloak_BlendShapeIndex, 100);
        }


        public void EnableHairForHood()
        {
            Mesh mesh = hairSkinnedMeshRenderer.sharedMesh;

            if (mesh == null)
            {
                Debug.LogError("Mesh not found on the SkinnedMeshRenderer!");
                return;
            }

            SetBlendShape(hairSkinnedMeshRenderer, hairForCloak_BlendShapeIndex, 0);
        }
        // Function to set blend shape weight by index and value
        private void SetBlendShape(SkinnedMeshRenderer skinnedMeshRenderer, int index, float weight)
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, weight);
            }
        }
    }
}