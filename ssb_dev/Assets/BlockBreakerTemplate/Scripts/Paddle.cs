using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour 
{
	public float speed;	
	public float maxSpeed;	
	public float speedFactor;	
	public float slowFactor;	
	public float minX;				//The minimum x position that the paddle can move to
	public float maxX;				//The maximum x position that the paddle can move to
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


	void Start ()
	{
	}
	void Update ()
	{
		if(canMove){

			MovementControl();
			InfinitePaddle();

			transform.position = new Vector3(transform.position.x, transform.position.y, 0);	//Clamps the position so that it doesn't go below the 'minX' or past the 'maxX' values

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
		if(transform.position.x < minX && !copyCreated)
		{
			copyCreated = true;
			GameObject paddleCopy = Instantiate(paddlePrototype);
			paddleCopy.transform.position = new Vector3(3.38f, transform.position.y, 0);		
		}
		else if (transform.position.x > maxX && !copyCreated)
		{
			copyCreated = true;
			GameObject paddleCopy = Instantiate(paddlePrototype);
			paddleCopy.transform.position = new Vector3(-3.38f, transform.position.y, 0);		
		}

		if (transform.position.x < -3.38f || transform.position.x > 3.38f) {
			Destroy(gameObject);
		}
		else if (transform.position.x > minX && transform.position.x < maxX) {
			copyCreated = false;
		} 		
	}
}