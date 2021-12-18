using System;
using UnityEngine;

namespace Experiments.Lib
{
    public class CameraFreeLook: MonoBehaviour
    {
        private Boolean rightClicked;
        private Boolean goingUp;
        private Boolean goingDown;
        private float rotX;
        private float rotY;

        void Update()
        {
            rightClicked = Input.GetMouseButton(1);
            if (!rightClicked) return;
        
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            goingUp = Input.GetKey(KeyCode.E);
            goingDown = Input.GetKey(KeyCode.Q);

            transform.position += transform.right * horizontal * Time.deltaTime * 10;
            transform.position += transform.forward * vertical * Time.deltaTime * 10;

            if (goingUp)
            {
                transform.position += transform.up * Time.deltaTime * 5;
            }
            if (goingDown)
            {
                transform.position -= transform.up * Time.deltaTime * 5;
            }

            rotX += -mouseY * 2;
            rotY += mouseX * 2;
            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
            // or
            // transform.eulerAngles = new Vector3(rotX, rotY, 0);
       
        }
    }
}
