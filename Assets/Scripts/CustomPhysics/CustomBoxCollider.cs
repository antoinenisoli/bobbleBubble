using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class CustomBoxCollider : MonoBehaviour
    {
        [SerializeField] [Range(0,0.1f)] float gizmoSize = 0.1f;
        [SerializeField] Color gizmoColor = Color.white;
        [SerializeField] SpriteRenderer spr;
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
            if (spr)
            {
                if (spr.drawMode != SpriteDrawMode.Tiled)
                {
                    box.width = Mathf.Abs((transform.lossyScale.x * 0.25f) * box.additionalSize.x);
                    box.height = Mathf.Abs((transform.lossyScale.y * 0.25f) * box.additionalSize.y);
                }
                else
                {
                    box.width = spr.size.x;
                    box.height = spr.size.y;
                }
            }
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

        private void FixedUpdate()
        {
            ApplyTransform();
        }
    }
}