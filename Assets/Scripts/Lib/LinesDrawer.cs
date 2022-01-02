using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        [HideInInspector] public bool activeSelf;
        
        public PointColor(Vector3 position, Color? color = null, Color? hoverColor = null, float thickness = 0.1f, float lineThickness = 0.01f, Color? lineColor = null, bool activeSelf = true)
        {
            this.position = position;
            this.color = color ?? Color.white;
            this.hoverColor = hoverColor ?? Color.red;
            this.lineColor = lineColor ?? Color.white;
            this.thickness = thickness;
            this.lineThickness = lineThickness;
            this.activeSelf = activeSelf;
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
        private Material transparentMaterial = null;
        private List<List<GameObject>> gameObjectListOfPointsLists = new List<List<GameObject>>();
        private List<List<GameObject>> gameObjectListOfLinesLists = new List<List<GameObject>>();
        public UserListOfPointsLists userListOfPointsLists = new UserListOfPointsLists();
        public Boolean isInitiallyInteractable = true;
        public Boolean shouldHideInHierarchy = true;
        public bool isControllingViaUserPoints = true;
        
        public delegate void OnChange(Move3D gameObject);
        public event OnChange onChangeCallback;

        void Start()
        {
            transparentMaterial = Resources.Load<Material>("Materials/mat_WhiteTransparent");
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                UserPointsList userPointsList = userListOfPointsLists[i];
                List<GameObject> gameObjectPointsList = new List<GameObject>();
                List<GameObject> gameObjectLinesList = new List<GameObject>();
                gameObjectListOfPointsLists.Add(gameObjectPointsList);
                gameObjectListOfLinesLists.Add(gameObjectLinesList);
                PointColor prevPoint = new PointColor(Vector3.zero);
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    PointColor point = userPointsList[j];
                    GameObject pointObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointObject.GetComponent<Renderer>().material = transparentMaterial;
                    pointObject.SetActive(userPointsList[j].activeSelf);
                    if (!isInitiallyInteractable || shouldHideInHierarchy)
                    {
                        pointObject.hideFlags = HideFlags.HideInHierarchy;
                    }
                    pointObject.GetComponent<Renderer>().material.color = point.color;
                    pointObject.name = "P: " + i + ", " + j;
                    pointObject.transform.SetPositionAndRotation(point.position, Quaternion.identity);
                    pointObject.transform.localScale = Vector3.one * point.thickness;
                    gameObjectPointsList.Add(pointObject);
                    
                    Move3D m3d = pointObject.AddComponent<Move3D>();
                    m3d.metaData.Add("listIndex", i);
                    m3d.metaData.Add("pointIndex", j);
                    m3d.OnMove3DMove.AddListener(UpdatePointPosition);
                    m3d.OnMove3DMouseOver.AddListener(OnMove3DMouseOver);
                    m3d.OnMove3DMouseExit.AddListener(OnMove3DMouseExit);
                    m3d.isMovable = isInitiallyInteractable;
                    
                    if (j > 0)
                    {
                        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        line.hideFlags = HideFlags.HideInHierarchy;
                        line.GetComponent<Renderer>().material = transparentMaterial;
                        line.GetComponent<Renderer>().material.color = point.lineColor;
                        line.name = "Line: " + i + ", " + (j - 1);
                        line.transform.SetPositionAndRotation(prevPoint.position, Quaternion.identity);
                        line.transform.up = (point.position - prevPoint.position);
                        line.transform.localScale = Vector3.one * point.lineThickness;
                        gameObjectLinesList.Add(line);
                    };

                    prevPoint = point;
                }
            }
            UpdatePositions();
        }
    
        void Update()
        {
            if (!isInitiallyInteractable) return;
            UpdatePositions();
        }

        void UpdatePositions()
        {
            if (userListOfPointsLists.Count != gameObjectListOfPointsLists.Count)
            {
                // should not need to be checked but sometimes these arrays are not in sync
                return;
            }
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                if (userListOfPointsLists[i].Count != gameObjectListOfPointsLists[i].Count)
                {
                    // should not need to be checked but sometimes these arrays are not in sync
                    return;
                }
                UserPointsList userPointsList = userListOfPointsLists[i];
                
                PointColor prevPoint = new PointColor(Vector3.zero);
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    // Sync with Inspector if positions were changed there.
                    PointColor point = userPointsList[j];
                    GameObject gameObjectPoint  = gameObjectListOfPointsLists[i][j];

                    if (gameObjectPoint.transform.position != point.position)
                    {
                        if (isControllingViaUserPoints)
                        {
                            // game object obeys userPoint (can use userPoints, scene gizmos are locked)
                            // good for scripting
                            gameObjectPoint.transform.position = point.position;
                        }
                        else
                        {
                            // userPoint obeys game object (can use scene gizmos, userPoints are locked)
                            point.position = gameObjectPoint.transform.position;
                        }
                    }

                    if (j > 0 && (gameObjectPoint.transform.hasChanged || gameObjectListOfPointsLists[i][j - 1].transform.hasChanged))
                    {
                        GameObject line = gameObjectListOfLinesLists[i][j - 1];
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
                        
                        gameObjectListOfPointsLists[i][j - 1].transform.hasChanged = false;
                    };
                    
                    Boolean isLastPointInList = j == userPointsList.Count - 1;
                    if (isLastPointInList)
                    {
                        gameObjectPoint.transform.hasChanged = false;
                    }
  
                    prevPoint = point;
                }
            }
        }

        private void OnDestroy()
        {
            if (userListOfPointsLists.Count != gameObjectListOfPointsLists.Count)
            {
                // should not need to be checked but sometimes these arrays are not in sync
                return;
            }
            for (int i = 0; i < userListOfPointsLists.Count; i++)
            {
                if (userListOfPointsLists[i].Count != gameObjectListOfPointsLists[i].Count)
                {
                    // should not need to be checked but sometimes these arrays are not in sync
                    return;
                }
                UserPointsList userPointsList = userListOfPointsLists[i];
                for (int j = 0; j < userPointsList.Count; j++)
                {
                    Destroy(gameObjectListOfPointsLists[i][j]);
                    if (j > 0)
                    {
                        Destroy(gameObjectListOfLinesLists[i][j - 1]);
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