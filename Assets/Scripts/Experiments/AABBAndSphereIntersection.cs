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
        void Start()
        {
            boundingBox1 = gameObject.AddComponent<AABoundingBox>();
            boundingBox2 = gameObject.AddComponent<AABoundingBox>();

            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            sphere.transform.localPosition = new Vector3(2, 2, 0);
            sphere.transform.localScale = Vector3.one * 0.2f;
            
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
            bool bb1AndSphere = boundingBox1.IntersectsWithSphere(sphere);
            bool bb2AndSphere = boundingBox2.IntersectsWithSphere(sphere);
            bool bb1AndBb2 = boundingBox1.IntersectWithBoundingBox(boundingBox2);
            bool bb2AndBb1 = boundingBox2.IntersectWithBoundingBox(boundingBox1);
            boundingBox1.SetIsIntersectingColor(bb1AndSphere || bb1AndBb2);
            boundingBox2.SetIsIntersectingColor(bb2AndSphere || bb2AndBb1);
        }
    }
}
