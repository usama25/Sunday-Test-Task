using System;
using System.Collections;
using System.Collections.Generic;
using THEBADDEST;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    public int numOfBalls;
    
    [SerializeField] Gradient  colorToApply;
    
    private static readonly int Color = Shader.PropertyToID("_BaseColor");
    private void Start() => Init();


    void Init()
    {
        for (int i = 0; i < numOfBalls; i++)
        {
            var ball      = MasterObjectPooler.Instance.GetNew("Ball");
            Material ballMat        = ball.GetComponent<MeshRenderer>().material;
            
            ballMat.SetColor(Color,SetColorValues(i));
        }
    }


    private Color SetColorValues(int index)
    {
       return  colorToApply.Evaluate((float) index / numOfBalls);
    }
}