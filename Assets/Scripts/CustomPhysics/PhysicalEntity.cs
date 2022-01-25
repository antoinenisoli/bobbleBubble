using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomPhysics2D
{
    public class PhysicalEntity : MonoBehaviour
    {
        [Header(nameof(PhysicalEntity))]
        [SerializeField] protected CustomBoxCollider boxCollider;
        public PhysicBody body;
        public LayerMask interactWith;

        public virtual void Awake()
        {
            body = GetComponent<PhysicBody>();
            boxCollider = GetComponent<CustomBoxCollider>();
        }

        public virtual void EnterBoxCollision(CustomCollision2D col)
        {
            
        }

        public virtual void ExitBoxCollision(CustomCollision2D col)
        {

        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }
    }
}
