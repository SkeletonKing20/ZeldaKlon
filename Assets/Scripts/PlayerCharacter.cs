using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
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
    CapsuleCollider2D collider;


    protected override void Awake()
    {
        base.Awake();
        animeThor = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        localInteractionPositions = new Vector3[]
        {
          new Vector3(0.5f, 0.5f),
          new Vector3(-0.5f, 0.5f),
          new Vector3(0f, 1f),
          new Vector3(0, 0),
        };

        interactionBoxSize = new Vector2(0.2f, 0.2f);
    }

    private void Start()
    {
        
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

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        Collider2D[] colliders = Physics2D.OverlapCapsuleAll((Vector2)transform.position + collider.offset, 
                                                             collider.size, collider.direction, 0);
        if (!isInvincible)
        {
            foreach (var otherCollider in colliders)
            {
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    ReceiveDamage(enemy);
                }
            }
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

    private void Attack()
    {
        interactionPosition = transform.position + localInteractionPositions[(int)facingDirection];

        Collider2D otherCollider = Physics2D.OverlapBox(interactionPosition, interactionBoxSize, 0, attackableLayers);

        IDamageable damageable = otherCollider?.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(localInteractionPositions[(int)facingDirection], interactionBoxSize);
        }
    }

    protected override void Die()
    {
        Debug.Log("You Died");
        RestoreHP();
    }
}
