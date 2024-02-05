using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float shortJumpForce = 5.0f;
    [SerializeField] private float longJumpForce = 10.0f;
    [SerializeField] private float comboJumpForce = 3.5f;
    [SerializeField] private float longPressDuration = 0.3f;
    [SerializeField] private GameObject comboSplashPrefab;
    [SerializeField] private GameObject comboSplashLongPrefab;

    public bool canCombo = false;
    private bool onGround = true;
    private bool isSpacePressed = false;
    private float spacePressDuration = 0.0f;
    private int comboCount = 0;

    // Update is called once per frame
    void Update()
    {
        // Check if player is on ground
        //onGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // COMBO JUMP
        if (canCombo)
        {
            // Check for space pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
                spacePressDuration += Time.time;
            }

            // Check for long pressed
            if (Input.GetKeyUp(KeyCode.Space) && isSpacePressed && Time.time - spacePressDuration >= longPressDuration)
            {
                isSpacePressed = false;
                spacePressDuration = 0.0f;
                
                playerRigidbody.AddForce(Vector3.up * comboJumpForce, ForceMode.Impulse);

                comboCount++;
                PlayComboSplashLong(comboCount);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                isSpacePressed = false;
                spacePressDuration = 0f;
                
                playerRigidbody.AddForce(Vector3.up * comboJumpForce, ForceMode.Impulse);

                comboCount++;
                PlayComboSplash(comboCount);
            }
        }

        comboCount = 0;
        
        // SIMPLE JUMP
        if (onGround)
        {
            canCombo = false;
            
            // Check for space pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
                spacePressDuration += Time.time;
            }

            // Check for long pressed
            if (Input.GetKeyUp(KeyCode.Space) && isSpacePressed && Time.time - spacePressDuration >= longPressDuration)
            {
                isSpacePressed = false;
                spacePressDuration = 0.0f;

                LongJump();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                isSpacePressed = false;
                spacePressDuration = 0f;

                ShortJump();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        onGround = true;
    }

    private void ShortJump()
    {
        onGround = false;

        playerRigidbody.AddForce(Vector3.up * shortJumpForce, ForceMode.Impulse);
        Debug.Log("+++++short jumping");
    }

    private void LongJump()
    {
        onGround = false;
        canCombo = true;

        playerRigidbody.AddForce(Vector3.up * longJumpForce, ForceMode.Impulse);
        Debug.Log("=====long jumping");
    }

    private void PlayComboSplash(int comboCount)
    {
        var go = Instantiate(comboSplashPrefab, transform);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)));
        go.transform.localScale *= comboCount * 1.3f;
        Destroy(go, 0.2f);
    }
    
    private void PlayComboSplashLong(int comboCount)
    {
        var go = Instantiate(comboSplashLongPrefab, transform);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)));
        go.transform.localScale *= comboCount * 1.5f;
        Destroy(go, 0.2f);
    }
}