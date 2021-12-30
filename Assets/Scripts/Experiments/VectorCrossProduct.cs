using System;
using UnityEngine;
using Experiments.Lib;

namespace Experiments.Experiments
{
    public class VectorCrossProduct : MonoBehaviour
    {
        private LinesDrawer ld;
        private readonly  UserPointsList u = new UserPointsList();
        private readonly  UserPointsList v = new UserPointsList();
        private readonly  UserPointsList crossProdLine = new UserPointsList();

        private Vector3 CrossProd => Vector3.Cross(u[1].position, v[1].position);  
       
        void Start()
        {
            ld = gameObject.AddComponent<LinesDrawer>();
            ld.shouldHideInHierarchy = true;
            ld.userListOfPointsLists.Add(u);
            u.Add(new PointColor(Vector3.zero));
            u.Add(new PointColor(new Vector3(0,1,0)));
            ld.userListOfPointsLists.Add(v);
            v.Add(new PointColor(Vector3.zero));
            v.Add(new PointColor(new Vector3(1,0,0)));
            ld.userListOfPointsLists.Add(crossProdLine);
            crossProdLine.Add(new PointColor(Vector3.zero, activeSelf: false));
            crossProdLine.Add(new PointColor(CrossProd, color: Color.yellow, lineColor: Color.yellow, activeSelf: true));
        }
        
        private void Update()
        {
            crossProdLine[1].position = CrossProd;
        }

        
        
    }
}
