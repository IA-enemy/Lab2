using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //si no hay vecinos, devuelve 0 ajustes
        if(context.Count == 0)
        {
            return Vector2.zero;
        }
        //mantener todos los puntos juntos 
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //crear offset desde la posicion actual
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }


}
