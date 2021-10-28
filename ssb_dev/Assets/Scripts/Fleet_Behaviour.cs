using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet_Behaviour : MonoBehaviour
{
    public GameManager manager;
    public int directionValue;
    public int directionValueY;
    public float enemyXPos;
    public float enemyYPos;
    public bool reArrange;
    public bool fleeCreated;
    AudioSource audiosource;
    public Vector3 direction;
    public bool sideTouched;
    public bool inside;
    Vector3 endPosition;
    Vector3 startPosition;
    public GameObject[] positions;
    public BoxCollider2D fleetBC;
    // public List<position> positionsList = new List<position>();

    void Start() {

        reArrange = true;
        enemyXPos = 0.0f;
        enemyYPos = 0.0f;
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        audiosource = GetComponent<AudioSource>();
        sideTouched = false;
        inside = false;
        // gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((manager.cameraX),(manager.cameraY),0f);

        // createGrid();

    }

    void Update() {

        if (transform.childCount < 1) {
            Destroy(gameObject);
        }
        else if (manager.gameOver) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate () {
        
        MovementControl();
    }

    // public void reArrangeFleet (GameObject newShip, GameObject fleetShip) {

    //     foreach (position pos in positionsList) {
            
    //         GameObject go = pos.go;
    //         go.transform.position = new Vector3(pos.pos.x, pos.pos.y, 0);
    //     }
 
        // enemyXPos = transform.GetChild(transform.childCount-1).transform.position.x - (newShip.GetComponent<BoxCollider2D>().bounds.size.x);
        // enemyYPos = transform.GetChild(transform.childCount-1).transform.position.y;

        // if (enemyXPos >= (manager.cameraX*-1) || enemyXPos <= manager.cameraX) {
            
        //     enemyXPos = transform.GetChild(transform.childCount-1).transform.position.x;
        //     enemyYPos = transform.GetChild(transform.childCount-1).transform.position.y - newShip.GetComponent<BoxCollider2D>().bounds.size.y;

        // }

        // endPosition = new Vector3(enemyXPos, enemyYPos, newShip.transform.position.z);
        // startPosition = new Vector3(newShip.transform.position.x, newShip.transform.position.y, newShip.transform.position.z);

        // // Call function from Enemy_Behaviour in the Update function to achive this
        // newShip.transform.position = Vector3.Lerp(startPosition, endPosition, 1f);

        // newShip.transform.parent = gameObject.transform;
        
    // }
    
    void MovementControl() {

        if (!manager.gameOver) {
            if (sideTouched) {

                gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.8f) , 0);
                gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((gameObject.GetComponent<Fleet_Behaviour>().transform.position.x+(manager.enemySpeed*directionValue)), gameObject.GetComponent<Fleet_Behaviour>().transform.position.y , 0);
                sideTouched = false;

            } 
            else {
                
                gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((gameObject.GetComponent<Fleet_Behaviour>().transform.position.x+(manager.enemySpeed*directionValue)), gameObject.GetComponent<Fleet_Behaviour>().transform.position.y , 0);

            }
        }
    }

    public void changeParent (GameObject newParent) {

        foreach (Transform child in transform) {
            child.transform.parent = newParent.transform;
        }

    }

    // public void assignPosition (GameObject newShip) {

    //     foreach (position pos in positionsList) {
    //         if (pos.availavle) {
    //             pos.go = newShip;
    //             break;
    //         }
    //     }
    //     reArrangeFleet();
    // }

	// void createGrid () {
		
	// 	float x = 0.0f;
	// 	float y = 0.0f;		
		
	// 	for (int i=1; i<=20; i++) {
			
	// 		position position = new position();
    //         position.go = null;
	// 		position.pos = new Vector2(x, y);
	// 		position.availavle = true;
	// 		positionsList.Add(position);
			
	// 		if (x < (0.4f*4)) {
	// 			x+= 0.4f;
	// 		} else {
	// 			x = 0.0f;
	// 			y+= 0.4f;
	// 		}
	// 	}
	// }
	// public class position {
    //     public GameObject go;
	// 	public Vector2 pos;
	// 	public bool availavle;
	// }        
}
