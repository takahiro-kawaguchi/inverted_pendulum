using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AcrobotAgent : Agent
{
    public ArticulationBody abody;
    [SerializeField]
    List<float> forces;
    [SerializeField]
    List<float> positions;
    [SerializeField]
    List<float> velocities;
    public float gain;
    [Range(-10, 10)]
    public float u;
    // Start is called before the first frame update
    void Start()
    {
        forces = new();
        positions = new();
        velocities = new();
        abody.GetJointPositions(positions);
        abody.GetJointVelocities(velocities);
        abody.GetJointForces(forces);
    }

    public override void OnEpisodeBegin()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i] = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
            velocities[i] = 0;
        }
        abody.SetJointVelocities(velocities);
        abody.SetJointPositions(positions);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        abody.GetJointPositions(positions);
        abody.GetJointVelocities(velocities);
        abody.GetJointForces(forces);
        sensor.AddObservation(ClipRad(positions[0] + Mathf.PI));
        sensor.AddObservation(ClipRad(positions[1]));
        sensor.AddObservation(velocities);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float uin = actionBuffers.ContinuousActions[0];
        forces[1] = gain * uin;
        abody.SetJointForces(forces);
        float r = GetReward(uin);
        SetReward(r);
        //Debug.Log(r);
        Debug.Log(GetCumulativeReward());
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = u;
    }

    float ClipRad(float theta)
    {
        return Mathf.Atan2(Mathf.Sin(theta), Mathf.Cos(theta));
    }

    float GetReward(float u)
    {
        abody.GetJointPositions(positions);
        float theta1 = ClipRad(positions[0] + Mathf.PI) / Mathf.PI;
        float theta2 = ClipRad(positions[1]) / Mathf.PI;
        float r = -Mathf.Abs(theta1) - Mathf.Abs(theta2) + 1;
        r -= Mathf.Pow(u, 2);
        return r;
    }

}
