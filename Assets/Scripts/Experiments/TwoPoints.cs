using UnityEngine;
using Experiments.Lib;

namespace Experiments.Experiments
{
    public class TwoPoints : MonoBehaviour
    {
        private LinesDrawer ld;
        private readonly  UserPointsList pointsList = new UserPointsList();
        void Start()
        {
            ld = gameObject.AddComponent<LinesDrawer>();
            ld.userListOfPointsLists.Add(pointsList);
            pointsList.Add(new PointColor(new Vector3(0,0,0)));
            pointsList.Add(new PointColor(new Vector3(1,1,0)));
        }

        
        void Update()
        {
        
        }
    }
}
