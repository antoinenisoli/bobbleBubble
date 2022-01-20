using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public static class CustomPhysics
    {
        public static bool CheckAABB(PhysicBox b1, PhysicBox b2, out Vector2 normal)
        {
            normal = new Vector2();

            if (b1.velocity.x > 0)
                normal.x = 1;
            else if (b1.velocity.x < 0)
                normal.x = -1;

            if (b1.velocity.y > 0)
                normal.y = 1;
            else if (b1.velocity.y < 0)
                normal.y = -1;

            bool check =
                b1.minX <= b2.maxX && b1.maxX >= b2.minX
                && b1.minY <= b2.maxY && b1.maxY >= b2.minY
                ;

            return check;
        }
    }
}
