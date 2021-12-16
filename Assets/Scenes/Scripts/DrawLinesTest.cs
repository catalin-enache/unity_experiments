using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesTest : MonoBehaviour
{
    private LineScript ls;
    void Start()
    {
        GameObject obj = new GameObject("Lines Drawer");
        ls = obj.AddComponent<LineScript>();

        ls.userData.userListOfPointList.Add(new UserPointList());
        ls.userData[0].userPointList.Add(new Vector3(0,0,0));
        ls.userData[0].userPointList.Add(new Vector3(1,1,0));
        ls.userData.userListOfPointList.Add(new UserPointList());
        ls.userData[1].userPointList.Add(new Vector3(2,2,0));
        ls.userData[1].userPointList.Add(new Vector3(3,3,0));
        ls.userData[1].userPointList.Add(new Vector3(4,4,0));

        ls.onChangeCallback += OnChange;
    }

     void Update()
    {
        // Vector3 tmp = ls.userData.userListOfPointList[0][0];
        // ls.userData.userListOfPointList[0][0] = new Vector3(tmp.x + Time.deltaTime * 1, tmp.y);
    }

    void OnChange(Move3D point)
    {
        // Debug.Log(point.name);
    }
}
