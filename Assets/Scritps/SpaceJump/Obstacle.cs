using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour{
    public GameManager gameManager;
    private bool isNotScored = true;

    private void Start(){
        gameManager = FindObjectOfType<GameManager>();
    }
    
    private void Update(){
        gameObject.transform.Translate(Vector3.left * 5 * Time.deltaTime);

        if(isNotScored && gameObject.transform.position.x < -10){
            gameManager.UpdateScore();
            isNotScored = false;
        }

        if(gameObject.transform.position.x < -15){
            isNotScored = true;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // gameManager.StopGame();
    }


}
