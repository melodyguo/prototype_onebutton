using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ComboSystem : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float shortJumpForce = 5.0f;
    [SerializeField] private float longJumpForce = 10.0f;
    [SerializeField] private float comboJumpForce = 3.5f;
    [SerializeField] private float longPressDuration = 0.3f;
    [SerializeField] private float doublePressDuration = 0.2f;
    public bool canCombo = false;
    private bool onGround = true;
    private bool isSpacePressed = false;
    private float spacePressDuration = 0.0f;
    private float lastSpacePress = 0.0f;
    
    [Header("Combo VFX")]
    [SerializeField] private GameObject comboSplashPrefab;
    [SerializeField] private GameObject comboSplashLongPrefab;
    [SerializeField] private ComboSpawner comboSpawner;
    private int comboCount = 0;
    
    [Header("SFX")]
    [SerializeField] private AudioSource shortJumpSFX;
    [SerializeField] private AudioSource longJumpSFX;
    [SerializeField] private AudioSource shortComboSFX;
    [SerializeField] private AudioSource longComboSFX;
    [SerializeField] private AudioSource comboSuccessSFX;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource bgm;

    [Header("Score & Death")]
    public int playerScore = 0;
    [SerializeField] private TMP_Text scoreText;
    public bool isDead = false;
    [SerializeField] private GameObject deathScreen;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (isDead)
        {
            return;
        }
        
        if (comboCount > 0)
        {
            scoreText.text = "Score: " + playerScore.ToString() + " + " + comboCount.ToString();
        }
        else
        {
            scoreText.text = "Score: " + playerScore.ToString();
        }
        float currentTime = Time.time;
        
        // COMBO JUMP
        if (canCombo)
        {
            if (!comboSpawner.IsSpawning())
            {
                comboSpawner.StartSpawning();
            }
            
            // Check for space pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
                spacePressDuration += Time.time;
                playerRigidbody.mass *= 0.75f;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (isSpacePressed && currentTime - spacePressDuration >= longPressDuration)
                {
                    PlayComboLong();
                }
                else if (false && isSpacePressed && currentTime - lastSpacePress <= doublePressDuration)
                {
                    PlayComboDouble();
                }
                else if (isSpacePressed)
                {
                    PlayComboShort();
                }
                
                isSpacePressed = false;
                spacePressDuration = 0.0f;
                lastSpacePress = currentTime;
                playerRigidbody.mass = 1.0f;
            }
        }
        
        // SIMPLE JUMP
        if (onGround)
        {
            canCombo = false;
            playerScore += comboCount;
            if (comboCount > 0)
            {
                comboSuccessSFX.Play();
            }
            comboCount = 0;
            comboSpawner.StopSpawning();
            
            // Check for space pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
                spacePressDuration += currentTime;
            }

            // Check for long pressed
            if (Input.GetKeyUp(KeyCode.Space) && isSpacePressed && currentTime - spacePressDuration >= longPressDuration)
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
        if (other.gameObject.tag == "Ground")
        {
            onGround = true;
        }

        if (other.gameObject.tag == "Rope" && !isDead)
        {
            Die();
        }
    }

    private void ShortJump()
    {
        onGround = false;

        playerAnimator.SetTrigger("Jump");
        shortJumpSFX.Play();
        playerRigidbody.AddForce(Vector3.up * shortJumpForce, ForceMode.Impulse);
    }

    private void LongJump()
    {
        onGround = false;
        canCombo = true;

        playerAnimator.SetTrigger("Jump");
        longJumpSFX.Play();
        playerRigidbody.AddForce(Vector3.up * longJumpForce, ForceMode.Impulse);
    }

    private void PlayComboShort()
    {
        playerAnimator.SetTrigger("Tap");
        shortComboSFX.Play();
        playerRigidbody.AddForce(Vector3.up * comboJumpForce, ForceMode.Impulse);
        
        /*var go = Instantiate(comboSplashPrefab, transform);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)));
        Destroy(go, 0.2f);*/
        
        comboCount += 1;
    }
    
    private void PlayComboLong()
    {
        playerAnimator.SetTrigger("Hold");
        longComboSFX.Play();
        playerRigidbody.AddForce(Vector3.up * comboJumpForce * 2.0f, ForceMode.Impulse);
        
        /*var go = Instantiate(comboSplashLongPrefab, transform);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)));
        Destroy(go, 0.2f);*/

        comboCount += 5;
    }

    private void PlayComboDouble()
    {
        playerAnimator.SetTrigger("Double");
        playerRigidbody.AddForce(Vector3.up * comboJumpForce * 1.5f, ForceMode.Impulse);
        
        /*var go = Instantiate(comboSplashLongPrefab, transform);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f)));
        Destroy(go, 0.2f);*/

        comboCount += 2;
    }

    private void Die()
    {
        isDead = true;
        playerAnimator.SetTrigger("Death");
        deathScreen.SetActive(true);
        bgm.Stop();
        deathSFX.Play();
        scoreText.text = "Score: " + playerScore.ToString();
    }
}