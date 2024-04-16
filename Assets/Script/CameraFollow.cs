using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // control camera behavior
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    //time taken for camera to reach player's position, higher = delay following player, lower = less delay camera
    private float smoothTime = 0.7f; 
    //velocity used for smoothing
    private Vector3 velocity = Vector3.zero;

    // Reference to the target object (in this case it's the player)
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        // Calculate the target position including the offset
        Vector3 targetPosition = target.position + offset;
        // Smoothly move the camera to the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
