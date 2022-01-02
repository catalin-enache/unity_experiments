using Experiments.Lib;
using UnityEngine;

namespace Experiments.Experiments
{
    public class AABBAndSphereIntersection : MonoBehaviour
    {

        private AABoundingBox boundingBox = null;
        public GameObject sphere = null;
        void Start()
        {
            boundingBox = gameObject.AddComponent<AABoundingBox>();
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            sphere.transform.localPosition = new Vector3(2, 2, 0);
            sphere.transform.localScale = Vector3.one * 0.2f;
        }

        void Update()
        {
            boundingBox.IntersectsWithSphere(sphere);
        }
    }
}
