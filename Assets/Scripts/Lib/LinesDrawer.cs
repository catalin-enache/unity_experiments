﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Experiments.Lib
{
    [System.Serializable]
    public class PointColor
    {
        public Vector3 position;
        [HideInInspector] public Color color;
        [HideInInspector] public Color hoverColor;
        [HideInInspector] public Color lineColor;
        [HideInInspector] public float thickness;
        [HideInInspector] public float lineThickness;
        
        public PointColor(Vector3 position, Color? color = null, Color? hoverColor = null, float thickness = 0.1f, float lineThickness = 0.01f, Color? lineColor = null)
        {
            this.position = position;
            this.color = color ?? Color.white;
            this.hoverColor = hoverColor ?? Color.red;
            this.lineColor = lineColor ?? Color.white;
            this.thickness = thickness;
            this.lineThickness = lineThickness;
        }


        public override string ToString()
        {
            return position.ToString();
        }
    }
    
    [System.Serializable] 
    public class UserPointsList : List<PointColor>
    {
        [SerializeField]
        private List<PointColor> userPointsList;

        public UserPointsList()
        {
            userPointsList = this;
        }
    
        public override string ToString()
        {
            return "UserPointsList: " + userPointsList.Select(v => v.ToString()).Aggregate((acc, item) => acc + ", " + item);
        }
    }

    [System.Serializable] 
    public class UserListOfPointsLists : List<UserPointsList>
    {
        [SerializeField]
        private List<UserPointsList> userListOfPointsLists;

        public UserListOfPointsLists()
        {
            userListOfPointsLists = this;
        }
    
        public override string ToString()
        {
            return "UserListOfPointsLists: " + userListOfPointsLists.Select(pointList => pointList.ToString()).Aggregate((acc, item) => acc + ", " +  item);
        }
    }

    public class LinesDrawer : MonoBehaviour
    {
        public UserListOfPointsLists userListOfPointsLists = new UserListOfPointsLists();
        private List<List<GameObject>> gameObjectListOfPointList = new List<List<GameObject>>();
        private List<List<GameObject>> gameObjectListOfLineList = new List<List<GameObject>>();
        
        public delegate void OnChange(Move3D gameObject);
        public event OnChange onChangeCallback;

        void Start()
        {
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                UserPointsList userPointsList = userListOfPointsLists[i];
                List<GameObject> gameObjectPointList = new List<GameObject>();
                List<GameObject> gameObjectLineList = new List<GameObject>();
                gameObjectListOfPointList.Add(gameObjectPointList);
                gameObjectListOfLineList.Add(gameObjectLineList);
                PointColor prevPoint = new PointColor(Vector3.zero);
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    PointColor point = userPointsList[j];
                    GameObject pointObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointObject.GetComponent<Renderer>().material.color = point.color;
                    pointObject.name = "Point: " + i + ", " + j;
                    pointObject.transform.SetPositionAndRotation(point.position, Quaternion.identity);
                    pointObject.transform.localScale = Vector3.one * point.thickness;
                    Move3D m3d = pointObject.AddComponent<Move3D>();
                    m3d.metaData.Add("listIndex", i);
                    m3d.metaData.Add("pointIndex", j);
                    m3d.metaData.Add("hoverColor", point.hoverColor);
                    m3d.OnMove3DMovingEvent.AddListener(UpdatePointPosition);
                    m3d.OnMove3DMouseOver.AddListener(OnMove3DMouseOver);
                    m3d.OnMove3DMouseExit.AddListener(OnMove3DMouseExit);
                    gameObjectPointList.Add(pointObject);

                    if (j > 0)
                    {
                        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        line.GetComponent<Renderer>().material.color = point.lineColor;
                        line.name = "Line: " + i + ", " + (j - 1);
                        line.transform.SetPositionAndRotation(prevPoint.position, Quaternion.identity);
                        line.transform.up = (point.position - prevPoint.position);
                        line.transform.localScale = Vector3.one * point.lineThickness;
                        gameObjectLineList.Add(line);
                    };

                    prevPoint = point;
                }
            }
        }
    
        void Update()
        {
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                UserPointsList userPointsList = userListOfPointsLists[i];
                PointColor prevPoint = new PointColor(Vector3.zero);
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    // Sync with Inspector if positions were changed there.
                    PointColor point = userPointsList[j];
                    gameObjectListOfPointList[i][j].transform.position = point.position;
                
                    if (j > 0)
                    {
                        GameObject line = gameObjectListOfLineList[i][j - 1];
                        Vector3 diff = (prevPoint.position - point.position);
                        Vector3 orientation = diff.normalized;
                        float distance = diff.magnitude;
                        float lineSize = distance / 2; // cylinders have 2 units on y coord => we're making it 1 unit
                        float halfDistance = distance / 2; // related to middle point
                        Vector3 middlePoint = (point.position + prevPoint.position) / 2;
                    
                        // scale
                        Vector3 localScale = line.transform.localScale;
                        line.transform.localScale = new Vector3(localScale.x,lineSize, localScale.z);
                    
                        // position
                        // line.transform.position = point.position + orientation * halfDistance;
                        line.transform.position = middlePoint;
                    
                        // rotation
                        // line.transform.rotation = Quaternion.LookRotation(orientation);
                        // then
                        // line.transform.rotation *= Quaternion.Euler(90, 0, 0);
                        // or
                        // line.transform.rotation *= Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                        // or just
                        line.transform.up = orientation;
                    };
                    prevPoint = point;
                }
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                UserPointsList userPointsList = userListOfPointsLists[i];
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    Destroy(gameObjectListOfPointList[i][j]);
                    if (j > 0)
                    {
                        Destroy(gameObjectListOfLineList[i][j - 1]);
                    }
                }
            }
        }

        void UpdatePointPosition(Move3D obj)
        {
            PointColor point = userListOfPointsLists[(int) obj.metaData["listIndex"]][(int) obj.metaData["pointIndex"]];
            point.position = obj.transform.position;
            onChangeCallback?.Invoke(obj);
        }
        
        private void OnMove3DMouseOver(Move3D obj)
        {
            PointColor point = userListOfPointsLists[(int) obj.metaData["listIndex"]][(int) obj.metaData["pointIndex"]];
            obj.GetComponent<Renderer>().material.color = point.hoverColor;
        }

        private void OnMove3DMouseExit(Move3D obj)
        {
            PointColor point = userListOfPointsLists[(int) obj.metaData["listIndex"]][(int) obj.metaData["pointIndex"]];
            obj.GetComponent<Renderer>().material.color = point.color;
        }
    
    }
}