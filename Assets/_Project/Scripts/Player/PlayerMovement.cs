using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent (typeof (PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    // component references
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public TMP_Text stateText;
    [SerializeField]
    Rigidbody2D rb;
    CapsuleCollider2D capsCollider;

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
    public PlayerStunState stunState;

    // movement variables
    public Vector2 directionalInput;
    public bool isGrounded = true;
    public bool isMovingX = false;
    [SerializeField]
	private float moveSpeed = 6;
    [SerializeField]
    private float jumpForce = 5;
	[SerializeField]
    private float minJumpMagnitude = 0.1f;
	[SerializeField]
    private float maxJumpMagnitude = 5f;
    [SerializeField]
    private float minJumpAngle = 0f;
    public float jumpSquatTime = 0.025f;

    void Start() {
        // components 
        rb = GetComponent<Rigidbody2D>();
        capsCollider = GetComponent<CapsuleCollider2D>();

        // State Machine
        sm = new StateMachine();
        idleState = new PlayerIdleState(this, sm);
        walkState = new PlayerWalkState(this, sm);
        chargeJumpState = new PlayerChargeJumpState(this, sm);
        jumpState = new PlayerJumpState(this, sm);
        stunState = new PlayerStunState(this, sm);
        sm.Initialize(idleState);

        #if !UNITY_EDITOR
            debug = false;
        #endif
    }

    void Update() {
        isMovingX = CheckForMovementX();

        sm.CurrentState.HandleInput();
        sm.CurrentState.LogicUpdate();

        #if UNITY_EDITOR
            if (debug) {
                stateText.text = sm.CurrentState.ToString();
            }
        #endif
    }

    void FixedUpdate() {
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
        if (collision.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_GROUND)
        ) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    bool CheckForMovementX() {
        return Mathf.Abs(rb.velocity.x) >= 0.01f;
    }

    public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

    public void Walk() {
        float tempMoveSpeed = Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime;
        float currentMoveDir = Mathf.Sign(rb.velocity.x);
        float inputMoveDir = Mathf.Sign(tempMoveSpeed);
        float processedMoveSpeed = 0;
        float resultantMoveDir = currentMoveDir + inputMoveDir;

        if (resultantMoveDir > 0) { // moving right
            processedMoveSpeed = Mathf.Max(
                Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime,
                rb.velocity.x
            );
        } else if (resultantMoveDir < 0) { // moving left
            processedMoveSpeed = Mathf.Min(
                Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime,
                rb.velocity.x
            );
        } else { // trying to move the opposite direction of current velocity
            processedMoveSpeed = 
                Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime +
                rb.velocity.x;
        }

        rb.velocity = new Vector2(
            processedMoveSpeed,
            rb.velocity.y
        );
    }


    public void Jump(Vector2 jumpVector) {
        // Debug.Log("Jump");
        Debug.Log("Jump Vector: " + jumpVector.ToString());
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
        if (isMovingX) {
            return walkState;
        } else {
            return idleState;
        }
    }

}
