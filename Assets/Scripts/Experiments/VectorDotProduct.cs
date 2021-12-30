using System;
using UnityEngine;
using Experiments.Lib;

// https://flexbooks.ck12.org/cbook/ck-12-college-precalculus/section/9.6/primary/lesson/scalar-and-vector-projections-c-precalc/
namespace Experiments.Experiments
{
    public class VectorDotProduct : MonoBehaviour
    {
        private LinesDrawer ld;
        private readonly  UserPointsList u = new UserPointsList();
        private readonly  UserPointsList v = new UserPointsList();
        private readonly  UserPointsList projVonULine = new UserPointsList();
        private readonly  UserPointsList projUonVLine = new UserPointsList();

        private float UMagnitude => u[1].position.magnitude; 
        private float VMagnitude => v[1].position.magnitude;
        private Vector3 UNormalized => u[1].position / UMagnitude;
        private Vector3 VNormalized => v[1].position / VMagnitude;
        private float DotProdUV => Vector3.Dot(u[1].position, v[1].position);
        private Vector3 ProjVonU => (DotProdUV / UMagnitude) * UNormalized;
        private Vector3 ProjUonV => (DotProdUV / VMagnitude) * VNormalized;
        void Start()
        {
            ld = gameObject.AddComponent<LinesDrawer>();
            ld.shouldHideInHierarchy = true;
            ld.userListOfPointsLists.Add(u);
            u.Add(new PointColor(new Vector3(0,0,0)));
            u.Add(new PointColor(new Vector3(0,1,0)));
            ld.userListOfPointsLists.Add(v);
            v.Add(new PointColor(new Vector3(0,0,0)));
            v.Add(new PointColor(new Vector3(1,0,0)));
            ld.userListOfPointsLists.Add(projVonULine);
            projVonULine.Add(new PointColor(u[0].position, activeSelf: false));
            projVonULine.Add(new PointColor(ProjVonU, color: Color.yellow, lineColor: Color.yellow, activeSelf: true));
            ld.userListOfPointsLists.Add(projUonVLine);
            projUonVLine.Add(new PointColor(v[0].position, activeSelf: false));
            projUonVLine.Add(new PointColor(ProjUonV, color: Color.yellow, lineColor: Color.yellow, activeSelf: true));
        }
        
        private void Update()
        {
            Debug.Log("dot: " + DotProdUV);
            projVonULine[1].position = ProjVonU;
            projUonVLine[1].position = ProjUonV;
        }

        
        
    }
}
