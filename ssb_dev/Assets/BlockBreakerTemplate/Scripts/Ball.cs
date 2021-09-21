using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public float speed;				//The amount of units that the ball will move each second
	public float maxSpeed;			//The maximum speed that the ball can travel at
	public float time;
	public Vector2 direction;		//The Vector2 direction that the ball will move in (eg: diagonal = Vector2(1, 1))
	public Rigidbody2D rig;			//The ball's Rigidbody 2D component
	public GameManager manager;		//The GameManager
	public Paddle paddle;		//The GameManager
	public bool goingLeft;			//Set to true when the ball is going left
	public bool goingDown;			//Set to true xwhen the ball is going down
	public bool goingFaster;
	public bool ballGenerated;
	

	void Start ()
	{
		ballGenerated = false;
		goingFaster = false;
		manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();   
		direction = Vector2.up;				
	}

	void Update ()
	{

		if (ballGenerated) {

			gameObject.transform.SetParent(null);
			rig.velocity = direction * manager.ballSpeed * Time.deltaTime; //Sets the object's rigidbody velocity to the direction multiplied by the speed

			if(transform.position.x > ((Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x)*-1) && !goingLeft){					//Is the ball at the right border and is not going left (heading towards the right border)
				direction = new Vector2(-direction.x, direction.y);		//Set the ball's x direction to the opposite so that it moves away from the right border (bouncing look)
				goingLeft = true;										//Sets goingLeft to true as the ball is now moving left
			}
			if(transform.position.x < (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x) && goingLeft){					//Is the ball at the left border and is going left (heading towards the left border)
				direction = new Vector2(-direction.x, direction.y);		//Set the ball's x direction to the opposite so that it moves away from the left border (bouncing look)
				goingLeft = false;										//Sets goingLeft to false as the ball is now moving right
			}
			if(transform.position.y > (((Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y)*-1)-1.0f) && !goingDown){					//Is the ball at the top border and not going down (heading towards the top border)
				direction = new Vector2(direction.x, -direction.y);		//Set the ball's y direction to the opposite so that it moves away from the top border (bouncing look)
				goingDown = true;										//Sets goingDown to true as the ball is now moving down
			}
			if(transform.position.y < (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y)){								//Has the ball gone down past the paddle
				Destroy(gameObject);											//Call the 'ResetBall()' function to reset the ball in the middle of the screen
			}		
		}
		else {
			
			gameObject.transform.position = new Vector3(transform.parent.position.x, -3.1f, 0);
		}

		// GoingFaster()
	}

	//Called when the ball needs to change direction (hit paddle, hit brick). The target parameter is the position of the object that the ball has hit
	public void SetDirection (Vector3 target)
	{
		Vector2 dir = new Vector2();			//Creating a variable called 'dir' which is a new vector2. This will be the new direction that will be set

		dir = transform.position - target;		//'dir' is set to the difference between the ball position and the target. This is now the direction.
		dir.Normalize();						//Since the difference could be any size, it will be converted to a magnitude of 1.

		direction = dir;						//Sets the ball's direction to the 'dir' variable

		manager.ballSpeed += manager.ballSpeedIncrement;    //Increases the speed of the ball by the GameManager's 'ballSpeedIncrement' value

		// if(manager.ballSpeed > manager.ballmaxSpeed)					//Is the speed of the ball more than the 'maxSpeed' value
		// 	manager.ballSpeed = manager.ballmaxSpeed;					

		if(dir.x > 0)							//If the x direction of the ball is more than 0, set 'goingLeft' to false
			goingLeft = false;
		if(dir.x < 0)							//If the x direction of the ball is less than 0, set 'goingLeft' to true
			goingLeft = true;	
		if(dir.y > 0)							//If the y direction of the ball is more than 0, set 'goingDown' to false
			goingDown = false;
		if(dir.y < 0)							//If the y direction of the ball is less than 0, set 'goingDown' to true
			goingDown = true;
	}

	//Called when the ball goes underneath the paddle and "dies"
	public void ResetBall ()
	{
		transform.position = Vector3.zero;		//Sets the ball position to the middle of the screen
		direction = Vector2.down;				//Sets the ball's direction to go down
		// StartCoroutine("ResetBallWaiter");		//Starts the 'ResetBallWaiter' coroutine to have the ball wait 1 second before moving
		// manager.LiveLost();						//Calls the 'LiveLost()' function in the GameManager function
	}

	public void DestroyBall () {

		Destroy(gameObject);

	}

	//Called to make the ball wait a second before moving. Called when the ball dies and is respawned at the middle of the screen
	// IEnumerator ResetBallWaiter ()
	// {
	// 	speed = 0;
	// 	yield return new WaitForSeconds(1.0f);	//Wait 1 second
	// 	speed = 5000;
	// }

	// public void GoingFaster() {

	// 	if (manager.isFaster) {
	// 		if (!goingDown) {
	// 			goingDown = true;
	// 			direction = new Vector2(direction.x, -direction.y);	
	// 		}
	// 	}
	// 	if (!manager.isFaster) {
	// 		if (goingDown) {
	// 			goingDown = false;
	// 			direction = new Vector2(direction.x, -direction.y);	
	// 		}
	// 	}
	// }
}

