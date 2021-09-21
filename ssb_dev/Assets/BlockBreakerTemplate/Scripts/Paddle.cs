using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour 
{
	public float speed;	
	public float maxSpeed;	
	public float speedFactor;	
	public float slowFactor;	
	public float minX;				//The minimum x position that the paddle can move to
	public float minXdelete;				//The minimum x position that the paddle can move to
	public float maxX;				//The maximum x position that the paddle can move to
	public float maxXdelete;				//The maximum x position that the paddle can move to
	public float inertia;
	public float controlTapR;
	public float controlTapL;
	public float rectBGWidth;
	public int direction;
	public bool movingL;
	public bool movingR;
	public bool intertia;
	public bool canMove;			//Determins wether or not the paddle can move
	public bool copyCreated;
	public bool imCopy;
	public bool recentlyCreated;
	public Rigidbody2D rig;			//The paddle's rigidbody 2D component
	public GameObject ball;
	public GameObject paddlePrototype;
	public GameManager manager;
	public bool ballGenerated;


	void Start ()
	{
		manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
		ballGenerated = false;
	}
	void Update ()
	{
		if(canMove){

			MovementControl();
			InfinitePaddle();

			transform.position = new Vector3(transform.position.x, transform.position.y, 0);

		}
	}

	//Called whenever a trigger has entered this objects BoxCollider2D. The value 'col' is the Collider2D object that has interacted with this one
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "Ball"){					
			col.gameObject.GetComponent<Ball>().SetDirection(transform.position);	//Get the 'Ball' component of the colliding object and call the 'SetDirection()' function to bounce the ball of the paddle
		}
	}

	//Called when the paddle needs to be reset to the middle of the screen
	public void ResetPaddle ()
	{
		transform.position = new Vector3(0, transform.position.y, 0);	//Sets the paddle's x position to 0
	}

	public void MovementControl ()
	{

		if (Input.GetKey(KeyCode.LeftArrow)){

			if (speed > 0 && movingR) {

				speed -= (Time.deltaTime*speedFactor);
				
			}
			else {
				movingR = false;
				movingL = true;

				direction = -1;

				if (speed < maxSpeed) {

					speed += (Time.deltaTime*speedFactor);
					
				}
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow)) {
			
			if (speed > 0 && movingL) {

				speed -= (Time.deltaTime*speedFactor);
				
			}
			else {
				movingL = false;
				movingR = true;
				direction = 1;

				if (speed < maxSpeed) {

					speed += (Time.deltaTime*speedFactor);
					
				}
			}
		}
		else {
			if (movingL || movingR) {

				if (speed > inertia) {
					speed -= (Time.deltaTime*slowFactor);
				}				
			}
		}		
		
		if (movingL || movingR) {
			rig.velocity = new Vector2(direction * (speed) * Time.deltaTime, 0);
		}
	}

	public void InfinitePaddle ()
	{	
		// print(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x));		
		if(transform.position.x < manager.minX && !copyCreated)
		{
			copyCreated = true;
			GameObject paddleCopy = Instantiate(paddlePrototype);
			paddleCopy.name = gameObject.name;
			paddleCopy.transform.position = new Vector3(manager.maxXdelete, transform.position.y, 0);	
			manager.numberPlayers = 2;
		}
		else if (transform.position.x >= manager.maxX && !copyCreated)
		{
			copyCreated = true;
			GameObject paddleCopy = Instantiate(paddlePrototype);
			paddleCopy.name = gameObject.name;
			paddleCopy.transform.position = new Vector3(manager.minXdelete, transform.position.y, 0);	
			manager.numberPlayers = 2;	
		}

		if (transform.position.x < manager.minXdelete || transform.position.x > manager.maxXdelete) {
			Destroy(gameObject);
			manager.numberPlayers = 1;
		}
		else if (manager.numberPlayers < 2) {
			copyCreated = false;
		} 		
	}
}