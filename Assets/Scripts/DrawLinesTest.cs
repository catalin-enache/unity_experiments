using Experiments.Lib;
using UnityEngine;

namespace Experiments
{
    public class DrawLinesTest : MonoBehaviour
    {
        private LinesDrawer ls;
        void Start() 
        {
            GameObject obj = new GameObject("Lines Drawer");
            ls = obj.AddComponent<LinesDrawer>();
            ls.userListOfPointsLists.Add(new UserPointsList());
            ls.userListOfPointsLists[0].Add(new PointColor(new Vector3(0,0,0)));
            ls.userListOfPointsLists[0].Add(new PointColor(new Vector3(1,1,0)));
            ls.userListOfPointsLists.Add(new UserPointsList());
            ls.userListOfPointsLists[1].Add(new PointColor(new Vector3(2,2,0)));
            ls.userListOfPointsLists[1].Add(new PointColor(new Vector3(3,3,0)));
            ls.userListOfPointsLists[1].Add(new PointColor(new Vector3(4,4,0)));
            ls.userListOfPointsLists[1].Add(new PointColor(new Vector3(5,5,0)));

            ls.onChangeCallback += OnChange;
        }

        void Update()
        {
            // Vector3 tmp = ls.userListOfPointsLists[0][0];
            // ls.userListOfPointsLists[0][0] = new Vector3(tmp.x + Time.deltaTime * 1, tmp.y);
        }

        void OnChange(Move3D point)
        {
            // Debug.Log(point.name);
        }
    }
}
