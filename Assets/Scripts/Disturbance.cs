using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour
{
    ArticulationBody abody;
    public float magnitude = 0;
    public bool random;
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

        if (random && UnityEngine.Random.Range(0, 100) < 0.000001f)
        {
            abody.AddForce(Vector3.forward * UnityEngine.Random.Range(-magnitude, magnitude), ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {

    }
}
