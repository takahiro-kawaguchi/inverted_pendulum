using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour
{
    public ArticulationBody abody;
    public float disturbance = 1;
    // Start is called before the first frame update
    void Start()
    {
        abody = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            abody.AddForce(Vector3.forward * disturbance, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {

    }
}
