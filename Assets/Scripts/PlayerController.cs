using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;


    private void Awake() {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();

    }
    private void OnEnable(){
        playerControls.Enable();
    }
    private void Update(){
        PlayerInput();
    }
    private void FixedUpdate(){
        AdjustPlayerFactingDirection();
        Move();
    }
    private void PlayerInput(){
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }
    private void Move(){
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
    
    
    private void AdjustPlayerFactingDirection() {
    if (movement.x > 0) {
        mySpriteRender.flipX = false;
    } 
    else if (movement.x < 0) {
        mySpriteRender.flipX = true;
    }
    }
    }