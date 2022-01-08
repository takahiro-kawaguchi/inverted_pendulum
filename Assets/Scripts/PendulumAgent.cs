using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PendulumAgent : Agent
{
    public Settings settings;
    public Controller controller;
    float width;
    // Start is called before the first frame update
    void Start()
    {
        //settings = transform.parent.GetComponent<Settings>();
        //controller = GetComponent<Controller>();
        ArticulationDrive drive = controller.root.transform.Find("CarJoint").GetComponent<ArticulationBody>().zDrive;
        width = (drive.upperLimit - drive.lowerLimit)/2f;
    }

    public override void OnEpisodeBegin()
    {
        
        settings.Initialize();
        controller.u_agent = 0;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(target.transform.position.y);
        //        sensor.AddObservation(hand.transform.position.x);
        //        sensor.AddObservation(hand.transform.position.y);
        //        sensor.AddObservation(hand.transform.position.z);
        float theta = controller.positions[1];
        theta = Mathf.Atan2(Mathf.Sin(theta), Mathf.Cos(theta));
        controller.positions[1] = theta;
        sensor.AddObservation(controller.positions[0]);
        sensor.AddObservation(controller.positions[1]);
        sensor.AddObservation(controller.velocities[0]);
        sensor.AddObservation(controller.velocities[1]);
        //Debug.Log(target_tire.transform.position.x - target.transform.position.x);
        //Debug.Log(target_tire.transform.position.y - target.transform.position.y);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        controller.u_agent = 100f * actionBuffers.ContinuousActions[0];
        float theta = controller.positions[1];
        theta = Mathf.Atan2(Mathf.Sin(theta), Mathf.Cos(theta));
        float r = -Mathf.Abs(theta)/Mathf.PI + 0.5f;
        SetReward(r);
        r = (-Mathf.Abs(controller.positions[0] / width) + 0.5f)*0.1f;
        AddReward(r);
        if (Mathf.Abs(controller.positions[0]) > width - 0.1)
        {
            EndEpisode();
        }
        /*if (Mathf.Abs(theta) > Mathf.PI/4 || Mathf.Abs(controller.positions[0])>13)
        {
            EndEpisode();
        }
        */
        /// Debug.Log(controller.positions[0]);

        //Debug.Log((-Mathf.Abs(theta) / Mathf.PI - Mathf.Abs(controller.positions[0])/width) + 1);
    }

}
