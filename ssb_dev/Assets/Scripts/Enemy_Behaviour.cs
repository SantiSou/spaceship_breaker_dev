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
    public int enemyPoints;
    
    void Start()
    {
        int enemySprite = Random.Range(0, 7);
        sideTouched = false;

        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();

        gameObject.name = "spaceship";
        enemyPoints = System.Convert.ToInt32(spriteArray[enemySprite].ToString().Split('_')[1].Split(' ')[0]);

    }

    void Update() {

        time += Time.deltaTime;

        MovementControl();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.name.Equals("ball_test(Clone)")) {

            Ball ball = other.gameObject.GetComponent<Ball>();

            if (ball != null && ball.ballCreated) {

                Destroy(gameObject);
                ball.SetDirection(transform.position);
                manager.experience += enemyPoints;
                manager.score += enemyPoints*10;
                manager.UpdatePoints();
                manager.countEnemies--;
                
            }
        } else if (other.gameObject.name.Equals("spaceship") && !(gameObject.transform.parent==other.transform.parent.transform)) {

            gameObject.transform.parent = other.transform.parent.transform;
            other.transform.parent.GetComponent<Fleet_Behaviour>().reArrangeFleet();

        }
    }

    void MovementControl() {

        if (transform.position.y < manager.cameraY) {
            Destroy(gameObject);
            manager.countEnemies--; 
        }
        else if (transform.position.x < manager.cameraX && !sideTouched) {

            sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.05f) , 0);
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = 1;
        }      
        else if (transform.position.x > (manager.cameraX*-1) && !sideTouched) {

            sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.05f) , 0);
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = -1;
        }        
        else if (transform.position.x > (manager.cameraX*-1) || transform.position.x < manager.cameraX) {

            sideTouched = false;
        }

        gameObject.GetComponent<Enemy_Behaviour>().direction = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue, gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValueY, 0);                

        stepVector = gameObject.GetComponent<Enemy_Behaviour>().direction * manager.enemySpeed * Time.deltaTime;
        rb.velocity = stepVector;
    }    
}
