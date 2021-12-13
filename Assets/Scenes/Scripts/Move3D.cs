using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ChangeEvent : UnityEvent<GameObject> { };

[RequireComponent(typeof(Collider))]
public class Move3D : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraZDistance;
    private Boolean isDragging;
    private Vector3 screenPosition;
    private Vector3 newWorldPosition;
    
    public ChangeEvent onChangeEvent = new ChangeEvent();
    
    public delegate void OnChangeCallback(GameObject gameObject);
    public event OnChangeCallback onChangeCallback;
    void Start()
    {
        mainCamera = Camera.main;
    }
 
    private void OnMouseDown()
    {
        isDragging = true;
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    void OnMouseDrag()
    {
        screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        transform.position = newWorldPosition;
        onChangeCallback?.Invoke(gameObject);
        onChangeEvent.Invoke(gameObject);
    }

    void OnGUI()
    {
        if (!isDragging) return;
        Rect rect = new Rect(20, 20, mainCamera.pixelWidth - 40, mainCamera.pixelHeight - 40);
        GUILayout.BeginArea(rect);
        GUILayout.Label("Screen pixels: " + mainCamera.pixelWidth + ":" + mainCamera.pixelHeight);
        GUILayout.Label("Screen position: " + screenPosition);
        GUILayout.Label("World position: " + transform.position.ToString("F3"));
        GUILayout.EndArea();
        // EditorGUI.DrawRect(rect, new Color(1,1,0,0.05f));
    }
}
