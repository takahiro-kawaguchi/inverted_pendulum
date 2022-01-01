using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disturbance : MonoBehaviour
{
    public ArticulationBody abody;
    public float disturbance = 50;
    bool pushed = false;
    // Start is called before the first frame update
    void Start()
    {
        abody = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            pushed = true;
        }
    }

    private void FixedUpdate()
    {
        if (pushed)
        {
            pushed = false;
            Debug.Log("test");
            abody.AddForce(new Vector3(0, disturbance, 0));
        }
    }
}
