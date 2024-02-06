using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tappable : MonoBehaviour
{
    [SerializeField] private float fallSpeed;
    [SerializeField] private bool canBePressed;
    [SerializeField] private float spacePressDuration;
    [SerializeField] private float longPressDuration = 0.3f;
    [SerializeField] private float doublePressDuration;

    
    private bool isSpacePressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canBePressed)
            {
                Destroy(gameObject);
            }
        }

        if (canBePressed)
        {
           
        }
        
        transform.position -= new Vector3(0f, fallSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Activator")
        {
            canBePressed = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Activator")
        {
            canBePressed = false;
            Destroy(gameObject);
        }
    }
}
