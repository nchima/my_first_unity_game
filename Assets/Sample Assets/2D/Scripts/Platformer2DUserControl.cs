using UnityEngine;


public class Platformer2DUserControl : MonoBehaviour 
{
	private PlatformerCharacter2D player1;
	private PlatformerCharacter2D player2;
    private bool jumpPlayer1;
    private bool jumpPlayer2;


	void Awake()
	{

		GameObject  ChildGameObject1 = transform.GetChild (0).gameObject;
		GameObject  ChildGameObject2 = transform.GetChild (1).gameObject;

		player1 = ChildGameObject1.GetComponent<PlatformerCharacter2D>();
		player2 = ChildGameObject2.GetComponent<PlatformerCharacter2D>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
		if (Input.GetKeyDown(KeyCode.LeftControl)) jumpPlayer1 = true;
		if (Input.GetKeyDown(KeyCode.Space)) jumpPlayer2 = true;
    }

	void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float horizontalDirectionPlayer1 = 0;
		float horizontalDirectionPlayer2 = 0;

		if (Input.GetKey(KeyCode.A)) horizontalDirectionPlayer1 = -1;
		if (Input.GetKey(KeyCode.D)) horizontalDirectionPlayer1 = 1;
		
		if (Input.GetKey(KeyCode.LeftArrow)) horizontalDirectionPlayer2 = -1;
		if (Input.GetKey(KeyCode.RightArrow)) horizontalDirectionPlayer2 = 1;

		// Pass all parameters to the character control script.
		player1.Move( horizontalDirectionPlayer1, jumpPlayer1 );
		player2.Move( horizontalDirectionPlayer2, jumpPlayer2 );

        // Reset the jump input once it has been used.
	    jumpPlayer1 = false;
	    jumpPlayer2 = false;
	}
}
