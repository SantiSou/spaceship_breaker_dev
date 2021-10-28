using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    public Vector3 direction;
    public GameManager manager;
    float time;
    Rigidbody2D rb; 
    Vector3 stepVector;
    public bool sideTouched;
    public bool fleet_created;
    public bool inside;
    public int enemyPoints;
    AudioSource audiosource;
    AudioSource soundEffects;
    public GameObject particlesystem;
    public GameObject ps;
    public bool shipDestroyed;
    public bool shipDeployed;
    public bool yolo;
    public int randomYolo;
    public int chanceYolo;
    public GameObject[] positions;
    public bool lerping;
    public Vector3 startPosition;
    public Vector3 endPosition;
	public GameObject[] spaceshipArray;
    
    void Start() {
        
        sideTouched = true;
        inside = false;
        shipDestroyed = false;
        shipDeployed = false;
        yolo = false;
        lerping = false;

        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();

        gameObject.name = "spaceship";
        gameObject.tag = "spaceship";
        
        audiosource = GetComponent<AudioSource> ();
		soundEffects = GameObject.Find("SoundEffects").GetComponent<AudioSource> ();
        audiosource.clip = manager.enemyEngine;
        audiosource.Play();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

    }

    void Update() {

        if (!manager.gameOver) {
            time += Time.deltaTime;

            randomYolo = 100000 - manager.getActualLevel();
            chanceYolo = Random.Range(0,randomYolo);

            if (chanceYolo == randomYolo-1) {
                yolo = true;
            }   
        }   
        else {
            Destroy(gameObject);
        }         
    }

    void FixedUpdate() {

        if (!manager.gameOver) {
            if (!yolo) {
                MovementControl();
            }
            else {
                YoloMovement();
            }
        }
    }    

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.name.Equals("ball_test(Clone)")) {

            Ball ball = other.gameObject.GetComponent<Ball>();

            if (ball != null && ball.ballCreated) {

                ball.DestroyBall();
                StartCoroutine(ShipDestroyed ());
			    soundEffects.PlayOneShot(manager.enemyExplosion[Random.Range(0, manager.enemyExplosion.Length-1)], 1f);
                manager.experience += enemyPoints;
                manager.score += enemyPoints*10;
                manager.UpdateScoreboard();
                manager.ballCreated = false;
                
            }
        } 
        else if (other.gameObject.name.Equals("block_test(Clone)")) {

            manager.gameOver = true;

        }
    }

    void MovementControl() {

        if (!shipDeployed) {
            if (transform.position.x > manager.cameraX && transform.position.x < (manager.cameraX*-1)) {
                shipDeployed = true;
                ShipDeployed();
            }

        }
        else if (transform.position.x <= manager.cameraX && !gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched && shipDeployed) {

            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = 1;

        } 
        else if (transform.position.x >= (manager.cameraX*-1) && !gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched && shipDeployed) {

            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = -1;

        }
        else if (transform.position.y < manager.cameraY) {

            Destroy(gameObject);

        }
    }  

    void YoloMovement() {

        gameObject.transform.parent = null;
        rb.velocity = Vector2.down * manager.enemyYoloSpeed * Time.deltaTime;

        if (transform.position.y < manager.cameraY ) {

            Destroy(gameObject);

        }

    }

    public void ShipDeployed () {
        
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }

    IEnumerator ShipDestroyed () {
		
        shipDestroyed = true;
        gameObject.tag = "spaceship_destroyed";
        gameObject.transform.parent = null;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject ps = Instantiate(particlesystem);
        ps.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        Destroy(ps);
    }  

    public void LerpMovement(Vector3 startPosition, Vector3 endPosition) {
        lerping = true;
    }  
}
