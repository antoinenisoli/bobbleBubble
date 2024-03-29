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
        protected SpriteRenderer sprRenderer;

        public virtual void Awake()
        {
            body = GetComponent<PhysicBody>();
            boxCollider = GetComponent<CustomBoxCollider>();
            sprRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void EnterBoxCollision(CustomCollision2D col)
        {
            
        }

        public virtual void ExitBoxCollision(CustomCollision2D col)
        {

        }

        private void OnEnable()
        {
            if (EventManager.Instance)
                EventManager.Instance.onNewBodyCreated.Invoke(boxCollider);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
                EventManager.Instance.onBodyRemove.Invoke(boxCollider);
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }
    }
}
