using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public int score;				//The player's current score
	public int experience;
	public int lives;				//The amount of lives the player has remaining
	public int ballSpeedIncrement;	//The amount of speed the ball will increase by everytime it hits a brick
	public int ballmaxSpeed;
	public int timerSec;
	public float faster;
	public float spaceObjSpeed;
	public float spaceObjxPos;
	public float spaceObjScale;	
	public float enemySpeed;
	public float ballSpeed;
	public float timer_keyDown;
	public float timer;
	public float time;
    public float enemyXPos;
    public float enemyYPos;
	public bool gameOver;			//Set true when the game is over
	public bool wonGame;			//Set true when the game has been won
	public bool isFaster;
	public Text points;
	public int countPoints; 
    public Text distance;
    public int distanceTxtDiff;
    public string distanceTxt;
	public GameObject paddlePrototype;		//The paddle game object
	public GameObject ballPrototype;			//The ball game object
	public GameUI gameUI;			//The GameUI class
	public GameObject brickPrefab;	//The prefab of the Brick game object which will be spawned
	public GameObject enemyPrototype;
	public GameObject fleetPrototype;
	public GameObject spaceObjPrototype;
	public GameObject environmentPrototype;
	GameObject spaceEnvironment;
	public List<GameObject> bricks = new List<GameObject>();	//List of all the bricks currently on the screen
	public Color[] colors;			//The color array of the bricks. This can be modified to create different brick color patterns
	public bool goingRight;
	public bool goingLeft;
	public bool environmentCreated;
	public bool fleet_created;
	public bool ballCreated;
	public int goingDown;
	public int pointsTxtDiff;
	public string pointsTxt;
	public int numberPlayers;
    int enemyArray;
	public float minX;
	public float minXdelete;
	public float maxX;
	public float maxXdelete;
	public int actualLevel;	
	public float cameraX;
	public float cameraY;
    public Sprite[] spriteArray;
    SpriteRenderer spriteRenderer;
	AudioSource audioSource;
	public AudioClip createBallAudio;


	void Start ()
	{

		StartGame();
		
	}

	public void StartGame ()
	{		

		audioSource = gameObject.GetComponent<AudioSource>();

		time = 0.0f;

		cameraX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
		cameraY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;

		createPadle ();

		environmentCreated 		= false;
		fleet_created 			= false;
		ballCreated 			= false;
		gameOver 				= false;
		wonGame 				= false;

		distance.text = "0000000";
		points.text = "0000000";
		actualLevel = 1;
		score = 0;
	}

	void Update () 
	{
		time += Time.deltaTime;

		generateEnvironment ();
		updateScoreboard ();
		
        if ((Random.Range(0,1000) == 1 && time >= 5) && !fleet_created) { // Condición para generar enemigos	

			fleet_created = true;
            createEnemy ();
            time = 0.0f;

        }  

        if (!isFaster) {
            timer += Time.deltaTime;		
        } //else
         //   timer += (Time.deltaTime*3);

        if (Input.GetKey(KeyCode.UpArrow)) {

			timer_keyDown += Time.deltaTime;

			if (!ballCreated) {
				ballCreated = true;
				audioSource.PlayOneShot(createBallAudio, 0.3f);
				createBall ();
			}
        }
		else if (Input.GetKeyUp(KeyCode.UpArrow)) {

				object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));

				foreach (object o in obj)
				{

					GameObject gameObject = (GameObject) o;
					
					if (gameObject.name == "ball_test(Clone)") {
						
						if (timer_keyDown < 1.0f && !gameObject.GetComponent<Ball>().ballGenerated) {
							Destroy(gameObject);
						}
						else {
							gameObject.GetComponent<Ball>().ballGenerated = true;
						}

					}	
				}
 
				ballCreated = false;
				timer_keyDown = 0.0f;
		}		
	}

	public void UpdatePoints () {

		if (score > 1000) {

			actualLevel = (int)(score/1000);

		}
		

		pointsTxt = "";
		for (pointsTxtDiff =  7 - score.ToString().Length; pointsTxtDiff > 0; pointsTxtDiff--) {

			pointsTxt += "0";

		}

		pointsTxt += score.ToString();
		points.text = pointsTxt;
		
	}

	public void updateScoreboard () {

		timerSec = (int)timer;

		distanceTxt = "";
		for (distanceTxtDiff =  7 - timerSec.ToString().Length; distanceTxtDiff > 0; distanceTxtDiff--) {

			distanceTxt += "0";

		}

		distanceTxt += timerSec.ToString();
        distance.text = distanceTxt;
	}

    void createBall () {

		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));

		foreach (object o in obj)
		{
			GameObject paddleObject = (GameObject) o;
			if (paddleObject.name == "block_test(Clone)") {

				GameObject ballObject 			= Instantiate(ballPrototype);
				
				ballObject.transform.position 	= new Vector3(paddleObject.transform.position.x, -3.1f, 0);
				ballObject.transform.parent 	= paddleObject.transform;
				paddlePrototype 				= paddleObject;
				ballObject.active 				= true;
			}	
		}			
	}

    void createEnemy () {

		GameObject fleetObject = Instantiate(fleetPrototype);
		
		enemyXPos = Random.Range(-2.5f, 2.5f);
		enemyYPos = Random.Range(-2.5f, 2.5f);

		fleetObject.GetComponent<Fleet_Behaviour>().enemyXPos = enemyXPos;
		fleetObject.GetComponent<Fleet_Behaviour>().enemyYPos = enemyYPos;
		
        int directionRandom = Random.Range(0, 1);
        int directionValue = 0;

		enemyArray = 0;

        if (directionRandom == 1) {
            directionValue = 1;
            goingRight = true; 
        }
        else {
            directionValue = -1;
            goingLeft = true; 
        }

		fleetObject.GetComponent<Fleet_Behaviour>().directionValueY = 0;
		fleetObject.GetComponent<Fleet_Behaviour>().directionValue = directionValue;

        for (int i = 1; i <= 5; i++) {

        	int enemySprite = Random.Range(0, 7);

            GameObject enemyCopy = Instantiate(enemyPrototype);
			
        	spriteRenderer = enemyCopy.GetComponent<SpriteRenderer>();
        	spriteRenderer.sprite = spriteArray[enemySprite];  // shaceships_0,6,12,18,24,30,36,42  
        	Vector2 sizeSpriteRenderer = enemyCopy.GetComponent<SpriteRenderer>().sprite.bounds.size;
        	enemyCopy.GetComponent<BoxCollider2D>().size = sizeSpriteRenderer;

            enemyCopy.transform.localScale = new Vector3(1f, 1f, 0); 
            enemyCopy.transform.position = new Vector3(enemyXPos, (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y * -1), -1f);

            enemyCopy.GetComponent<Enemy_Behaviour>().direction = new Vector3(directionValue, 0, 0);
			
			enemyXPos += enemyCopy.GetComponent<BoxCollider2D>().bounds.size.x;

			enemyCopy.transform.parent = fleetObject.transform;
        }
    }	

	void generateEnvironment () {

		if (!environmentCreated) {

			GameObject spaceEnvironment = Instantiate(environmentPrototype);
			spaceEnvironment.name = "Environment";
			environmentCreated = true;

		}		

		spaceEnvironment = GameObject.Find("Environment");

        if (Random.value < 1f / (60f * 3f)) { // Condición para generar estrellas

            float spaceObjxPos = Random.Range(-2.6f, 2.7f);
            float spaceObjScale = Random.Range(0.1f, 1f);        

            GameObject spaceObjCopy = Instantiate(spaceObjPrototype);
            spaceObjCopy.transform.localScale = new Vector3(spaceObjScale, spaceObjScale, 0); 
            spaceObjCopy.transform.position = new Vector3(spaceObjxPos, transform.position.y, -1f);
            spaceObjCopy.GetComponent<Star_Behaviour>().direction = new Vector3(0, -transform.localScale.y, 0);

			spaceObjCopy.transform.parent = spaceEnvironment.transform;
        }		
	}

	void createPadle () {

		GameObject paddle = Instantiate(paddlePrototype);
		numberPlayers = 1;
		minX = cameraX + ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		minXdelete = cameraX - ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		maxX = (cameraX * -1) - ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		maxXdelete = (cameraX * -1) + ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);

		paddle.active = true;

	}
}	

	//Spawns the bricks and sets their colours
	// public void CreateBrickArray ()
	// {
	// 	int colorId = 0;					//'colorId' is used to tell which color is currently being used on the bricks. Increased by 1 every row of bricks

	// 	for(int y = 0; y < brickCountY; y++){															
	// 		for(int x = -(brickCountX / 2); x < (brickCountX / 2); x++){
	// 			Vector3 pos = new Vector3(0.8f + (x * 1.6f), 1 + (y * 0.4f), 0);						//The 'pos' variable is where the brick will spawn at
	// 			GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity) as GameObject;	//Creates a new brick game object at the 'pos' value
	// 			brick.GetComponent<Brick>().manager = this;												//Gets the 'Brick' component of the game object and sets its 'manager' variable to this the GameManager
	// 			brick.GetComponent<SpriteRenderer>().color = colors[colorId];							//Gets the 'SpriteRenderer' component of the brick object and sets the color
	// 			bricks.Add(brick);																		//Adds the new brick object to the 'bricks' list
	// 		}

	// 		colorId++;						//Increases the 'colorId' by 1 as a new row is about to be made

	// 		if(colorId == colors.Length)	//If the 'colorId' is equal to the 'colors' array length. This means there is no more colors left
	// 			colorId = 0;
	// 	}

	// 	ball.GetComponent<Ball>().ResetBall();	//Gets the 'Ball' component of the ball game object and calls the 'ResetBall()' function to set the ball in the middle of the screen
	// }

	//Called when there is no bricks left and the player has won
	// public void WinGame ()
	// {
	// 	wonGame = true;
	// 	paddle.active = false;			//Disables the paddle so it's invisible
	// 	ball.active = false;			//Disables the ball so it's invisible
	// 	gameUI.SetWin();				//Set the game over UI screen
	// }

	//Called when the ball goes under the paddle and "dies"
	// public void LiveLost ()
	// {
	// 	lives--;										//Removes a life

	// 	if(lives < 0){									//Are the lives less than 0? Are there no lives left?
	// 		gameOver = true;
	// 		paddle.active = false;						//Disables the paddle so it's invisible
	// 		ball.active = false;						//Disables the ball so it's invisible
	// 		gameUI.SetGameOver();						//Set the game over UI screen

	// 		for(int x = 0; x < bricks.Count; x++){		//Loops through the 'bricks' list
	// 			Destroy(bricks[x]);						//Destory the brick
	// 		}

	// 		bricks = new List<GameObject>();			//Resets the 'bricks' list variable
	// 	}
	// }
