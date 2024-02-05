using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingUIObject : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float fallSpeed = 100f;
    [SerializeField] private Vector2 spawnPoint = new Vector2(0, 500);

    void Start()
    {
        rectTransform.anchoredPosition = spawnPoint;
    }

    void Update()
    {
        // Move object down
        rectTransform.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);
    }

    public void DestroyFallingUIObject()
    {
        Destroy(gameObject);
    }
}