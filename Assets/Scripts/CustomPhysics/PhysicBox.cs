using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    [System.Serializable]
    public class PhysicBox
    {
        public Vector2 position;
        public Vector2 center = new Vector2();
        public float width, height;
        public Vector2 additionalSize;
        public Vector2 velocity;

        public Vector2 topRightCorner => position + new Vector2(width / 2, height / 2);
        public Vector2 topLeftCorner => position + new Vector2(-width / 2, height / 2);
        public Vector2 bottomLeftCorner => position - new Vector2(width / 2, height / 2);
        public Vector2 bottomRightCorner => position - new Vector2(-width / 2, height / 2);

        public float minX => position.x - (width / 2);
        public float minY => position.y - (height / 2);
        public float maxX => position.x + (width / 2);
        public float maxY => position.y + (height / 2);
    };
}
