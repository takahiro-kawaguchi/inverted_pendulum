using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour
{
    ArticulationBody abody;
    public float magnitude = 0;
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
            abody.AddForce(Vector3.forward * magnitude, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {

    }
}
