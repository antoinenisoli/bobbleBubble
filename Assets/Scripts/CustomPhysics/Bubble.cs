using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class Bubble : PhysicalEntity
    {
        [SerializeField] Vector2 direction;
        [SerializeField] float currentSpeed;
        [SerializeField] GameObject[] deathFX;

        [Header("Transforms into bubble")]
        [SerializeField] float bubbleDelay = 0.15f;
        public bool projectile = true;
        [SerializeField] float bubbleSpeed = 1f;

        [SerializeField] float maxContainingDuration = 30f;
        float containingTimer;
        Enemy containedEnemy;
        public bool contains => containedEnemy != null;

        public override void Awake()
        {
            base.Awake();
            Shoot(direction, currentSpeed);
        }

        private void Start()
        {
            StartCoroutine(GrowBubble(bubbleDelay));
        }

        public override void EnterBoxCollision(CustomCollision2D col)
        {
            base.EnterBoxCollision(col);
            if (!CustomPhysics.CompareLayer(interactWith, col.collider.gameObject.layer))
                return;

            //print(col.collider);
            Enemy enemy = col.collider.GetComponent<Enemy>();
            if (enemy)
            {
                containedEnemy = enemy;
                enemy.gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(GrowBubble(0f));
            }
            else
            {
                if (col.normal.y > 0)
                    direction = Vector2.left;
                else if (col.normal.y < 0)
                    direction = Vector2.right;
                else if (col.normal.x > 0)
                    direction = Vector2.up;
                else if (col.normal.x < 0)
                    direction = Vector2.down;
            }

            body.velocity = direction * currentSpeed;
        }

        private void OnDestroy()
        {
            if (contains)
            {
                Destroy(containedEnemy.gameObject);
                for (int i = 0; i < deathFX.Length; i++)
                {
                    GameObject fx = Instantiate(deathFX[i], transform.position, Quaternion.identity);
                }
            }
        }

        public IEnumerator GrowBubble(float delay)
        {
            yield return new WaitForSeconds(delay);
            projectile = false;
            currentSpeed = bubbleSpeed;
            body.velocity = direction * currentSpeed;
        }

        public void Shoot(Vector2 dir, float force)
        {
            currentSpeed = force;
            direction = dir;
            body.velocity = direction * force;
        }

        public override void Update()
        {
            base.Update();

            if (!projectile)
            {
                containingTimer += Time.deltaTime;
                if (containingTimer > maxContainingDuration)
                {
                    containedEnemy.gameObject.SetActive(true);
                    containedEnemy.gameObject.transform.position = transform.position;
                    Destroy(gameObject);
                }
            }
        }
    }
}