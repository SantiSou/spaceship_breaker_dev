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
    
    void Start()
    {
        int enemySprite = Random.Range(0, 7);
        sideTouched = false;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();

        spriteRenderer.sprite = spriteArray[enemySprite];  // shaceships_0,6,12,18,24,30,36,42      
        Vector2 sizeSpriteRenderer = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = sizeSpriteRenderer;

        gameObject.name = "spaceship"+spriteArray[enemySprite].ToString().Split('_')[1];

    }

    void Update() {

        time += Time.deltaTime;

        if (transform.position.y < -5) {
            Destroy(gameObject);
        }
        else if (transform.position.x < -2.6f && !sideTouched) {

            sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.05f) , 0);
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = 1;
        }      
        else if (transform.position.x > 2.6f && !sideTouched) {

            sideTouched = true;
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.x, (gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().transform.position.y-0.05f) , 0);
            gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue = -1;
        }        
        else if (transform.position.x > -2.6f || transform.position.x < 2.6f) {

            sideTouched = false;
        }  
        
        gameObject.GetComponent<Enemy_Behaviour>().direction = new Vector3(gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValue, gameObject.transform.parent.gameObject.GetComponent<Fleet_Behaviour>().directionValueY, 0);
  
        if (time >= 0.01f) {

            stepVector = manager.enemySpeed * gameObject.GetComponent<Enemy_Behaviour>().direction;
            rb.velocity = stepVector;
            time = 0.0f;            

        }
        else {

            stepVector = 0 * gameObject.GetComponent<Enemy_Behaviour>().direction;
            rb.velocity = stepVector;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("ball_test")) {

            Ball ball = other.gameObject.GetComponent<Ball>();

            if (ball != null) {
                Destroy(gameObject);
                manager.countPoints += 100;
                manager.UpdatePoints();
                ball.SetDirection(transform.position);
            }
            
        }
    }    
}
