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
    
    void Start()
    {
        // int enemySprite = Random.Range(0, 7);
        sideTouched = true;
        inside = false;

        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();

        gameObject.name = "spaceship";
        gameObject.tag = "spaceship";
        // enemyPoints = System.Convert.ToInt32(spriteArray[enemySprite].ToString().Split('_')[1].Split(' ')[0]);
        audiosource = GetComponent<AudioSource> ();
		soundEffects = GameObject.Find("SoundEffects").GetComponent<AudioSource> ();
        audiosource.clip = manager.enemyEngine;
        audiosource.Play();

    }

    void Update() {

        time += Time.deltaTime;
       
    }

    void FixedUpdate() {

        MovementControl();
        // rb.velocity = gameObject.GetComponent<Enemy_Behaviour>().direction * manager.enemySpeed * Time.deltaTime;
        // gameObject.transform.position = new Vector3(0f,0f,0f); 
    }    

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.name.Equals("ball_test(Clone)")) {

            Ball ball = other.gameObject.GetComponent<Ball>();

            if (ball != null && ball.ballCreated) {

                ball.DestroyBall();
                Destroy(gameObject);
			    soundEffects.PlayOneShot(manager.enemyExplosion, 0.5f);
                manager.experience += enemyPoints;
                manager.score += enemyPoints*10;
                manager.UpdatePoints();
                manager.countEnemies--;
                manager.ballCreated = false;
                
            }
        } else if (other.gameObject.name.Equals("spaceship") && !(gameObject.transform.parent==other.transform.parent.transform)) {
            
            other.transform.parent.GetComponent<Fleet_Behaviour>().reArrangeFleet(gameObject);

        }
    }

    void MovementControl() {

        if (transform.position.x <= manager.cameraX && !gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched) {

            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = 1;

        } else if (transform.position.x >= (manager.cameraX*-1) && !gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched) {

            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = -1;

        } 
    }    
}
