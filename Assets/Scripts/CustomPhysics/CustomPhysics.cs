using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public static class CustomPhysics
    {
        public static bool CheckAABB(PhysicBox b1, PhysicBox b2)
        {
            bool check =
                b1.minX <= b2.maxX && b1.maxX >= b2.minX
                && b1.minY <= b2.maxY && b1.maxY >= b2.minY
                ;

            return check;
        }

        public static bool CheckAABB(PhysicBox b1, PhysicBox b2, out Vector2 normal)
        {
            normal = GetNormal(b1, b2);
            return CheckAABB(b1, b2);
        }

        public static Vector2 GetNormal(PhysicBox b1, PhysicBox b2)
        {
            Vector2 normal = new Vector2();

            if (b1.minX <= b2.minX && b2.minX <= b1.maxX && b1.maxX < b2.maxX)
                normal.x = -1;
            if (b2.minX <= b1.minX && b1.minX <= b2.maxX && b2.maxX < b1.maxX)
                normal.x = 1;

            if (b1.minY <= b2.minY && b2.minY <= b1.maxY && b1.maxY <= b2.maxY)
                normal.y = -1;
            if (b2.minY <= b1.minY && b1.maxY >= b2.maxY && b2.maxY <= b1.maxY)
                normal.y = 1;

            if (normal.x != 0 && normal.y != 0)
                normal.y = 0;

            return normal;
        }
    }
}
