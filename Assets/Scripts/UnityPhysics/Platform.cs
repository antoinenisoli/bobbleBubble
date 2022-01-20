using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class Platform : MonoBehaviour
    {
        CustomPhysics_PlayerController player;
        BoxCollider2D box;
        [SerializeField] CustomBoxCollider boxCollider;
        [SerializeField] bool collidable = true;

        private void OnDrawGizmos()
        {
            if (boxCollider != null)
            {
                boxCollider.Show();
            }
        }

        private void Awake()
        {
            player = FindObjectOfType<CustomPhysics_PlayerController>();
            box = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            EventManager.Instance.onPlayerJump.AddListener(ColliderDisable);
            EventManager.Instance.onPlayerLanding.AddListener(ColliderEnable);
        }

        void ColliderEnable()
        {
            collidable = true;
        }

        void ColliderDisable()
        {
            if (player.transform.position.y < transform.position.y)
                collidable = false;
        }

        private void Update()
        {
            //boxCollider.enabled = collidable;
        }
    }
}