using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {
    [SerializeField]
    private Text gBestScore;

    [SerializeField]
    private Text bestScore;

    [SerializeField]
    private Text gScore;

    [SerializeField]
    private Text score;

	// Use this for initialization
	void Start () {
        UpdateScore();
    }
	
	public void UpdateScore()
    {
        //@todo: best score will get from save data
        bestScore.text = ": " + Util.NumberFormat(GameSave.GetInstance().GetBestScore());
        score.text = ": " + Util.NumberFormat(0);
    }
}
