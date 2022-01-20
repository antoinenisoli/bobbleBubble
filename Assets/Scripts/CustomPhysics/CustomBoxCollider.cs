using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class CustomBoxCollider : MonoBehaviour
    {
        [SerializeField] [Range(0,0.1f)] float gizmoSize = 0.1f;
        [SerializeField] Color gizmoColor = Color.white;
        public bool isColliding;
        [SerializeField] bool applyMatrix = true;
        public PhysicBox box;

        private void OnDrawGizmos()
        {
            if (box != null)
                Show();
        }

        public void SetBox(PhysicBox box)
        {
            this.box = box;
        }

        public void Show(PhysicBox box)
        {
            ApplyTransform();

            if (applyMatrix)
            {
                Matrix4x4 oldmMatrix = Gizmos.matrix;
                Gizmos.matrix = transform.localToWorldMatrix;
                DisplayBox(box, (Vector2)transform.position - box.center);
                Gizmos.matrix = oldmMatrix;
            }
            else
                DisplayBox(box);
        }

        public void Show()
        {
            ApplyTransform();

            if (applyMatrix)
            {
                Matrix4x4 oldmMatrix = Gizmos.matrix;
                Gizmos.matrix = transform.localToWorldMatrix;
                DisplayBox(box, (Vector2)transform.position - box.center);
                Gizmos.matrix = oldmMatrix;
            }
            else
                DisplayBox(box);
        }

        void ApplyTransform()
        {
            box.position = transform.position + (Vector3)box.center;
        }

        void DisplayBox(PhysicBox box, Vector2 positionOffset = default)
        {
            Color r = gizmoColor;
            Gizmos.color = r;
            Gizmos.DrawCube(transform.position - (Vector3)positionOffset, new Vector2(box.width, box.height));
            Gizmos.DrawSphere(box.topRightCorner - positionOffset, gizmoSize);
            Gizmos.DrawSphere(box.topLeftCorner - positionOffset, gizmoSize);
            Gizmos.DrawSphere(box.bottomLeftCorner - positionOffset, gizmoSize);
            Gizmos.DrawSphere(box.bottomRightCorner - positionOffset, gizmoSize);

            Gizmos.DrawSphere(new Vector2(box.position.x, box.maxY) - positionOffset, gizmoSize);
            Gizmos.DrawSphere(new Vector2(box.position.x, box.minY) - positionOffset, gizmoSize);
            Gizmos.DrawSphere(box.bottomRightCorner - positionOffset, gizmoSize);
        }

        private void Update()
        {
            ApplyTransform();
        }
    }
}