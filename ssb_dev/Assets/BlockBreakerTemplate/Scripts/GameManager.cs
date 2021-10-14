using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public int score;
	public int lives;	
	public int ballSpeedIncrement;
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
	public bool gameOver;		
	public bool wonGame;			
	public bool isFaster;
	public int countPoints; 
    public int distanceTxtDiff;
    public string distanceTxt;
	public GameObject paddlePrototype;		
	public GameObject ballPrototype;		
	public GameObject enemyPrototype;
	public GameObject fleetPrototype;
	public GameObject spaceObjPrototype;
	public GameObject environmentPrototype;
	public GameObject MongoAPI;
	GameObject spaceEnvironment;
	public bool goingRight;
	public bool goingLeft;
	public bool environmentCreated;
	public bool fleet_created;
	public bool ballCreated;
	public int goingDown;
	public int pointsTxtDiff;
	public string pointsTxt;
	public int numberPlayers;
	public float minX;
	public float minXdelete;
	public float maxX;
	public float maxXdelete;
	public int actualLevel;	
	public int experience;
	public float experienceCap;
	public float experienceFactor;
	public float cameraX;
	public float cameraY;
    public GameObject[] spriteArray;
	public GameObject[] spaceshipArray;
	public AudioClip playerEngine;
	public AudioClip enemyEngine;
	public AudioClip enemyExplosion;
	public AudioClip enemyShoot;
	public AudioClip ballBounce;
    SpriteRenderer spriteRenderer;
	public Canvas canvasPrototype;
	public Text distanceText;
	public Text pointsText; 
	public GameObject background;
	public GameObject scoreboard;
	public GameObject Score_panel;
	public GameObject points;
	public GameObject Distance_panel;
	public GameObject distance;
	public int countBalls;
	public int maxBalls;
	public int countEnemies;
	public int maxEnemies;
	public bool levelUp;
    public AudioSource audiosource;

	void Start ()
	{
		StartGame();
	}

	void Update () 
	{
		time += Time.deltaTime;

		updateScoreboard ();
		updateDifficulty ();
		if (time>5) {
			updateEnemies ();
			time = 0;
		}

		if (GameObject.FindGameObjectsWithTag("Ball").Length < maxBalls && !ballCreated) {
			ballCreated = true;
			print(maxBalls);
			StartCoroutine(shootBall(maxBalls-GameObject.FindGameObjectsWithTag("Ball").Length));
		}	
	}

	public void StartGame ()

	{		

		GameObject mongo = Instantiate(MongoAPI);

		time = 0.0f;
		actualLevel 		= 1;
		experience			= 0;
		experienceFactor	= 0.2f;
		experienceCap		= (actualLevel*100)+((actualLevel*100)*experienceFactor);
		countBalls			= 0;
		maxBalls			= 1;
		countEnemies		= 0;
		maxEnemies			= 4;
		score 				= 0;
		levelUp				= false;
		audiosource = GameObject.Find("SoundEffects").GetComponent<AudioSource> ();

		cameraX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
		cameraY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;

		print(cameraX);
		print(cameraY);

		createPaddle ();

		Canvas canvas 		= Instantiate(canvasPrototype);
		RectTransform canvasRT = canvas.GetComponent<RectTransform>();
		canvas.renderMode 	= RenderMode.ScreenSpaceCamera;
		canvas.worldCamera 	= Camera.main;

		background 		= canvas.transform.Find("Background").gameObject;
		RectTransform backgroundRT = background.GetComponent<RectTransform>();
		print(Screen.width);
		print(Screen.height);

		// backgroundRT.sizeDelta = new Vector2(canvas.GetComponent<CanvasScaler>().referenceResolution.x, canvas.GetComponent<CanvasScaler>().referenceResolution.yç
		
		scoreboard 		= canvas.transform.Find("Scoreboard").gameObject;
		scoreboard.transform.position = new Vector3(0, (cameraY*-1), 0);
		scoreboard.transform.localScale = new Vector3(4f, 4f, 0); 

		Score_panel		= scoreboard.transform.Find("Score_panel").gameObject;
		points			= Score_panel.transform.Find("points").gameObject;
		points.GetComponent<Text>().text = "0000000";

		Distance_panel	= scoreboard.transform.Find("Distance_panel").gameObject;
		distance		= Distance_panel.transform.Find("distance").gameObject;
		distance.GetComponent<Text>().text = "";

		time = 0.0f;

		environmentCreated 		= false;
		ballCreated 			= false;
		gameOver 				= false;
		wonGame 				= false;

		createEnemy (4);
	}	

	public void UpdatePoints () {

		pointsTxt = "";
		for (pointsTxtDiff =  7 - score.ToString().Length; pointsTxtDiff > 0; pointsTxtDiff--) {

			pointsTxt += "0";

		}

		pointsTxt += score.ToString();
		points.GetComponent<Text>().text = pointsTxt;
		
	}

	public void updateScoreboard () {

		distance.GetComponent<Text>().text = actualLevel.ToString();

	}

	public void updateDifficulty () {

		int level = getActualLevel();

		if (levelUp) {
			if (level%2==0) {

				if (maxEnemies < 20) {
					maxEnemies++;
				} else {
					enemySpeed+=0.01f;
				}

				if (maxBalls < 3) {
					maxBalls++;
				}
			} else {
				enemySpeed+=0.01f;
				ballSpeed+=5;
			}
			levelUp = false;
		}

	}

	public void updateEnemies () {

		if (countEnemies < maxEnemies) {
			int qty = maxEnemies - countEnemies;
            createEnemy (qty);
		}

	}

    public void createBall (GameObject obj) {

		if (countBalls < maxBalls) {

			GameObject ballObject 			= Instantiate(ballPrototype);

            audiosource.PlayOneShot(enemyShoot,0.5f);
			
			ballObject.transform.position 	= new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
			ballObject.SetActive(true);
			countBalls++;

		}

	}

    void createEnemy (int qty) {

		GameObject fleetObject = Instantiate(fleetPrototype);

        int directionValue = 0;
		
		if (Random.Range(0, 1) == 1f) {

			enemyXPos = (cameraX*-1);
            directionValue = -1;
            goingLeft = true; 

		} else {

			enemyXPos = cameraX;
            directionValue = 1;
            goingRight = true; 

		}

		enemyYPos = (cameraY * -1)-1;

		float firstEnemyXPos = enemyXPos;

		fleetObject.GetComponent<Fleet_Behaviour>().directionValueY = 0;
		fleetObject.GetComponent<Fleet_Behaviour>().directionValue = directionValue;
		fleetObject.GetComponent<Fleet_Behaviour>().direction = new Vector3(directionValue, 0, 0);
		fleetObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(enemyXPos,(cameraY*-1),0f);   

		int amountCreated = 0;
        for (int i = 0; i < qty; i++) {

        	int enemySprite = Random.Range(0, spriteArray.Length);

            GameObject enemyCopy = Instantiate(spriteArray[enemySprite]);

            enemyCopy.transform.localScale = new Vector3(2f, 2f, 0); 
            enemyCopy.transform.position = new Vector3(enemyXPos, (cameraY*-1), 0f);

        	Vector2 sizeSpriteRenderer = enemyCopy.GetComponent<SpriteRenderer>().sprite.bounds.size;
        	enemyCopy.GetComponent<BoxCollider2D>().size = sizeSpriteRenderer;

			amountCreated++;

			if (amountCreated%5==0) {

				enemyXPos = firstEnemyXPos;
				enemyYPos += enemyCopy.GetComponent<BoxCollider2D>().bounds.size.y;

			} else {

				enemyXPos += enemyCopy.GetComponent<BoxCollider2D>().bounds.size.x;

			}

            enemyCopy.GetComponent<Enemy_Behaviour>().direction = new Vector3(directionValue, 0, 0);

			enemyCopy.transform.parent = fleetObject.transform;

			countEnemies++;
        }                                                                                                                           
    }	

	void createPaddle () {

		GameObject paddle = Instantiate(paddlePrototype);
		numberPlayers = 1;
		minX = cameraX + ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		minXdelete = cameraX - ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		maxX = (cameraX * -1) - ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);
		maxXdelete = (cameraX * -1) + ((paddle.GetComponent<SpriteRenderer>().bounds.size.x/2)/2);

		paddle.SetActive(true);

	}

	int getActualLevel () {

		experienceCap	= (actualLevel*100)+((actualLevel*100)*experienceFactor);

		if (experience >= experienceCap) {

			actualLevel++;
			experience = 0;
			levelUp = true;
		}

		return actualLevel;

	}

	// void shootBall (int qty) {


	// }

    IEnumerator shootBall (int qty) {
		
		print("qty:"+qty.ToString());
		spaceshipArray = GameObject.FindGameObjectsWithTag("spaceship");
		for (int i = 0; i < qty; i++) {
			yield return new WaitForSeconds(1);
			createBall(spaceshipArray[Random.Range(0, (int)spaceshipArray.Length-1)]);
		}
		ballCreated = false;
    } 	
}	

	// void generateEnvironment () {

	// 	if (!environmentCreated) {

	// 		GameObject spaceEnvironment = Instantiate(environmentPrototype);
	// 		spaceEnvironment.name = "Environment";
	// 		environmentCreated = true;

	// 	}		

	// 	spaceEnvironment = GameObject.Find("Environment");

    //     if (Random.value < 1f / (60f * 3f)) { // Condición para generar estrellas

    //         float spaceObjxPos = Random.Range(-2.6f, 2.7f);
    //         float spaceObjScale = Random.Range(0.1f, 1f);        

    //         GameObject spaceObjCopy = Instantiate(spaceObjPrototype);
    //         spaceObjCopy.transform.localScale = new Vector3(spaceObjScale, spaceObjScale, 0); 
    //         spaceObjCopy.transform.position = new Vector3(spaceObjxPos, transform.position.y, -1f);
    //         spaceObjCopy.GetComponent<Star_Behaviour>().direction = new Vector3(0, -transform.localScale.y, 0);

	// 		spaceObjCopy.transform.parent = spaceEnvironment.transform;
    //     }		
	// }

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
