using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;



public class Controller : MonoBehaviour
{
    private UDPApp udpApp;
    public ArticulationBody root;
    Observations observations = new Observations();
    public List<float> velocities = new List<float>();
    public List<float> positions = new List<float>();
    List<float> forces = new List<float>();
    Actions actions;
    float u = 0;
    public float u_agent = 0;

    // Start is called before the first frame update
    void Start()
    {
        udpApp = GetComponent<UDPApp>();
        udpApp.RecieveAction += RecieveAction;
        udpApp.SendAction += SendAction;
        //udpApp.UDPStart();
        root.GetJointPositions(positions);
        root.GetJointVelocities(velocities);
        
    }

    void RecieveAction(string message)
    {
        actions = JsonUtility.FromJson<Actions>(message);
        u = actions.u;
    }

    string SendAction()
    {

        string json = JsonUtility.ToJson(observations);
        return json;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        root.GetJointPositions(positions);
        root.GetJointVelocities(velocities);
        observations.x = positions[0];
        observations.vx = velocities[0];
        observations.y = positions[1];
        observations.vy = velocities[1];
        observations.time = Time.time;
        observations.delta_time = Time.deltaTime;
        */
        //rbody.AddForce(new Vector3(1f, 0f, 0f));
    }

    void FixedUpdate()
    {

        root.GetJointPositions(positions);
        root.GetJointVelocities(velocities);

        observations.angle_pendulum = positions[1];
        observations.angular_velocity_pendulum = velocities[1];
        observations.position_car = positions[0];
        observations.velocity_car = velocities[0];    
        observations.time = Time.fixedTime;

        root.GetJointForces(forces);
        forces[0] = u + u_agent; ;
        root.SetJointForces(forces);
        
    }
}

[Serializable]
public struct Observations
{
    public float velocity_car;
    public float position_car;
    public float angular_velocity_pendulum;
    public float angle_pendulum;
    public float time;
}

[Serializable]
public struct Actions
{
    public float u;
}

