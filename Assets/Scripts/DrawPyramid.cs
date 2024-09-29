using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class DrawPyramid : MonoBehaviour
{
    public float N = 10f; // Numero ingresado por el usuario
    public bool calculated = false;
    public float area;
    public float perimeter;
    public float volume;
    Vector3 vectorA;
    Vector3 vectorB;
    Vector3 vectorC;

    // Update is called once per frame
    void OnDrawGizmos()
    {
        area = 0.0f;
        perimeter = 0.0f;
        volume = 0.0f;


        float magnitudeA = 10.0f; //Hacer random
        if (!calculated)
        {
            calculated = true;
            // Generar un vector A con direcci�n aleatoria
            vectorA = Random.onUnitSphere * magnitudeA; // Magnitud aleatoria         

            // Vector B a 90� del vector A sobre el eje Z
            vectorB = GetCrossProduct(vectorA, Vector3.forward).normalized * magnitudeA;

            // Vector C a 90� del vector A y B
            vectorC = GetCrossProduct(vectorA, vectorB).normalized * (magnitudeA / N);
        }
        Debug.Log(vectorA.magnitude);
        float stepHeight = vectorC.magnitude;
        float stepWidth = magnitudeA / 2;

        // Posici�n inicial para la pir�mide
        Vector3 currentPosition = Vector3.zero;

        //Perimetro y area
        float auxiliar = magnitudeA;
        if (stepWidth >= vectorC.magnitude)
            perimeter += auxiliar * 4;

        for (int i = 0; stepWidth >= vectorC.magnitude; i++)
        {
            Vector3 baseCenter = currentPosition;
            // Definir las esquinas de la base de cada escal�n (cuadrado)
            Vector3 corner1 = baseCenter + vectorA.normalized * stepWidth;
            Vector3 corner2 = baseCenter - vectorA.normalized * stepWidth;
            Vector3 corner3 = baseCenter + vectorB.normalized * stepWidth;
            Vector3 corner4 = baseCenter - vectorB.normalized * stepWidth;


            // Dibujar las aristas de la base del escal�n
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(corner1, corner3);
            Gizmos.DrawLine(corner1, corner4);
            Gizmos.DrawLine(corner2, corner3);
            Gizmos.DrawLine(corner2, corner4);


            // Dibujar la altura (escal�n)
            //Gizmos.color = Color.green;
            Vector3 nextPosition = currentPosition + vectorC.normalized * stepHeight;
            Gizmos.DrawLine(corner1, corner1 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner2, corner2 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner3, corner3 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner4, corner4 + vectorC.normalized * stepHeight);


            //Dibujar siguiente escalon
            //Gizmos.color = Color.blue;
            Gizmos.DrawLine(corner1 + vectorC.normalized * stepHeight, corner3 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner1 + vectorC.normalized * stepHeight, corner4 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner2 + vectorC.normalized * stepHeight, corner3 + vectorC.normalized * stepHeight);
            Gizmos.DrawLine(corner2 + vectorC.normalized * stepHeight, corner4 + vectorC.normalized * stepHeight);

            //Calcular area perimetro y volumen
            area += auxiliar * vectorC.magnitude * 4; //Caras laterales
            area += auxiliar * auxiliar * 2; // Caras superiores e infioreres

            perimeter += (vectorC.magnitude * 2 + auxiliar * 2) * 4; //Caras laterales
            if (stepWidth >= vectorC.magnitude) // Caras superiores
                perimeter += ((vectorC.magnitude * 2 + auxiliar * 2) * 4 - (vectorC.magnitude * 4 * 4));
            else
                perimeter += auxiliar * 4;

            volume += auxiliar * auxiliar * vectorC.magnitude;

            // Actualizar posici�n y tama�o para el siguiente escal�n
            currentPosition = nextPosition;
            stepWidth -= vectorC.magnitude; // Reducir el tama�o de la base en cada nivel
            auxiliar -= vectorC.magnitude;
        }

    }

    Vector3 GetCrossProduct(Vector3 a, Vector3 b)
    {
        Vector3 newVector;
        float x = a.y * b.z - a.z * b.y;
        float y = a.z * b.x - a.x * b.z;
        float z = a.x * b.y - a.y * b.x;

        newVector = new Vector3(x, y, z);

        return newVector;
    }
}
