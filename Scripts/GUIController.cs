using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GUIController : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;

    private Label speedUpLabel;
    private Label scoreLabel;
    private Button restartButton;


    // Start is called before the first frame update
    void Start()
    {

        UIDocument document = GetComponent<UIDocument>();

        scoreLabel = document.rootVisualElement.Q("Score") as Label;
        speedUpLabel = document.rootVisualElement.Q("SpeedUpMessage") as Label;

        restartButton = document.rootVisualElement.Q("RestartButton") as Button;
        restartButton.RegisterCallback<ClickEvent>(RestartGame);

    }

    // Update is called once per frame
    void Update()
    {
        speedUpLabel.visible = gameManager.displaySpeedUpMessage;
        scoreLabel.text = "SCORE: " + gameManager.score;

        bool restartButtonStatus = !gameManager.player.gameObject.activeInHierarchy;
        restartButton.visible = restartButtonStatus;
        restartButton.SetEnabled(restartButtonStatus);

    }

    void RestartGame(ClickEvent evt){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
