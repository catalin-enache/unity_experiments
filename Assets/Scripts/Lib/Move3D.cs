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
        private Camera mainCamera;
        private Boolean isDragging;
        private Vector3 screenPosition;
        private Vector3 newWorldPosition;
        private float offsetX;
        private float offsetY;
        private Boolean isMouseOver;
        
        public Dictionary<string, object> metaData = new Dictionary<string, object>();
        public Move3DMovingEvent OnMove3DMovingEvent = new Move3DMovingEvent();
        public Move3DMovingEvent OnMove3DMouseOver = new Move3DMovingEvent();
        public Move3DMovingEvent OnMove3DMouseExit = new Move3DMovingEvent();
    
        void Start()
        {
            mainCamera = Camera.main;
            CameraFreeLook cameraFreeLook = mainCamera.GetComponent<CameraFreeLook>();
            if (cameraFreeLook != null) cameraFreeLook.OnCameraMove += UpdateScreenPosition;
            UpdateScreenPosition();
            transform.hasChanged = false;
        }

        private void OnDestroy()
        {
            if (mainCamera == null) return;
            CameraFreeLook cameraFreeLook = mainCamera.GetComponent<CameraFreeLook>();
            if (cameraFreeLook != null) cameraFreeLook.OnCameraMove -= UpdateScreenPosition;
        }

        private void OnMouseDown()
        {
            UpdateScreenPosition();
            offsetX = Input.mousePosition.x - screenPosition.x;
            offsetY = Input.mousePosition.y - screenPosition.y;
            isDragging = true;
        }

        private void OnMouseUp()
        {
            isDragging = false;
            transform.hasChanged = false;
        }

        private void OnMouseOver()
        {
            isMouseOver = true;
            UpdateScreenPosition();
            OnMove3DMouseOver.Invoke(this);
        }
        
        private void OnMouseExit()
        {
            isMouseOver = false;
            OnMove3DMouseExit.Invoke(this);
        }

        void OnMouseDrag()
        {
            float newXPos = Input.mousePosition.x - offsetX;
            float newYPos = Input.mousePosition.y - offsetY;
            screenPosition = new Vector3(newXPos, newYPos, screenPosition.z);
            newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
            transform.localPosition = newWorldPosition;
            OnMove3DMovingEvent.Invoke(this);
        }

        void UpdateScreenPosition()
        {
            screenPosition = mainCamera.WorldToScreenPoint(transform.localPosition);
        }

        private void OnGUI()
        {
            if (!isMouseOver) return;
            
            GUI.skin.label.fontSize = 10;
            GUI.contentColor = Color.cyan;
            
            GUILayout.BeginArea(new Rect(screenPosition.x + 10, -screenPosition.y + mainCamera.pixelHeight - 10 , 250, 50));
            GUILayout.Label(name + " " + transform.localPosition.ToString("F2"));
            GUILayout.EndArea();
            
            if (!isDragging) return;
            
            Rect rect = new Rect(20, 20, mainCamera.pixelWidth - 40, mainCamera.pixelHeight - 40);
            GUILayout.BeginArea(rect);
            GUILayout.Label("Name: " + name);
            GUILayout.Label("Screen pixels: " + mainCamera.pixelWidth + ":" + mainCamera.pixelHeight);
            GUILayout.Label("Screen position: " + screenPosition);
            GUILayout.Label("World position: " + transform.localPosition.ToString("F3"));
            GUILayout.EndArea();
            // EditorGUI.DrawRect(rect, new Color(1,1,0,0.05f));
        }
    }
}