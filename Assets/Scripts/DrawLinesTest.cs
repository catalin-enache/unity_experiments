using System.Collections.Generic;
using System.Linq;
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

            List<WithStringSet> withTagsList = FindObjectsOfType<WithStringSet>().Where(withTags => withTags.Set.Set.Contains("One")).ToList();
            // FindObjectsOfType<WithDictStringString>()[0].Dict.Dict.Add("bbb", "ccc");
            // FindObjectsOfType<WithDictStringString>()[0].Dict.Dict.Remove("qq");
            // FindObjectsOfType<WithStringSet>()[0].Set.Set.Add("new entry");
            // FindObjectsOfType<WithStringSet>()[0].Set.Set.Remove("Two");
            Debug.Log(string.Join(" | ", withTagsList.Select(withTags => string.Join(", ", withTags.Set.Set)))); 
        }

        void Update()
        {
            // PointColor tmp = ls.userListOfPointsLists[0][0];
            // ls.userListOfPointsLists[0][0].position = new Vector3(tmp.position.x + Time.deltaTime * 1, tmp.position.y);
        }

        void OnChange(Move3D point)
        {
            // Debug.Log(point.name);
        }
    }
}
