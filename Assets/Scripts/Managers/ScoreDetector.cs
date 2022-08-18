using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            other.gameObject.tag = "Untagged";
            ScoreManager.Instance.AddScore();
        }
    }
}
