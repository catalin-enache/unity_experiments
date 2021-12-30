using System;
using UnityEngine;
using Experiments.Lib;

namespace Experiments.Experiments
{
    public class VectorSubtraction : MonoBehaviour
    {
        private LinesDrawer ld;
        private readonly  UserPointsList v1 = new UserPointsList();
        private readonly  UserPointsList v2 = new UserPointsList();
        private readonly  UserPointsList v2Mirror = new UserPointsList();
        private readonly  UserPointsList diffLine = new UserPointsList();
        void Start()
        {
            ld = gameObject.AddComponent<LinesDrawer>();
            ld.shouldHideInHierarchy = true;
            ld.userListOfPointsLists.Add(v1);
            v1.Add(new PointColor(new Vector3(0,0,0)));
            v1.Add(new PointColor(new Vector3(0,1,0)));
            ld.userListOfPointsLists.Add(v2);
            v2.Add(new PointColor(new Vector3(0,0,0)));
            v2.Add(new PointColor(new Vector3(1,0,0)));
            ld.userListOfPointsLists.Add(v2Mirror);
            v2Mirror.Add(new PointColor(v1[1].position, activeSelf: false));
            v2Mirror.Add(new PointColor(v1[1].position - v2[1].position, color: Color.white * 0.5f, lineColor: Color.white * 0.3f, activeSelf: false));
            ld.userListOfPointsLists.Add(diffLine);
            diffLine.Add(new PointColor(v2[1].position, activeSelf: false));
            diffLine.Add(new PointColor(v2[1].position + (v1[1].position - v2[1].position), color: Color.white * 0.5f, lineColor: Color.white * 0.3f, activeSelf: false));
        }


        private void Update()
        {
            v2Mirror[0].position = v1[1].position;
            v2Mirror[1].position = v1[1].position - v2[1].position;
            diffLine[0].position = v2[1].position;
            diffLine[1].position = v2[1].position + (v1[1].position - v2[1].position);
        }
    }
}
