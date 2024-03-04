using Player.Animation;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //input fields
    private BatmanCombatSystem playerActionsAsset;
    private PlayerAnimController playerAnimController;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;
    public float rayCastLength;
    const string PLAYER_IDLE = "idleAnim";


    const string PLAYER_RUN = "runAnim";

    const string PLAYER_JUMP = "jumpAnim";
    const string PLAYER_WALKBACKWARD = "walkBackwardAnim";
    const string PLAYER_WALK = "walkAnim";

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new BatmanCombatSystem();
        animator = this.GetComponent<Animator>();
        playerAnimController = GetComponent<PlayerAnimController>();

    }

    private void OnEnable()
    {
        //playerActionsAsset.Player.Jump.started += DoJump;
        //  playerActionsAsset.Player.Attack.started += DoAttack;
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        //  playerActionsAsset.Player.Jump.started -= DoJump;
        //   playerActionsAsset.Player.Attack.started -= DoAttack;
        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private void Move()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        // Apply force to the Rigidbody
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        // Reset velocity if there is no input
        if (move.ReadValue<Vector2>().sqrMagnitude < 0.01f)
        {
            rb.velocity = Vector3.zero;
        }

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        if (horizontalVelocity.sqrMagnitude > 0.01f)
            playerAnimController.ChangeAnimationState(PLAYER_RUN);
        else
            playerAnimController.ChangeAnimationState(PLAYER_IDLE);
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    /*  private void DoJump(InputAction.CallbackContext obj)
      {
          if (IsGrounded())
          {
              forceDirection += Vector3.up * jumpForce;
          }
      }

      private bool IsGrounded()
      {
          return Physics.Raycast(this.transform.position, Vector3.down, rayCastLength);
      }
  */
    // private void DoAttack(InputAction.CallbackContext obj)
    // {
    //    animator.SetTrigger("attack");
    // }
}
