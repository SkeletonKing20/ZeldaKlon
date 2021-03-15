using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    float movementSpeed = 3f;
       ///<summary>
       ///right = 0
       ///left  = 1
       ///up    = 2
       ///down  = 3
       ///</summary>
    float facingDirection = 3f;
    Vector3 movementDirection;
    Vector3 currentVelocity;

    bool isMoving;

    Vector3[] localInteractionPositions;
    Vector3 interactionPosition;
    Vector2 interactionBoxSize;
    Animator animeThor;
    private void Awake()
    {
        animeThor = GetComponent<Animator>();

        localInteractionPositions = new Vector3[] 
        { 
          new Vector3(0.5f, 0.5f),
          new Vector3(-0.5f, 0.5f),
          new Vector3(0f, 1f),
          new Vector3(0, 0),
        };

        interactionBoxSize = new Vector2(0.2f, 0.2f);
    }
    void Update()
    {
        movementDirection = ((Vector3.right * Input.GetAxisRaw("Horizontal")) + (Vector3.up * Input.GetAxisRaw("Vertical"))).normalized;

        currentVelocity = movementDirection * movementSpeed;
        isMoving = currentVelocity.sqrMagnitude > 0;
        if (isMoving)
        {
            facingDirection = GetFacingDirectionFromVector(currentVelocity);
        }
        transform.position += currentVelocity * Time.deltaTime;

        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }

        animeThor.SetFloat("xVelocity", currentVelocity.x);
        animeThor.SetFloat("yVelocity", currentVelocity.y);
        animeThor.SetFloat("facingDirection", facingDirection);
        animeThor.SetBool("isMoving", isMoving);
    }

    private int GetFacingDirectionFromVector(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //horizontal
            if (direction.x > 0)
            {
                //right
                return 0;
            }
            else
            {
                //left
                return 1;
            }
        }
        else
        {
            //vertical
            if (direction.y > 0)
            {
                //up
                return 2;
            }
            else
            {
                //down
                return 3;
            }
        }
    }

    private void Interact()
    {
        interactionPosition = transform.position + localInteractionPositions[(int)facingDirection];

        Collider2D otherCollider = Physics2D.OverlapBox(interactionPosition, interactionBoxSize, 0);

        IInteractables interactable = otherCollider?.gameObject.GetComponent<IInteractables>();
        interactable?.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(localInteractionPositions[(int)facingDirection], interactionBoxSize);
        }
    }
}
