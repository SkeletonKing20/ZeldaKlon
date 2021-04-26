using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 targetPosition;
    public Vector2 offset;
    public Transform followTransform;

    private void Start()
    {
        if (followTransform == null)
        {
            var player = FindObjectOfType<PlayerCharacter>();
            followTransform = player.transform;
        }
    }
    private void LateUpdate()
    { 
        targetPosition = followTransform.position + new Vector3(offset.x,offset.y,transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.02f);
    }
}
