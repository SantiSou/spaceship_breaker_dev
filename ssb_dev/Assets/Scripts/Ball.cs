using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public float speed;				//The amount of units that the ball will move each second
	public float time;
	public Vector2 direction;		//The Vector2 direction that the ball will move in (eg: diagonal = Vector2(1, 1))
	public Rigidbody2D rig;			//The ball's Rigidbody 2D component
	public GameManager manager;		//The GameManager
    public AudioSource audiosource;
	public bool goingLeft;			//Set to true when the ball is going left
	public bool goingDown;			//Set to true xwhen the ball is going down
	public bool ballCreated;	
	ParticleSystem ps;

	void Start () {
		ballCreated = false;
		manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
		audiosource = GameObject.Find("SoundEffects").GetComponent<AudioSource> ();
		direction = Vector2.down;
		ps = GetComponent<ParticleSystem>();
		var sparkle = ps.emission;
		sparkle.enabled = false;
	}

	void Update () {

		if (!manager.gameOver) {

			MovementControl ();

		}
        else {
            Destroy(gameObject);
        }
	}

	void FixedUpdate () {
		
		if (!manager.gameOver) {

			rig.velocity = direction * manager.ballSpeed * Time.deltaTime; //Sets the object's rigidbody velocity to the direction multiplied by the speed

		}
		else {
			rig.velocity = new Vector2(0.0f, 0.0f);
		}
	}

	public void SetDirection (GameObject target) {

		Vector2 dir = new Vector2();		

		dir = transform.position - target.transform.position;		
		dir.Normalize();						

		direction = dir;						

		// manager.ballSpeed += manager.ballSpeedIncrement;    				

		if(dir.x > 0)							
			goingLeft = false;
		if(dir.x < 0)						
			goingLeft = true;	
		if(dir.y > 0)						
			goingDown = false;
		if(dir.y < 0)							
			goingDown = true;
	}

	public void DestroyBall () {

		manager.countBalls--;	
		Destroy(gameObject);

	}

	void MovementControl () {

		if(transform.position.x > ((Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x)*-1) && !goingLeft){
			direction = new Vector2(-direction.x, direction.y);		
			goingLeft = true;										
		}
		if(transform.position.x < (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x) && goingLeft){	
			direction = new Vector2(-direction.x, direction.y);		
			goingLeft = false;										
		}
		if(transform.position.y > (((Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y)*-1))){							
			DestroyBall ();													
		}
		if(transform.position.y < (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y)){								
			DestroyBall ();										
		}	

	}

	void OnTriggerEnter2D(Collider2D col) {

		if(col.gameObject.tag == "Paddle"){						
			var sparkle = ps.emission;
			sparkle.enabled = true;
			ps.Play();
			// StartCoroutine(stopSparkles());
		}
		else if (col.gameObject.tag == "Player") {

			manager.gameOver = true;

		}

	}

	// IEnumerator stopSparkles() {

	// 	yield return new WaitForSeconds (.3f);
	// 	var sparkle = ps.emission;
	// 	sparkle.enabled = false;

	// }

	// public void ResetBall ()
	// {
	// 	transform.position = Vector3.zero;		//Sets the ball position to the middle of the screen
	// 	direction = Vector2.down;				//Sets the ball's direction to go down
		// StartCoroutine("ResetBallWaiter");		//Starts the 'ResetBallWaiter' coroutine to have the ball wait 1 second before moving
		// manager.LiveLost();						//Calls the 'LiveLost()' function in the GameManager function
	// }

	// public void CreateBall () {

	// 	Destroy(gameObject);

	// }

	//Called to make the ball wait a second before moving. Called when the ball dies and is respawned at the middle of the screen
	// IEnumerator ResetBallWaiter ()
	// {
	// 	speed = 0;
	// 	yield return new WaitForSeconds(1.0f);	//Wait 1 second
	// 	speed = 5000;
	// }
}

