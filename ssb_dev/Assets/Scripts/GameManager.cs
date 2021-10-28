using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

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
	public float enemyYoloSpeed;
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
	public GameObject mongoText;
	GameObject spaceEnvironment;
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
	public int experienceCap;
	public float experienceFactor;
	public float cameraX;
	public float cameraY;
    public GameObject[] spriteArray;
	public GameObject[] spaceshipArray;
	public AudioClip playerEngine;
	public AudioClip enemyEngine;
	public AudioClip[] enemyExplosion;
	public AudioClip enemyShoot;
	public AudioClip ballBounce;
    SpriteRenderer spriteRenderer;
	public Canvas canvasPrototype;
	public Canvas canvas;
	public Text distanceText;
	public Text pointsText; 
	public GameObject background;
	public GameObject scoreboard;
	public GameObject Score_panel;
	public GameObject points;
	public GameObject Distance_panel;
	public GameObject distance;
	public GameObject recordScore_panel;
	public GameObject nickname_panel;
	public GameObject inputField;
	public InputField InputField;
	// public GameObject mongoText;
	public int countBalls;
	public int maxBalls;
	public int maxEnemies;
	public bool levelUp;
	public bool restarGame;
    public AudioSource audiosource;
	public GameObject mongo;
	public UnityWebRequest mongoResponse;
	public TextMeshPro textmeshPro;
	public bool deploying;
	public bool gameStarted;
	public bool showRecordScore;

	void Start () {

		gameStarted = false;
		mongo = Instantiate(MongoAPI);

		if (!(SceneManager.GetActiveScene().buildIndex == 0)) {
			StartGame();
			gameStarted = true;
		}
		// mongoResponse = mongo.GetComponent<MongoAPI>().GetDatabase();
		// endgamePanel = Instantiate(endgamePanelPrototype);
		
	}

	void Update () {

		if (gameStarted) {
			if (!gameOver) {

				UpdateStates ();

			} 
			else {

				if (!showRecordScore) {	

					mongo.GetComponent<MongoAPI>().GetLastPoints();	

					showRecordScore = true;
					recordScore_panel.SetActive(true);

				}


			}
		}
	}

	public void StartGame () {

		restarGame = false;
		time = 0.0f;
		actualLevel 		= 1;
		experience			= 0;
		experienceFactor	= 0.2f;
		experienceCap		= (actualLevel*100)+(int)((actualLevel*100)*experienceFactor);
		countBalls			= 0;
		maxBalls			= 1;
		maxEnemies			= 4;
		enemySpeed			= 0.01f;
		ballSpeed			= 100;
		score 				= 0;
		levelUp				= false;
		enemyYoloSpeed		= 200;
		audiosource			= GameObject.Find("SoundEffects").GetComponent<AudioSource> ();
		deploying 			= false;
		gameOver			= false;
		showRecordScore		= false;

		cameraX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
		cameraY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;

		print(cameraX);
		print(cameraY);

		createPaddle ();

		if (GameObject.FindGameObjectsWithTag("Canvas").Length < 1) {

			canvas 		= Instantiate(canvasPrototype);
			RectTransform canvasRT = canvas.GetComponent<RectTransform>();
			canvas.renderMode 	= RenderMode.ScreenSpaceCamera;
			canvas.worldCamera 	= Camera.main;

			// print(Screen.width);
			// print(Screen.height);
			
			scoreboard 		= canvas.transform.Find("Scoreboard").gameObject;
			scoreboard.transform.position = new Vector3(0, (cameraY*-1), 0);
			scoreboard.transform.localScale = new Vector3(4f, 4f, 0); 

			Score_panel		= scoreboard.transform.Find("Score_panel").gameObject;
			points			= Score_panel.transform.Find("points").gameObject;
			points.GetComponent<Text>().text = "0000000";

			Distance_panel	= scoreboard.transform.Find("Distance_panel").gameObject;
			distance		= Distance_panel.transform.Find("distance").gameObject;
			distance.GetComponent<Text>().text = "";
		}

		recordScore_panel	= canvas.transform.Find("RecordScore").gameObject;
		nickname_panel		= recordScore_panel.transform.Find("Nickname").gameObject;
		inputField			= nickname_panel.transform.Find("InputField").gameObject;
		InputField 			= inputField.GetComponent<InputField>();

		recordScore_panel.SetActive(false);

		time = 0.0f;

		environmentCreated 		= false;
		ballCreated 			= false;
		gameOver 				= false;
		wonGame 				= false;

		StartCoroutine(deployEnemy (4));
	}	

	public void RestartGame () {

		restarGame = true;
		
		StartGame();

	}

	void UpdateStates () {

		time += Time.deltaTime;

		UpdateScoreboard ();
		UpdateDifficulty ();

		if (time>2 && !deploying) {
			UpdateEnemies ();
			time = 0;
		}

		if (GameObject.FindGameObjectsWithTag("Ball").Length < maxBalls && !ballCreated) {
			ballCreated = true;
			StartCoroutine(shootBall(maxBalls-GameObject.FindGameObjectsWithTag("Ball").Length));
		}
		else if (GameObject.FindGameObjectsWithTag("Ball").Length == 0) {
			ballCreated = true;
			StartCoroutine(shootBall(maxBalls));
		}
	}

	public void UpdateLeaderboard (UnityWebRequest mongoResponse) {

		// mongoText = endgamePanel.transform.Find("Texto").gameObject;
		// textmeshPro = mongoText.GetComponent<TextMeshPro>();
		// print(mongoResponse.downloadHandler.text);
		// textmeshPro.text = mongoResponse.downloadHandler.text;
		// if (mongoResponse.downloadHandler.text != null) {
		// 	textmeshPro.SetText(mongoResponse.downloadHandler.text);
		// }

	}

	public void UpdateScoreboard () {

		distance.GetComponent<Text>().text = actualLevel.ToString();

		pointsTxt = "";
		for (pointsTxtDiff =  7 - score.ToString().Length; pointsTxtDiff > 0; pointsTxtDiff--) {

			pointsTxt += "0";

		}

		pointsTxt += score.ToString();
		points.GetComponent<Text>().text = pointsTxt;

	}

	public void UpdateDifficulty () {

		int level = getActualLevel();

		if (levelUp) {
			if (level%2==0) {

				if (maxEnemies < 20) {
					maxEnemies+=2;
				} else {
					enemySpeed+=0.01f;
					enemyYoloSpeed+=25;
				}

				if (maxBalls <= 5) {
					maxBalls++;
				}
			} else {
				enemySpeed+=0.01f;
				ballSpeed+=25;
			}
			levelUp = false;
		}

	}

	public void UpdateEnemies () {

		spaceshipArray = GameObject.FindGameObjectsWithTag("spaceship");
		if (spaceshipArray.Length==0) {
			// int qty = maxEnemies - spaceshipArray.Length;
			StartCoroutine(deployEnemy (maxEnemies));
		}

	}

    public void createBall (GameObject obj) {

		if (obj != null) {
			if (countBalls < maxBalls) {

				GameObject ballObject 			= Instantiate(ballPrototype);

				audiosource.PlayOneShot(enemyShoot,0.5f);
				
				ballObject.transform.position 	= new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
				ballObject.SetActive(true);
				countBalls++;

			}
		}	
	}

    void createEnemy (int qty) {

		GameObject fleetObject = createFleet ();

		float enemyPosX;
		float enemyPosY;
		int countEnemy;

		enemyPosX = fleetObject.transform.position.x;
		enemyPosY = fleetObject.transform.position.y-1.6f;
		countEnemy = 0;

		if (qty == 1) {
			enemyPosY = Random.Range(enemyPosY, 3f);
		}

        for (int i = 0; i < qty; i++) {

			countEnemy++;

        	int enemySprite = Random.Range(0, spriteArray.Length);

            GameObject enemyCopy = Instantiate(spriteArray[enemySprite]);

            enemyCopy.transform.localScale = new Vector3(2f, 2f, 0); 
			Vector2 sizeSpriteRenderer = enemyCopy.GetComponent<SpriteRenderer>().sprite.bounds.size;
        	enemyCopy.GetComponent<BoxCollider2D>().size = sizeSpriteRenderer;

			enemyCopy.transform.parent = fleetObject.transform;

            enemyCopy.transform.position = new Vector3(enemyPosX, enemyPosY, 0f);

			if (countEnemy%5==0) {
				
				enemyPosX = fleetObject.transform.position.x;
				enemyPosY -= enemyCopy.GetComponent<SpriteRenderer>().sprite.bounds.size.x*2;
				countEnemy = 0;

			} else {

				enemyPosX -= ((enemyCopy.GetComponent<SpriteRenderer>().sprite.bounds.size.x*2)*fleetObject.GetComponent<Fleet_Behaviour>().directionValue);

			}


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

	public GameObject createFleet () {

		GameObject fleetObject = Instantiate(fleetPrototype);
		fleetObject.tag = "Fleet";
		
		float randomDir = Random.Range(0, 2);

        int directionValue = -1;
		enemyXPos = (cameraX*-1);

		if (randomDir == 1f) {

			enemyXPos = cameraX;
            directionValue = 1;

		}

		enemyYPos = (cameraY * -1)-1;

		fleetObject.GetComponent<Fleet_Behaviour>().directionValueY = 0;
		fleetObject.GetComponent<Fleet_Behaviour>().directionValue = directionValue;
		fleetObject.GetComponent<Fleet_Behaviour>().direction = new Vector3(directionValue, 0, 0);
		fleetObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(enemyXPos,(cameraY*-1),0f);  		

		return fleetObject;

	}

	public int getActualLevel () {

		experienceCap	= (actualLevel*100)+(int)((actualLevel*100)*experienceFactor);

		if (experience >= experienceCap) {

			actualLevel++;
			experience = experience - experienceCap;
			levelUp = true;
		}

		return actualLevel;

	}

    IEnumerator shootBall (int qty) {
		
		spaceshipArray = GameObject.FindGameObjectsWithTag("spaceship");

		if (spaceshipArray.Length >= 1) {
			for (int i = 0; i < qty; i++) {
				GameObject shipShooting = spaceshipArray[Random.Range(0, (int)spaceshipArray.Length-1)];
				if (!shipShooting.GetComponent<Enemy_Behaviour>().shipDestroyed && shipShooting.GetComponent<Enemy_Behaviour>().shipDeployed) {
					createBall(shipShooting);
				}
			}
		}
		ballCreated = false;
		yield return new WaitForSeconds (0f);
    } 	

	IEnumerator deployEnemy (int qty) {
		deploying = true;
		// print("deployEnemy");
		if (Random.Range(0,2) == 1) {
			
			createEnemy(qty);
			deploying = false;
			yield break;

		}
		else {

			for (int i=0; i < qty; i++) {
				createEnemy(1);
				yield return new WaitForSeconds (1f);
			}
			
			deploying = false;
		}
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
