using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Transform followTarget;

    private Vector3 velocity = Vector3.zero;
    private Vector3 defaultPosition;
    private bool isfollowMode = false;

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
}
