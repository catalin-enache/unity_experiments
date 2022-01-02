using UnityEngine;

namespace Experiments.Lib
{
    
    public class AABoundingBox : MonoBehaviour
    {
        private GameObject minPosHandler = null;
        private GameObject maxPosHandler = null;
        private GameObject centerPosHandler = null;
        private GameObject AABB = null;
        private GameObject closestPoint = null;
        private Material handlersMaterial = null;
        private readonly Vector3 handlersSize = Vector3.one * 0.2f;

        public bool UseMinMax = true;
        public Vector3 Scale = Vector3.one;
        
        private readonly Color AABBIntersectColor = new Color(1, 0, 0, 0.3f);
        private readonly Color AABBNormalColor = new Color(1, 1, 1, 0.3f);
        private Color _AABBColor = new Color(1, 1, 1, 0.3f);

        public Color AABBColor
        {
            get => _AABBColor;
            set
            {
                _AABBColor = value;
                AABB.GetComponent<Renderer>().material.color = value;
            }
        }

        public void SetIsIntersectingColor(bool isIntersecting)
        {
            AABBColor = isIntersecting ? AABBIntersectColor : AABBNormalColor;
        }
        
        void Start()
        {
            handlersMaterial = Resources.Load<Material>("Materials/mat_WhiteTransparent");
           
            minPosHandler = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            minPosHandler.name = "Min";
            maxPosHandler = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            maxPosHandler.name = "Max";
            centerPosHandler = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            centerPosHandler.name = "Center";
            AABB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            AABB.name = "AABB";
            AABB.GetComponent<Renderer>().material.color = AABBColor;
            closestPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            closestPoint.name = "Closest Point";
            
            minPosHandler.GetComponent<Renderer>().material = handlersMaterial;
            maxPosHandler.GetComponent<Renderer>().material = handlersMaterial;
            centerPosHandler.GetComponent<Renderer>().material = handlersMaterial;
            AABB.GetComponent<Renderer>().material = handlersMaterial;
            closestPoint.GetComponent<Renderer>().material = handlersMaterial;
            closestPoint.GetComponent<Renderer>().material.color = new Color(1,1,0,0.5f);
            
            minPosHandler.transform.localScale = handlersSize;
            maxPosHandler.transform.localScale = handlersSize;
            centerPosHandler.transform.localScale = handlersSize;
            closestPoint.transform.localScale = handlersSize / 2;
            
            
            MinPos = new Vector3(-1, -1, -1);
            MaxPos = new Vector3(1, 1, 1);
            CenterPos = new Vector3(0, 0, 0);
            
            UpdatePositions();
        }

        void Update()
        {
            UpdatePositions();
        }

        public Vector3 MinPos
        {
            get => minPosHandler.transform.localPosition;
            set => minPosHandler.transform.localPosition = value;
        }
        
        public Vector3 MaxPos
        {
            get => maxPosHandler.transform.localPosition;
            set => maxPosHandler.transform.localPosition = value;
        }

        public Vector3 CenterPos
        {
            get => centerPosHandler.transform.localPosition;
            set => centerPosHandler.transform.localPosition = value;
        }

        void UpdateCenterAndScaleFromMinMax()
        {
            CenterPos = (MinPos + MaxPos) / 2;
            Scale.x = Mathf.Abs(MaxPos.x - MinPos.x);
            Scale.y = Mathf.Abs(MaxPos.y - MinPos.y);
            Scale.z = Mathf.Abs(MaxPos.z - MinPos.z);
        }

        void UpdateMinMaxFromCenterAndScale()
        {
            MinPos = CenterPos - new Vector3(Scale.x / 2, Scale.y / 2, Scale.z / 2);
            MaxPos = CenterPos + new Vector3(Scale.x / 2, Scale.y / 2, Scale.z / 2);
        }
        
        void UpdatePositions()
        {
            if (UseMinMax)
            {
                UpdateCenterAndScaleFromMinMax();
            }
            else
            {
                UpdateMinMaxFromCenterAndScale();
            }

            AABB.transform.localPosition = CenterPos;
            AABB.transform.localScale = Scale;
        }

        public Vector3 ClosestPointToOtherPoint(Vector3 point)
        {
            float x = Mathf.Max(MinPos.x, Mathf.Min(point.x, MaxPos.x));
            float y = Mathf.Max(MinPos.y, Mathf.Min(point.y, MaxPos.y));
            float z = Mathf.Max(MinPos.z, Mathf.Min(point.z, MaxPos.z));
            Vector3 closestPointPosition = new Vector3(x, y, z);
            closestPoint.transform.localPosition = closestPointPosition;
            return closestPointPosition;
        }

        public bool IntersectsWithSphere(GameObject sphere)
        {
            // https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection#sphere_vs._aabb
            SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
            float sphereRadius = sphereCollider.radius * sphereCollider.transform.localScale.x;
            float sphereRadiusSquared = Mathf.Pow(sphereRadius, 2);
            Vector3 spherePosition = sphere.transform.localPosition;
            Vector3 closestPointPosition = ClosestPointToOtherPoint(spherePosition);
            float distanceSquared = (spherePosition - closestPointPosition).sqrMagnitude;
            bool isIntersecting =  distanceSquared < sphereRadiusSquared;
            return isIntersecting;
        }

        public bool IntersectWithBoundingBox(AABoundingBox boundingBox)
        {
            // https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection#aabb_vs._aabb
            bool isIntersecting = (MinPos.x <= boundingBox.MaxPos.x && MaxPos.x >= boundingBox.MinPos.x)
                                   && (MinPos.y <= boundingBox.MaxPos.y && MaxPos.y >= boundingBox.MinPos.y)
                                   && (MinPos.z <= boundingBox.MaxPos.z && MaxPos.z >= boundingBox.MinPos.z);
            return isIntersecting;
        }
    }
}
