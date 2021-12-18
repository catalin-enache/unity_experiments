using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Experiments.Lib
{
    public class Move3DMovingEvent : UnityEvent<Move3D> { };

    [RequireComponent(typeof(Collider))]
    public class Move3D : MonoBehaviour
    {
        private static Color hoverColor = Color.red;
        private Camera mainCamera;
        private float cameraZDistance;
        private Boolean isDragging;
        private Vector3 screenPosition;
        private Vector3 newWorldPosition;
        public Dictionary<string, object> metaData = new Dictionary<string, object>();
        public Move3DMovingEvent OnMove3DMovingEvent = new Move3DMovingEvent();
        private Color initialColor;
    
        void Start()
        {
            mainCamera = Camera.main;
            initialColor = GetComponent<Renderer>().material.color;
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

        private void OnMouseOver()
        {
            GetComponent<Renderer>().material.color = hoverColor;
        }

        private void OnMouseExit()
        {
            GetComponent<Renderer>().material.color = initialColor;
        }

        void OnMouseDrag()
        {
            screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
            newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
            transform.position = newWorldPosition;
            OnMove3DMovingEvent.Invoke(this);
        }

        void OnGUI()
        {
            if (!isDragging) return;
            Rect rect = new Rect(20, 20, mainCamera.pixelWidth - 40, mainCamera.pixelHeight - 40);
            GUILayout.BeginArea(rect);
            GUILayout.Label("Name: " + name);
            GUILayout.Label("Screen pixels: " + mainCamera.pixelWidth + ":" + mainCamera.pixelHeight);
            GUILayout.Label("Screen position: " + screenPosition);
            GUILayout.Label("World position: " + transform.position.ToString("F3"));
            GUILayout.EndArea();
            // EditorGUI.DrawRect(rect, new Color(1,1,0,0.05f));
        }
    }
}