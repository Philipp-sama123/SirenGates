using System;
using System.Collections;
using System.Collections.Generic;
using KrazyKatgames;
using UnityEngine;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
    private CharacterManager character;

    private AudioSource audioSource;
    private GameObject steppedObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootStepSFX = false;


    [SerializeField] float distanceToGround = 0.05f;

    private void Awake()
    {
        character = GetComponentInParent<CharacterManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        CheckForFootsteps();
    }
    private void CheckForFootsteps()
    {
        if (character == null) return;

        if (!character.characterNetworkManager.isMoving.Value) return;

        RaycastHit hit;

        if (Physics.Raycast(
                transform.position,
                character.transform.TransformDirection(Vector3.down),
                out hit,
                distanceToGround,
                WorldUtilityManager.Instance.GetEnviroLayers()))
        {
            hasTouchedGround = true;
            if (!hasPlayedFootStepSFX)
            {
                steppedObject = hit.transform.gameObject;
            }
        }
        else
        {
            hasTouchedGround = false;
            hasPlayedFootStepSFX = false;
            steppedObject = null;
        }
        if (hasTouchedGround && !hasPlayedFootStepSFX)
        {
            hasPlayedFootStepSFX = true;
            PlayFootStepSoundFX();
        }
    }
    private void PlayFootStepSoundFX()
    {
        // different sfx depending on the layer of the ground or a tag (wood, stone, etc.) 
        audioSource.PlayOneShot(
            WorldSoundFXManager.instance.ChooseRandomFootStepSoundBasedOnGround(steppedObject, character)
        );
    }
}