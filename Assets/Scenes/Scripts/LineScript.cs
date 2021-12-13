using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public Vector3 p1Position = Vector3.zero;
    public Vector3 p2Position = Vector3.up;
    public float lineThickness = 0.01f;
    public float handlersThickness = 0.1f;
    private GameObject _p1;
    private GameObject _p2;
    private GameObject line;
    void Start()
    {
        _p1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _p1.transform.SetPositionAndRotation(p1Position, Quaternion.identity);
        _p1.transform.localScale = Vector3.one * handlersThickness;
        _p1.AddComponent<Move3D>();
        _p1.GetComponent<Move3D>().onChangeCallback += UpdateP1Position;
        // _p1.GetComponent<Move3D>().onChangeEvent.AddListener(UpdateP1Position);

        _p2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _p2.transform.SetPositionAndRotation(p2Position, Quaternion.identity);
        _p2.transform.localScale = Vector3.one * handlersThickness;
        _p2.AddComponent<Move3D>();
        // _p2.GetComponent<Move3D>().onChangeCallback += UpdateP2Position;
        _p2.GetComponent<Move3D>().onChangeEvent.AddListener(UpdateP2Position);

        line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        line.transform.SetPositionAndRotation(p1Position, Quaternion.identity);
        line.transform.up = (p2Position - p1Position);
        line.transform.localScale = Vector3.one * lineThickness;
    }

    private void OnDestroy()
    {
        new List<GameObject>(new []{_p1, _p2, line}).ForEach(Destroy);
    }

    void UpdateP1Position(GameObject p1)
    {
        p1Position = p1.transform.position;
    }
    
    void UpdateP2Position(GameObject p2)
    {
        p2Position = p2.transform.position;
    }

    void Update()
    {
        // Sync with Inspector if positions were changed there.
        _p1.transform.position = p1Position;
        _p2.transform.position = p2Position;
        
        Vector3 diff = (p2Position - p1Position);
        Vector3 orientation = diff.normalized;
        float distance = diff.magnitude;
        float lineSize = distance / 2; // cylinders have 2 units on y coord => we're making it 1 unit
        float halfDistance = distance / 2; // related to middle point
        Vector3 middlePoint = (p1Position + p2Position) / 2f;
        
        // scale
        Vector3 localScale = line.transform.localScale;
        line.transform.localScale = new Vector3(localScale.x,lineSize, localScale.z);
        
        // position
        // line.transform.position = p1Position + orientation * halfDistance;
        line.transform.position = middlePoint;
        
        // rotation
        // line.transform.rotation = Quaternion.LookRotation(orientation);
        // then
        // line.transform.rotation *= Quaternion.Euler(90, 0, 0);
        // or
        // line.transform.rotation *= Quaternion.FromToRotation(Vector3.up, Vector3.forward);
        // or just
        line.transform.up = orientation;
    }
}
