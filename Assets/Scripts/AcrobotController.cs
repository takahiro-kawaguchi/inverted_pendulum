using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrobotController : MonoBehaviour
{
    public ArticulationBody abody;
    List<float> forces;
    [Range(-10, 10)]
    public float u;
    // Start is called before the first frame update
    void Start()
    {
        forces = new();
        abody.GetJointForces(forces);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        forces[1] = u;
        abody.SetJointForces(forces);
    }
}
