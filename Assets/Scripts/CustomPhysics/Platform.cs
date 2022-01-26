using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class Platform : MonoBehaviour
    {
        PlayerController player;
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
            player = FindObjectOfType<PlayerController>();
            box = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            boxCollider.enabled = collidable;
        }
    }
}