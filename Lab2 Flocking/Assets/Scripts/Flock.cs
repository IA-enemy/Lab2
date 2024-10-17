using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public FlockLeader leaderPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    List<FlockLeader> leaders = new List<FlockLeader>();
    public FlockBehavior behaviour;
    

    [Range(10, 500)]
    public int startingCount = 250;
    [Range(1, 10)]
    public int leaderCount = 5;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingCount * AgentDensity, Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), transform);
            newAgent.name = "Agent" + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
        for (int i = 0; i < leaderCount; i++)
        {
            FlockLeader newLeader = Instantiate(leaderPrefab, Random.insideUnitCircle * startingCount * AgentDensity, Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), transform);
            newLeader.name = "Leader" + i;
            leaders.Add(newLeader);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);
            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;

            //Buscar y seguir al lider cercano
            FlockLeader closestLeader = FindClosestLeader(agent, context);
            if (closestLeader != null)
            {
                agent.UpdateLeader(closestLeader);
                Vector2 directionToLeader = (Vector2)closestLeader.transform.position - (Vector2)agent.transform.position;
                move+= directionToLeader.normalized * 2;
            }
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        FlockLeader[] leaders = FindObjectsOfType<FlockLeader>();
        foreach (FlockLeader leader in leaders)
        {
            context.Add(leader.transform);
        }

        return context;
    }

    //private FlockLeader FindClosestLeader(FlockAgent agent, List<Transform> context)
    //{
    //    FlockLeader closestLeader = null;
    //    float cloasetLeaderDistance = Mathf.Infinity;

    //    foreach (Transform item in context)
    //    {
    //        FlockLeader leader = item.GetComponent<FlockLeader>();
    //        if (leader != null)
    //        {
    //            float distancetoLeader = Vector2.Distance(agent.transform.position, leader.transform.position);
    //            if (distancetoLeader < cloasetLeaderDistance)
    //            {
    //                cloasetLeaderDistance = distancetoLeader;
    //                closestLeader = leader;
    //            }
    //        }
    //    }
    //    return closestLeader;
    //}
    FlockLeader FindClosestLeader(FlockAgent agent, List<Transform> context)
    {
        FlockLeader closestLeader = null;
        float closestDistance = Mathf.Infinity;

        FlockLeader[] leaders = FindObjectsOfType<FlockLeader>();
        foreach (FlockLeader leader in leaders)
        {
            float distance = Vector2.Distance(agent.transform.position, leader.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestLeader = leader;
            }
        }

        return closestLeader;
    }
}
