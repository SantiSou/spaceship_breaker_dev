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
    Rigidbody2D rb; 
    public Vector3 direction;
    public bool sideTouched;
    public bool inside;
    Vector3 endPosition;
    Vector3 startPosition;

    void Start() {

        reArrange = true;
        enemyXPos = 0.0f;
        enemyYPos = 0.0f;
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        audiosource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D> ();
        sideTouched = false;
        inside = false;
        // gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((manager.cameraX),(manager.cameraY),0f);

    }

    void Update() {

        if (transform.childCount < 1) {
            Destroy(gameObject);
        } 
    }

    void FixedUpdate () {
        
        // gameObject.GetComponent<Fleet_Behaviour>().direction = new Vector3(directionValue, directionValueY, 0); 
        MovementControl();
        // rb.velocity = gameObject.GetComponent<Fleet_Behaviour>().direction * manager.enemySpeed * Time.deltaTime;
    }

    public void reArrangeFleet (GameObject spaceship) {

        enemyXPos = transform.GetChild(transform.childCount-1).transform.position.x + spaceship.GetComponent<BoxCollider2D>().bounds.size.x;
        enemyYPos = transform.GetChild(transform.childCount-1).transform.position.y;

        if (enemyXPos >= (manager.cameraX*-1) ) {

            enemyYPos = transform.GetChild(transform.childCount-1).transform.position.y - spaceship.GetComponent<BoxCollider2D>().bounds.size.y;
            enemyXPos = transform.GetChild(0).transform.position.x;
            
        }

        endPosition = new Vector3(enemyXPos, enemyYPos, spaceship.transform.position.z);
        startPosition = new Vector3(spaceship.transform.position.x, spaceship.transform.position.y, spaceship.transform.position.z);

        spaceship.transform.parent = gameObject.transform;
        spaceship.transform.position = Vector3.Lerp(startPosition, endPosition, 5f);
        
    }
    
    void MovementControl() {

        if (sideTouched) {

            gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.4f) , 0);
            gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((gameObject.GetComponent<Fleet_Behaviour>().transform.position.x+(manager.enemySpeed*directionValue)), gameObject.GetComponent<Fleet_Behaviour>().transform.position.y , 0);
            sideTouched = false;

        } 
        else {
            
            gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3((gameObject.GetComponent<Fleet_Behaviour>().transform.position.x+(manager.enemySpeed*directionValue)), gameObject.GetComponent<Fleet_Behaviour>().transform.position.y , 0);

        }
    }    
}
