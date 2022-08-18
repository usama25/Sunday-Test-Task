using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysicalProperties : MonoBehaviour
{
    public PhysicMaterial mPhysicMaterial;

    [Header("Set Physical Properties")]
    
    [Range(0,1)]
    public float m_Bounciness;

    [Range(0,20)]
    public float m_Drag;
    
    [Range(0.1f,100)]
    public float m_Mass;

    private void Start()
    {
        SetPhysicalProperties();
    }

    void SetPhysicalProperties()
    {
        mPhysicMaterial.bounciness = m_Bounciness;
        
        var rb  = GetComponent<Rigidbody>();
        rb.mass = m_Mass;
        rb.drag = m_Drag;
    }
}
