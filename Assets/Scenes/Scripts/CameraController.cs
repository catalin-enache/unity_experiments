﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    private Boolean rightClicked;
    private Boolean goingUp;
    private Boolean goingDown;
    private float rotX;
    private float rotY;
    private Vector3 dir;

    private void Start()
    {
        dir = new Vector3(1, 1, 1) - transform.position;
    }

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

        // !!! good rotation
        rotX += -mouseY * 2;
        rotY += mouseX * 2;
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.eulerAngles = new Vector3(rotX, rotY, 0);
       
    }
}
