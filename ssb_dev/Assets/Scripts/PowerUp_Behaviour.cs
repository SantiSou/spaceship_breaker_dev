using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Behaviour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    public Vector3 direction = new Vector3(0,-1,0);
    Vector3 stepVector;
    Rigidbody2D rb; 
    public GameManager manager;
    // Start is called before the first frame update

    void Start() {
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int enemySprite = Random.Range(0, 4);
        spriteRenderer.sprite = spriteArray[enemySprite];         
        
    }

    // Update is called once per frame
    void Update() {
        stepVector = manager.enemySpeed * direction.normalized;
        rb.velocity = stepVector;
        if(transform.position.y < -5)
        {
			Destroy(gameObject);
        }       
    }
}


