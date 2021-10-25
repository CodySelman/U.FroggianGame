using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent (typeof (PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    // component references
    [SerializeField]
    PlayerHead playerHead;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public TMP_Text stateText;
    [System.NonSerialized]
    public Rigidbody2D rb;
    // [SerializeField]
    // CapsuleCollider2D capsCollider;
    BoxCollider2D boxCollider;
    [SerializeField]
    private PhysicsMaterial2D defaultMaterial;
    [SerializeField]
    private PhysicsMaterial2D stunMaterial;
    public Arrow arrow;
    public GameObject arrowAnchor;

    public bool debug = false;

    // State Machine
    private StateMachine sm;
    [System.NonSerialized]
    public PlayerIdleState idleState;
    [System.NonSerialized]
    public PlayerWalkState walkState;
    [System.NonSerialized]
    public PlayerChargeJumpState chargeJumpState;
    [System.NonSerialized]
    public PlayerJumpState jumpState;
    [System.NonSerialized]
    public PlayerLandingLagState landingLagState;
    [System.NonSerialized]
    public PlayerStunState stunState;

    // movement variables
    public Vector2 directionalInput;
    public int playerFacingDir = 0;
    public bool isGrounded = true;
    public bool isMovingX = false;
    public bool isPlayerTryingToMoveX = false;
    [SerializeField]
	private float moveSpeed = 6;
    [SerializeField]
    private float jumpForce = 5;
    public float minJumpMagnitude = 0.1f;
    public float maxJumpMagnitude = 5f;
    public float minJumpAngle = 0f;
    public float jumpSquatTime = 0.025f;
    public float landingLagTime = 0.25f;
    public float bonkSpriteTime = 0.5f;
    public float ballSpinSpeed = 1f;
    public float ballExitVelocity = 0.5f;

    void Start() {
        // components 
        rb = GetComponent<Rigidbody2D>();
        // capsCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        // ActivateStunMaterial(false);

        // State Machine
        sm = new StateMachine();
        idleState = new PlayerIdleState(this, sm);
        walkState = new PlayerWalkState(this, sm);
        chargeJumpState = new PlayerChargeJumpState(this, sm);
        jumpState = new PlayerJumpState(this, sm);
        landingLagState = new PlayerLandingLagState(this, sm);
        stunState = new PlayerStunState(this, sm);
        sm.Initialize(idleState);

        arrow.ShowSpriteRenderer(false);

        #if !UNITY_EDITOR
            debug = false;
        #endif
    }

    void Update() {
        isPlayerTryingToMoveX = CheckForPlayerInputX();
        isGrounded = CheckForGrounded();

        sm.CurrentState.HandleInput();
        sm.CurrentState.LogicUpdate();

        #if UNITY_EDITOR
            if (debug) {
                stateText.text = sm.CurrentState.ToString();
            }
        #endif
    }

    void FixedUpdate() {
        isMovingX = CheckForMovementX();

        sm.CurrentState.PhysicsUpdate();
    }

    void LateUpdate()
    {
        sm.CurrentState.LateUpdate();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        HandleCollision(collision);
    }

    void OnCollisionStay2D(Collision2D collision) {
        // HandleCollision(collision);
    }

    void HandleCollision(Collision2D collision) {
        if (sm.CurrentState == jumpState) {
            Debug.Log("Collision. isGrounded: " + isGrounded);
            // Debug.Log(collision.otherCollider.);
        }
    }

    bool CheckForMovementX() {
        return Mathf.Abs(rb.velocity.x) >= 0.01f;
    }

    bool CheckForPlayerInputX() {
        return Mathf.Abs(directionalInput.x) >= 0.1f;
    }

    bool CheckForGrounded() {
        // Cast a number of rays from  vertical center of capsule, equal to capsule height/2 + offset
        // if any of the rays hit the ground, isGrounded = true
        Bounds bounds = boxCollider.bounds;
        float boundsWidth = bounds.size.x;
        int rayCount = 5;
        float rayBuffer = 0.05f;
        // float checkingWidth = transform.position.x + bounds.extents.x + 0.05f;

        for (int i = 0; i < rayCount; i ++) {
            float rayOriginX = (transform.position.x + bounds.extents.x) 
                - ((bounds.size.x / (rayCount - 1)) * i);
            // buffer to prevent getting stuck on ledge
            if (i == 0) {
                rayOriginX += 0.025f;
            } 
            else if (i == (rayCount - 1)) {
                rayOriginX -= 0.025f;
            }

            Vector2 rayOrigin = new Vector2(
                rayOriginX,
                transform.position.y + boxCollider.offset.y - bounds.extents.y
            );

            if (debug) {
                Debug.DrawRay(
                    rayOrigin, 
                    Vector2.down * rayBuffer, 
                    Color.green
                );
            }

            RaycastHit2D[] hit = Physics2D.RaycastAll(
                rayOrigin, 
                Vector2.down, 
                rayBuffer
            );

            foreach (RaycastHit2D h in hit) {
                if (debug) {
                    // Debug.Log("CheckForGrounded hit: " + h.collider.gameObject.name);
                }
                if (h.collider.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_GROUND)) {
                    return true;
                }
            }
		}
        return false;
    }

    public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

    public void Walk() {
        // float tempMoveSpeed = Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime;
        // float currentMoveDir = Mathf.Sign(rb.velocity.x);
        // float inputMoveDir = Mathf.Sign(tempMoveSpeed);
        // float processedMoveSpeed = 0;
        // float resultantMoveDir = currentMoveDir + inputMoveDir;

        // if (resultantMoveDir > 0) { // moving right
        //     processedMoveSpeed = Mathf.Max(
        //         Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime,
        //         rb.velocity.x
        //     );
        // } else if (resultantMoveDir < 0) { // moving left
        //     processedMoveSpeed = Mathf.Min(
        //         Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime,
        //         rb.velocity.x
        //     );
        // } else { // trying to move the opposite direction of current velocity
        //     processedMoveSpeed = 
        //         Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime +
        //         rb.velocity.x;
        // }

        float processedMoveSpeed = Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime;

        // rb.velocity = new Vector2(
        //     processedMoveSpeed,
        //     rb.velocity.y
        // );
        // rb.MovePosition(new Vector2(transform.position.x + processedMoveSpeed, transform.position.y));
        transform.Translate(new Vector3(processedMoveSpeed, 0, 0));
    }

    public void Jump(Vector2 jumpVector) {
        // Debug.Log("Jump Vector: " + jumpVector.ToString());
        // Debug.Log("unprocesses mag: " + jumpVector.magnitude);
        float magnitude = Mathf.Clamp(
            jumpVector.magnitude, 
            minJumpMagnitude, 
            maxJumpMagnitude
        );
        float jumpDir = Mathf.Sign(jumpVector.x);
        float tempAngle = jumpDir > 0 ? 
            Vector2.SignedAngle(Vector2.right, jumpVector) :
            Vector2.SignedAngle(Vector2.left, jumpVector) * -1;
        // Debug.Log("tempAngle: " + tempAngle);
        float jumpAngle = Mathf.Max(tempAngle, minJumpAngle);
        // Debug.Log("jumpAngle: " + jumpAngle);
        Vector2 processedJumpVector = Utilities.DegreeToVector2(jumpAngle);
        processedJumpVector.x *= jumpDir;
        // Debug.Log("ProcessedJumpVector: " + processedJumpVector.ToString());
        // Debug.Log("processed Mag: " + magnitude.ToString());
        rb.AddForce(processedJumpVector * magnitude* jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    public PlayerGroundedState GetProperGroundedState() {
        if (CheckForPlayerInputX()) {
            return walkState;
        } else {
            return idleState;
        }
    }

    public void SetVelocityToZero() {
        rb.velocity = Vector2.zero;
    }

    public void SetPlayerFacingDirection(int dir) {
        if (dir != 0 && dir != 1 && dir != -1) {
            Debug.LogError("SetPlayerFacingDirection must be called with either 1, 0 or -1. Called with: " + dir);
        }

        playerFacingDir = dir;
    }

    // public void ActivateStunMaterial(bool shouldActivate = true) {
    //     if (shouldActivate) {
    //         // rb.sharedMaterial = stunMaterial;
    //         // boxCollider.sharedMaterial = stunMaterial;
    //         capsCollider.sharedMaterial = stunMaterial;
    //     } else {
    //         // rb.sharedMaterial = defaultMaterial;
    //         // boxCollider.sharedMaterial = defaultMaterial;
    //         capsCollider.sharedMaterial = defaultMaterial;
    //     }
    // }

    public void ProcessHeadCollision(Collision2D collision) {
        if (sm.CurrentState == stunState) {
            stunState.GetBonked();
        }

        if (
            sm.CurrentState == jumpState
            && sm.CurrentState != stunState 
            && !isGrounded 
            && sm.CurrentState != landingLagState
        ) {
            sm.ChangeState(stunState);
        }
    }

    public void TurnIntoBall(bool shouldBall = true) {
        // boxCollider.enabled = !shouldBall;
        boxCollider.isTrigger = shouldBall;
    }
}
