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
    public bool isGrounded = true;
    [SerializeField]
	private float moveSpeed = 6;
    [SerializeField]
    private float jumpForce = 5;
	[SerializeField]
    private float minJumpMagnitude = 0.1f;
	[SerializeField]
    private float maxJumpMagnitude = 5f;
    public float jumpSquatTime = 0.025f;

    Vector2 directionalInput;

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
        HandleCollision(collision);
    }

    void HandleCollision(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_GROUND)
        ) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

    public void Walk() {
        rb.velocity = new Vector2(
            Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL) * moveSpeed * Time.deltaTime,
            0
        );
    }

    public void SetVelocityToZero() {
        rb.velocity = Vector2.zero;
    }


    public void Jump(Vector2 jumpVector) {
        Debug.Log("Jump");
        Debug.Log("Jump Vector: " + jumpVector.ToString());
        Debug.Log("unprocesses mag: " + jumpVector.magnitude);
        float magnitude = Mathf.Clamp(jumpVector.magnitude, minJumpMagnitude, maxJumpMagnitude);
        Debug.Log("processed Mag: " + magnitude.ToString());
        rb.AddForce(jumpVector.normalized * magnitude, ForceMode2D.Impulse);
        isGrounded = false;
    }

    public PlayerGroundedState GetProperGroundedState() {
        if (Mathf.Abs(directionalInput.x) >= 0.01f) {
            return walkState;
        } else {
            return idleState;
        }
    }

}
