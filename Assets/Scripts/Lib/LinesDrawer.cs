using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Experiments.Lib
{
    [System.Serializable] 
    public class UserPointList : List<Vector3>
    {
        [SerializeField]
        private List<Vector3> userPointList;

        public UserPointList()
        {
            userPointList = this;
        }
    
        public override string ToString()
        {
            return "UserPointList: " + userPointList.Select(v => v.ToString()).Aggregate((acc, item) => acc + ", " + item);
        }
    }

    [System.Serializable] 
    public class UserListOfPointList : List<UserPointList>
    {
        [SerializeField]
        private List<UserPointList> userListOfPointList;

        public UserListOfPointList()
        {
            userListOfPointList = this;
        }
    
        public override string ToString()
        {
            return "UserListOfPointList: " + userListOfPointList.Select(pointList => pointList.ToString()).Aggregate((acc, item) => acc + ", " +  item);
        }
    }

    public class LinesDrawer : MonoBehaviour
    {
        public UserListOfPointList userListOfPointList = new UserListOfPointList();
        private List<List<GameObject>> gameObjectListOfPointList = new List<List<GameObject>>();
        private List<List<GameObject>> gameObjectListOfLineList = new List<List<GameObject>>();

        public float initialLineThickness = 0.01f;
        public float initialHandlersThickness = 0.1f;
        public Color initialHandlersColor = Color.green;
        public Color initialLineColor = Color.yellow;
    
        public delegate void OnChange(Move3D gameObject);
        public event OnChange onChangeCallback;

        void Start()
        {
            for (int i = 0; i < userListOfPointList.Count; i++)
            {
                UserPointList userPointList = userListOfPointList[i];
                List<GameObject> gameObjectPointList = new List<GameObject>();
                List<GameObject> gameObjectLineList = new List<GameObject>();
                gameObjectListOfPointList.Add(gameObjectPointList);
                gameObjectListOfLineList.Add(gameObjectLineList);
                Vector3 prevPoint = Vector3.zero;
                for (int j = 0; j < userPointList.Count; j++)
                {
                    Vector3 point = userPointList[j];
                    GameObject pointObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointObject.GetComponent<Renderer>().material.color = initialHandlersColor;
                    pointObject.name = "Point: " + i + ", " + j;
                    pointObject.transform.SetPositionAndRotation(point, Quaternion.identity);
                    pointObject.transform.localScale = Vector3.one * initialHandlersThickness;
                    Move3D m3d = pointObject.AddComponent<Move3D>();
                    m3d.metaData.Add("listIndex", i);
                    m3d.metaData.Add("pointIndex", j);
                    m3d.OnMove3DMovingEvent.AddListener(UpdatePointPosition);
                    gameObjectPointList.Add(pointObject);

                    if (j > 0)
                    {
                        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        line.GetComponent<Renderer>().material.color = initialLineColor;
                        line.name = "Line: " + i + ", " + (j - 1);
                        line.transform.SetPositionAndRotation(prevPoint, Quaternion.identity);
                        line.transform.up = (point - prevPoint);
                        line.transform.localScale = Vector3.one * initialLineThickness;
                        gameObjectLineList.Add(line);
                    };

                    prevPoint = point;
                }
            }
        }
    
        void Update()
        {
            for (int i = 0; i < userListOfPointList.Count; i++)
            {
                UserPointList userPointList = userListOfPointList[i];
                Vector3 prevPoint = Vector3.zero;
                for (int j = 0; j < userPointList.Count; j++)
                {
                    // Sync with Inspector if positions were changed there.
                    Vector3 point = userPointList[j];
                    gameObjectListOfPointList[i][j].transform.position = point;
                
                    if (j > 0)
                    {
                        Debug.Log("line");
                        GameObject line = gameObjectListOfLineList[i][j - 1];
                        Vector3 diff = (prevPoint - point);
                        Vector3 orientation = diff.normalized;
                        float distance = diff.magnitude;
                        float lineSize = distance / 2; // cylinders have 2 units on y coord => we're making it 1 unit
                        float halfDistance = distance / 2; // related to middle point
                        Vector3 middlePoint = (point + prevPoint) / 2;
                    
                        // scale
                        Vector3 localScale = line.transform.localScale;
                        line.transform.localScale = new Vector3(localScale.x,lineSize, localScale.z);
                    
                        // position
                        // line.transform.position = point + orientation * halfDistance;
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
            for (int i = 0; i < userListOfPointList.Count; i++)
            {
                UserPointList userPointList = userListOfPointList[i];
                for (int j = 0; j < userPointList.Count; j++)
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
            userListOfPointList[(int)obj.metaData["listIndex"]][(int)obj.metaData["pointIndex"]] = obj.transform.position;
            onChangeCallback?.Invoke(obj);
        }
    
    }
}