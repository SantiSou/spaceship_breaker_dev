using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public int score;				//The player's current score
	public int lives;				//The amount of lives the player has remaining
	public int ballSpeedIncrement;	//The amount of speed the ball will increase by everytime it hits a brick
	public int ballmaxSpeed;
	int timerSec;
	public float faster;
	public float spaceObjSpeed;
	public float spaceObjxPos;
	public float spaceObjScale;	
	public float enemySpeed;
	public float ballSpeed;
	float timer;
	public bool gameOver;			//Set true when the game is over
	public bool wonGame;			//Set true when the game has been won
	public bool isFaster;
	public Text points;
	public int countPoints; 
	public int fasterInt;	
    public Text distance;
	public GameObject paddle;		//The paddle game object
	public GameObject ball;			//The ball game object
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
	public int goingDown;
	private int tick;
	private float tickTimer;
	float time;
    int enemyArray;
	

	void Start ()
	{
		StartGame();
		
		
	}

	public void StartGame ()
	{		
		time = 0.0f;
		tick = 0;
		score = 0;
		lives = 3;
		fasterInt = (int)faster;
		gameOver = false;
		wonGame = false;
		paddle.active = true;
		ball.active = true;
		distance.text = "0 mts.";
		points.text = "0 pts.";
		paddle.GetComponent<Paddle>().ResetPaddle();
		environmentCreated = false;
		
	}

	void Update () 
	{
		createEnvironment ();


		time += Time.deltaTime; 
		
        if (Random.Range(0,1000) == 1 && time >= 5) { // Condición para generar enemigos	
            createEnemy ();
            time = 0.0f;
        }  

		tickTimer += Time.deltaTime;

        if (!isFaster) {
            timer += Time.deltaTime;		
        } else
            timer += (Time.deltaTime*3);

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (!isFaster) {
				isFaster = true;
                spaceObjSpeed += faster;
				enemySpeed += faster;	
				// ballSpeedIncrement += fasterInt;
				// ball.GetComponent<Ball>().GoingFaster();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (isFaster) {
				isFaster = false;
                spaceObjSpeed -= faster;
				enemySpeed -= faster;
				// ball.GetComponent<Ball>().GoingFaster();
                
            }
        }

		// if (tickTimer >= 1) {
		// 	tickTimer -= 1;
		// 	enemySpeed += (countPoints + timerSec)/1000;
		// 	spaceObjSpeed += (countPoints + timerSec)/1000;
		// }

		UpdateDistance();
		
	}

	public void UpdatePoints () {

		points.text = countPoints.ToString() + " pts.";
	}

	public void UpdateDistance () {
		timerSec = (int)timer;
        distance.text = timerSec.ToString() + " mts.";
	}

    void createEnemy () {

		GameObject fleetObject = Instantiate(fleetPrototype);

        float enemyXPos = Random.Range(-2.5f, 2.5f);
        float enemyYPos = 5;
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

        for (int i = 0; i <= 4; i++) {

            GameObject enemyCopy = Instantiate(enemyPrototype);
            enemyCopy.transform.localScale = new Vector3(1f, 1f, 0); 
            enemyCopy.transform.position = new Vector3(enemyXPos, enemyYPos, -1f);
            enemyCopy.GetComponent<Enemy_Behaviour>().direction = new Vector3(directionValue, 0, 0);  

        	// enemyArray += 1;
            // enemyCopy.name = "Array"+enemyArray.ToString();

            switch (i) {
                case 1:
                    enemyXPos += 0.5f;
                    break;
                case 2:
                    enemyXPos -= 0.5f;
                    enemyYPos += 0.5f;
                    break;
                case 3:
                    enemyXPos += 0.5f;
                    break;
            }

			enemyCopy.transform.parent = fleetObject.transform;
        }
      
    }	

	void createEnvironment () {

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
}
