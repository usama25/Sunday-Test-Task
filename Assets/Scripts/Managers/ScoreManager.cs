using System;
using System.Collections;
using System.Collections.Generic;
using GameAssets.GameSet.GameDevUtils.Managers;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField]  private TMP_Text   scoreText;
    [SerializeField]  private GameObject confettiParticles;
    [HideInInspector] public  int        currentScore, totalScore;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
           Destroy(gameObject);
    }


    private void Start()
    {
        totalScore = 20;
        scoreText.text = currentScore + "/" + totalScore;
    }

    public void AddScore()
    {
        if (currentScore >= totalScore)
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
            MMVibrationManager.Haptic(HapticTypes.Success);
            confettiParticles.SetActive(true);
            return;
        }

        
        currentScore++;
        scoreText.text = currentScore + "/" + totalScore;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }
}
