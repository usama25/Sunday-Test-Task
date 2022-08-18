using System;
using System.Collections;
using System.Collections.Generic;
using GameAssets.GameSet.GameDevUtils.Managers;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class CheckBalls : MonoBehaviour
{
    public BallsManager ballsManager;
    private int counter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            counter++;

            if (counter >= ballsManager.numOfBalls && GameManager.Instance.GameCurrentState == GameState.Gameplay)
            {
                GameManager.Instance.ChangeGameState(GameState.Fail);
                MMVibrationManager.Haptic(HapticTypes.Failure);                
            }

        }
    }
}
