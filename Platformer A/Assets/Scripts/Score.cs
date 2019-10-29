using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private MovementScript player;

    [SerializeField]
    private Text _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";

    }
    public void UpdatePlayerScore(int playerScore)
    {
        _scoreText.text = $"Score: {playerScore}";
    }

}
