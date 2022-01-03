using System.Collections;
using Experiments.Lib;
using UnityEngine;

namespace Experiments.Experiments
{
    public class AABBAndSphereIntersection : MonoBehaviour
    {

        private AABoundingBox boundingBox1 = null;
        private AABoundingBox boundingBox2 = null;
        public GameObject sphere = null;
        public GameObject point = null;
        void Start()
        {
            boundingBox1 = gameObject.AddComponent<AABoundingBox>();
            boundingBox2 = gameObject.AddComponent<AABoundingBox>();

            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.name = "Point";
            
            sphere.transform.localPosition = new Vector3(2, 2, 0);
            sphere.transform.localScale = Vector3.one * 0.2f;
            
            point.transform.localPosition = new Vector3(3, 3, 0);
            point.transform.localScale = Vector3.one * 0.05f;
            
            StartCoroutine(SetUpScene());
        }
        
        IEnumerator SetUpScene()
        {
            yield return new WaitForSeconds(0.1f);
            boundingBox1.UseMinMax = false;
            boundingBox1.CenterPos = Vector3.right * -2f;
            boundingBox2.UseMinMax = false;
            boundingBox2.CenterPos = Vector3.right * 2f;
        }

        void Update()
        {
            bool bb1AndSphere = boundingBox1.IsIntersectingWithSphere(sphere);
            bool bb2AndSphere = boundingBox2.IsIntersectingWithSphere(sphere);
            bool bb1AndPoint = boundingBox1.IsIntersectingWithPoint(point.transform.position);
            bool bb2AndPoint = boundingBox2.IsIntersectingWithPoint(point.transform.position);
            bool bb1AndBb2 = boundingBox1.IsIntersectingWithBoundingBox(boundingBox2);
            bool bb2AndBb1 = boundingBox2.IsIntersectingWithBoundingBox(boundingBox1);
            boundingBox1.SetIsIntersectingColor(bb1AndSphere || bb1AndBb2 || bb1AndPoint);
            boundingBox2.SetIsIntersectingColor(bb2AndSphere || bb2AndBb1 || bb2AndPoint);
        }
    }
}
