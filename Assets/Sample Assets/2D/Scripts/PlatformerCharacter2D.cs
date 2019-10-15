using UnityEngine;
using UnityEngine.UI;

public class PlatformerCharacter2D : MonoBehaviour 
{
	private bool onFinalPlatform = false;							// For determining which way the player is currently facing.
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

	Transform playerGraphics;							// Reference to the graphics so we can change direction

	GameObject  Player1GameObject1;
	GameObject  Player2GameObject2;
	GameObject levelCompleteText;


	PlatformerCharacter2D[]  characters;

    void Awake()
	{
		characters = transform.parent.GetComponentsInChildren<PlatformerCharacter2D>();

		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
		playerGraphics = transform.Find("Graphics");

		if(playerGraphics == null)
			Debug.LogError ("Graphics not found!");
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
	}


	public void Move(float move, bool jump)
	{


		// If crouching, check to see if the character can stand up
		// if(!crouch && anim.GetBool("Crouch"))
		// {
		// 	// If the character has a ceiling preventing them from standing up, keep them crouching
		// 	if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
		// 		crouch = true;
		// }

		// // Set whether or not the character is crouching in the animator
		// anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{


			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;
	}

	void OnCollisionEnter2D(Collision2D collision)
    {

        if (
			collision.gameObject.layer == LayerMask.NameToLayer("UnassignedPlatforms")
			||
			collision.gameObject.layer == LayerMask.NameToLayer("Player1Platform")
			||
			collision.gameObject.layer == LayerMask.NameToLayer("Player2Platform")
		){
			Debug.Log("Entered collision");
			
			if (gameObject.layer == LayerMask.NameToLayer("Player1")){
				collision.gameObject.layer = LayerMask.NameToLayer("Player1Platform");
			}		
			else if (gameObject.layer == LayerMask.NameToLayer("Player2")){
				collision.gameObject.layer = LayerMask.NameToLayer("Player2Platform");
			}
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("EndPlatform")){
				this.onFinalPlatform = true;	
		}

		if(characters[0].onFinalPlatform && characters[1].onFinalPlatform){
			Debug.Log("Taadaaaaaaaaaaa");
 			levelCompleteText = GameObject.Find("LevelComplete");
 			levelCompleteText.GetComponent<Text>().enabled = true;
			 // Pause game
			 Time.timeScale = 0;
        // pausePanel.SetActive(true);
		}
    }

	void OnCollisionExit2D(Collision2D collision)
    {
		this.onFinalPlatform = false;	
	}

	void Update ()
    {

    }
}
