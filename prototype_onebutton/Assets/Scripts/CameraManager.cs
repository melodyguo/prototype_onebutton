using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [Header("Follow Camera")]
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Transform followTarget;
    private Vector3 velocity = Vector3.zero;
    private Vector3 defaultPosition;
    private bool isfollowMode = false;

    [Header("Zoom Camera")]
    [SerializeField] private float zoomAmount = 0.5f;
    [SerializeField] private float zoomDuration = 0.5f;
    public bool isZooming = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isfollowMode)
        {
            Vector3 targetPosition = followTarget.position + defaultPosition;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void SetCameraToDefault()
    {
        isfollowMode = false;
        gameObject.transform.position = defaultPosition;
    }

    public void SetCameraToFollow()
    {
        isfollowMode = true;
    }
    
    public void StartZoom()
    {
        isZooming = true;
        StartCoroutine(ZoomCoroutine());
    }

    IEnumerator ZoomCoroutine()
    {
        float initialFieldOfView = mainCamera.fieldOfView;

        // Zoom in
        float timer = 0f;
        while (timer < zoomDuration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(initialFieldOfView, initialFieldOfView / zoomAmount, timer / zoomDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Wait for a short duration (you can adjust this if needed)
        yield return new WaitForSeconds(0.5f);

        // Zoom out
        timer = 0f;
        while (timer < zoomDuration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(initialFieldOfView / zoomAmount, initialFieldOfView, timer / zoomDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        isZooming = false;
    }
}
