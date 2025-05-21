using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform centerPoint; // Центральная точка, вокруг которой будет вращаться круг
    public float rotationSpeed = 180f; // Скорость вращения круга
    public GameObject crystalPrefab; // Префаб кристалла
    GameObject crystal;
    public Transform player;

    private void Start()
    {

        crystal = Instantiate(crystalPrefab, centerPoint.position + Vector3.forward + Vector3.up, Quaternion.identity);
        crystal.transform.parent = player;
        crystal.name = "CrystalRotationgAround";
        
    }


    private void Update()
    {
        crystal.GetComponent<Transform>().RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);


        // Вычисляем новую позицию для круга вокруг центральной точки
        Vector3 offset = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0f) * (transform.position - centerPoint.position);

        //var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        //var type = assembly.GetType("UnityEditor.LogEntries");
        //var method = type.GetMethod("Clear");
        //method.Invoke(new object(), null);

        transform.position = centerPoint.position + offset;


        Vector3 interpolatedPosition = Vector3.Lerp(Vector3.up, Vector3.forward, 0.5f);

        Debug.DrawLine(Vector3.zero, Vector3.up, Color.green);
        Debug.DrawLine(Vector3.zero, Vector3.forward, Color.blue);
        Debug.DrawLine(Vector3.zero, interpolatedPosition, Color.yellow);

    }
}
