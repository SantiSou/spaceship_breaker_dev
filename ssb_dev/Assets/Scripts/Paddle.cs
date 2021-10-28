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
    public AudioSource audiosource;


	void Start () {
		manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
		ballGenerated = false;
		audiosource = GameObject.Find("SoundEffects").GetComponent<AudioSource> ();
		audiosource.clip = manager.playerEngine;
		audiosource.Play();
	}
	void Update () {

		if(!manager.gameOver){

			MovementControl();

		}
        else {
            Destroy(gameObject);
        }
	}

	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.tag == "Ball"){					
			col.gameObject.GetComponent<Ball>().SetDirection(gameObject);
			col.gameObject.GetComponent<Ball>().ballCreated = true;
			audiosource.PlayOneShot(manager.ballBounce, 0.5f);
		}
	}

	public void MovementControl () {

		Vector3 mouse = new Vector3(Input.mousePosition.x, 0.0f, Camera.main.transform.position.z);
		mouse = Camera.main.ScreenToWorldPoint(mouse);

		if (mouse.x >= manager.minX && mouse.x <= manager.maxX) {

			transform.position = new Vector3(mouse.x, transform.position.y, 0);

		}
	}

	// public void ResetPaddle ()
	// {
	// 	transform.position = new Vector3(0, transform.position.y, 0);
	// }

	// {

	// 	if (Input.GetKey(KeyCode.LeftArrow)){

	// 		if (speed > 0 && movingR) {

	// 			speed -= (Time.deltaTime*speedFactor);
				
	// 		}
	// 		else {
	// 			movingR = false;
	// 			movingL = true;

	// 			direction = -1;

	// 			if (speed < maxSpeed) {

	// 				speed += (Time.deltaTime*speedFactor);
					
	// 			}
	// 		}
	// 	}
	// 	else if (Input.GetKey(KeyCode.RightArrow)) {
			
	// 		if (speed > 0 && movingL) {

	// 			speed -= (Time.deltaTime*speedFactor);
				
	// 		}
	// 		else {
	// 			movingL = false;
	// 			movingR = true;
	// 			direction = 1;

	// 			if (speed < maxSpeed) {

	// 				speed += (Time.deltaTime*speedFactor);
					
	// 			}
	// 		}
	// 	}
	// 	else {
	// 		if (movingL || movingR) {

	// 			if (speed > inertia) {
	// 				speed -= (Time.deltaTime*slowFactor);
	// 			}				
	// 		}
	// 	}		
		
	// 	if (movingL || movingR) {
	// 		rig.velocity = new Vector2(direction * (speed) * Time.deltaTime, 0);
	// 	}
	// }

	// public void InfinitePaddle ()
	// {	
	// 	// print(Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x));		
	// 	if(transform.position.x < manager.minX && !copyCreated)
	// 	{
	// 		copyCreated = true;
	// 		GameObject paddleCopy = Instantiate(paddlePrototype);
	// 		paddleCopy.name = gameObject.name;
	// 		paddleCopy.transform.position = new Vector3(manager.maxXdelete, transform.position.y, 0);	
	// 		manager.numberPlayers = 2;
	// 	}
	// 	else if (transform.position.x >= manager.maxX && !copyCreated)
	// 	{
	// 		copyCreated = true;
	// 		GameObject paddleCopy = Instantiate(paddlePrototype);
	// 		paddleCopy.name = gameObject.name;
	// 		paddleCopy.transform.position = new Vector3(manager.minXdelete, transform.position.y, 0);	
	// 		manager.numberPlayers = 2;	
	// 	}

	// 	if (transform.position.x < manager.minXdelete || transform.position.x > manager.maxXdelete) {
	// 		Destroy(gameObject);
	// 		manager.numberPlayers = 1;
	// 	}
	// 	else if (manager.numberPlayers < 2) {
	// 		copyCreated = false;
	// 	} 		
	// }
}