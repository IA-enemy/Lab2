using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockLeader : MonoBehaviour
{
    public Vector2 movementDirection; // Direcci�n de movimiento del l�der
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public float movementRadius = 10f;

    void Start()
    {
        // Inicializa con una direcci�n aleatoria
        movementDirection = Random.insideUnitCircle.normalized;
    }
    void Update()
    {

        // Movimiento aut�nomo del l�der
        transform.Translate(movementDirection * speed * Time.deltaTime);

        // Cambiar la direcci�n de movimiento (puedes implementar una l�gica m�s compleja)
        if (Random.Range(0, 100) < 1) // Ejemplo: Cambiar direcci�n aleatoriamente
        {
            movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Vector2 newPosition = (Vector2)transform.position + (Random.insideUnitCircle * speed * Time.deltaTime);

        // Comprobar si la nueva posici�n est� dentro del radio
        if (Vector2.Distance(newPosition, Vector2.zero) < movementRadius) // Asumiendo que el centro es (0,0)
        {
            transform.position = newPosition;  // Mover solo si est� dentro del radio
        }
        else
        {
            // Si est� fuera del radio, ajustar la posici�n hacia el centro
            Vector2 directionToCenter = (Vector2.zero - newPosition).normalized;
            transform.position += (Vector3)directionToCenter * speed * Time.deltaTime;
        }
    }
}