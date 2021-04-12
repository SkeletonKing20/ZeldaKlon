using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RaycastOrigins
{
   public Vector2 bottomLeft, topLeft, bottomRight, topRight;
}

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
    BoxCollider2D collider;

    private RaycastOrigins raycastOrigins;
    float skinWidth = 0.01f;

    public int horizontalRays = 2;
    public int verticalRays = 2;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    public LayerMask collDetectMask;

    void CalculateRaycastSpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = bounds.size.y / (horizontalRays - 1);
        verticalRaySpacing = bounds.size.x / (verticalRays - 1);
    }
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void OnValidate()
    {
        horizontalRays = Mathf.Clamp(horizontalRays, 2, int.MaxValue);
        verticalRays = Mathf.Clamp(verticalRays, 2, int.MaxValue);
    }
    protected override void Awake()
    {
        base.Awake();
        animeThor = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        localInteractionPositions = new Vector3[]
        {
          new Vector3(0.5f, 0.5f),
          new Vector3(-0.5f, 0.5f),
          new Vector3(0f, 1f),
          new Vector3(0, 0),
        };

        interactionBoxSize = new Vector2(0.2f, 0.2f);

        CalculateRaycastSpacing();
    }

    private void Start()
    {
        
    }
    void Update()
    {
        movementDirection = ((Vector3.right * Input.GetAxisRaw("Horizontal")) + (Vector3.up * Input.GetAxisRaw("Vertical"))).normalized;

        currentVelocity = movementDirection * movementSpeed;

        HorizontalCollisionDetection();

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

        //Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + collider.offset, 
        //                                                     collider.size, 0);
        //if (!isInvincible)
        //{
        //    foreach (var otherCollider in colliders)
        //    {
        //        Enemy enemy = otherCollider.GetComponent<Enemy>();
        //        if (enemy != null)
        //        {
        //            ReceiveDamage(enemy);
        //        }
        //    }
        //}



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

    void HorizontalCollisionDetection()
    {
        CalculateRaycastSpacing();
        UpdateRaycastOrigins();

        float horizontalDirection = Mathf.Sign(currentVelocity.x);
        float distance = currentVelocity.x * Time.deltaTime;
        Vector2 origin = (horizontalDirection > 0) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        for (int i = 0; i < horizontalRays; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin * Vector2.up * i * horizontalRaySpacing, 
                                                 Vector2.right, distance,
                                                 collDetectMask);
            if (hit.collider != null)
            {
                Debug.Log(hit.distance);
                distance = hit.distance;

                float newX = hit.distance - skinWidth;
                currentVelocity.x = newX;
            }
        }
    }
}
