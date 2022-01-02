using UnityEngine;
using UnityEngine.UI;

namespace Experiments.Lib
{
    public class CartesianCoordsDrawer : MonoBehaviour
    {
        private LinesDrawer ld;
        private readonly  UserPointsList xCoordList = new UserPointsList();
        private readonly  UserPointsList yCoordList = new UserPointsList();
        private readonly  UserPointsList zCoordList = new UserPointsList();
        private enum _coords
        {
            x,
            y,
            z
        };
        public int unitsNum = 5;
        void Start()
        {
            ld = gameObject.AddComponent<LinesDrawer>();
            ld.isInitiallyInteractable = false;
            ld.userListOfPointsLists.Add(xCoordList);
            ld.userListOfPointsLists.Add(yCoordList);
            ld.userListOfPointsLists.Add(zCoordList);

            DrawCoord(_coords.x);
            DrawCoord(_coords.y);
            DrawCoord(_coords.z);
        }

        void DrawCoord(_coords coord)
        {
            float transparency = 0.3f;
            // if coord is X
            Vector3 vector = Vector3.right;
            Color pointColor = new Color(1,0,0,transparency);
            Color smallPointColor = new Color(0.3f,0,0, transparency);
            Color lineColor = new Color(1,0,0, transparency);
            UserPointsList list = xCoordList;
            
            float pointThickness = 0.09f;
            float smallPointThickness = 0.05f;
            float lineThickness = 0.009f;

            if (coord == _coords.y)
            {
                vector = Vector3.up;
                pointColor = new Color(0,1,0, transparency);
                smallPointColor = new Color(0,0.3f,0, transparency);
                lineColor = new Color(0,1,0, transparency);
                list = yCoordList;
            }
            else if (coord == _coords.z)
            {
                vector = Vector3.forward;
                pointColor = new Color(0,0,1, transparency);
                smallPointColor = new Color(0,0,0.3f, transparency);
                lineColor = new Color(0,0,1, transparency);
                list = zCoordList;
            }
            
            for (int i = -unitsNum; i < unitsNum; i++)
            {
                list.Add(new PointColor(vector * i, color: pointColor, lineColor: lineColor, thickness: pointThickness, lineThickness: lineThickness));
                for (int j = 1; j < 10; j++)
                {
                    list.Add(new PointColor(vector * (i + 0.1f * j), color: smallPointColor, lineColor: lineColor, thickness: smallPointThickness, lineThickness: lineThickness));
                }
            } 
            list.Add(new PointColor(vector * unitsNum, color: pointColor, lineColor: lineColor, thickness: pointThickness, lineThickness: lineThickness));
        }
    }
}
