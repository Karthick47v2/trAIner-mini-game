using UnityEngine;

public class SpaceShip : MonoBehaviour{
    public Visual visual;
    private bool jumpSwitch = false;
    private int score = 0;

    private void FixedUpdate(){
        if(!jumpSwitch){
            if(visual.conf > 0.3 && visual.x < 0.1){
                jumpSwitch = true;
            }
        }
        else if(jumpSwitch && visual.conf > 0.3 && visual.x > 0.5){
            jumpSwitch = false;
            gameObject.transform.Translate(Vector3.up * 5 * 0.2f);
            Debug.Log(++score);
        }
    }
}
