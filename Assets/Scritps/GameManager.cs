using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{
    private int playerScore = 0;
    public TextMeshProUGUI score;
    public GameObject playPanel, player;

    private void Awake() {
        Time.timeScale = 0;
    }

    public void StartClick(){
        playPanel.SetActive(false);
        player.SetActive(true);
        score.enabled = true;
        Time.timeScale = 1;
    }
    public void StopGame(){
        Time.timeScale = 0;
    }

    public void UpdateScore(){
        score.text = playerScore.ToString();
    }
}
