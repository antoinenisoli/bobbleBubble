using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics2D
{
    public class Bubble : PhysicalEntity
    {
        [SerializeField] Vector2 direction;
        [SerializeField] float currentSpeed;

        [Header("Transforms into bubble")]
        [SerializeField] float bubbleDelay = 0.15f;
        public bool projectile = true;
        [SerializeField] float bubbleSpeed = 1f;
        Enemy containedEnemy;

        float swipeDirectionTimer;
        float swipeDirectionCooldown;
        [SerializeField] Vector2 randomTimerRange;

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

        float RandomCooldown()
        {
            return Random.Range(randomTimerRange.x, randomTimerRange.y + 1);
        }

        public void AbsorbEnemy(Enemy enemy)
        {
            SoundManager.Instance.PlayAudio("Bubble Bobble SFX (2)");
            containedEnemy = enemy;
            enemy.gameObject.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(GrowBubble(0f));
        }

        Vector2 RandomDirection()
        {
            int random = Random.Range(0, 5);
            if (direction.x != 0)
            {
                if (random > 0)
                    return Vector2.up;
                else
                    return Vector2.down;
            }
            else
            {
                if (random > 0)
                    return Vector2.left;
                else
                    return Vector2.right;
            }
        }

        public override void EnterBoxCollision(CustomCollision2D col)
        {
            base.EnterBoxCollision(col);
            if (!CustomPhysics.CompareLayer(interactWith, col.collider.gameObject.layer))
                return;

            Enemy enemy = col.collider.GetComponent<Enemy>();
            if (enemy)
            {
                if (projectile)
                    AbsorbEnemy(enemy);
                else
                    return;
            }
        }

        public void DeathFeedback()
        {
            SoundManager.Instance.PlayAudio("Bubble Bobble SFX (17)");
            GameplayManager.instance.AddScore(containedEnemy.scoreValue);
            VFXManager.Instance.PlayVFX("EnemyDeath", transform.position);
            GameObject fx = VFXManager.Instance.PlayVFX("ScoreText", transform.position);
            FloatingText text = fx.GetComponent<FloatingText>();
            if (text)
                text.Create(containedEnemy.scoreValue);
        }

        private void OnDestroy()
        {
            if (contains)
            {
                DeathFeedback();
                Destroy(containedEnemy.gameObject);
            }
        }

        public IEnumerator GrowBubble(float delay)
        {
            yield return new WaitForSeconds(delay);
            projectile = false;
            currentSpeed = bubbleSpeed;
            direction = Vector2.up;
            SetVelocity();
            swipeDirectionCooldown = RandomCooldown();
        }

        void SetVelocity()
        {
            body.velocity = direction * currentSpeed * Time.fixedDeltaTime;
        }

        public void Shoot(Vector2 dir, float force)
        {
            currentSpeed = force;
            direction = dir;
            SetVelocity();
        }

        public void SwipeDirection()
        {
            swipeDirectionTimer = 0;
            swipeDirectionCooldown = RandomCooldown();
            direction = RandomDirection();
            SetVelocity();
        }

        void KeepInScreen()
        {
            Vector2 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            if (transform.position.x > stageDimensions.x - boxCollider.box.width)
            {
                transform.position = new Vector2(stageDimensions.x - boxCollider.box.width, transform.position.y);
                SwipeDirection();
            }
            if (transform.position.x < -stageDimensions.x)
            {
                transform.position = new Vector2(-stageDimensions.x + boxCollider.box.width, transform.position.y);
                SwipeDirection();
            }

            if (transform.position.y > stageDimensions.y - boxCollider.box.width)
            {
                transform.position = new Vector2(stageDimensions.y - boxCollider.box.width, transform.position.y);
                SwipeDirection();
            }
            if (transform.position.y < -stageDimensions.y / 2 + boxCollider.box.width)
            {
                transform.position = new Vector2(-stageDimensions.y / 2 + boxCollider.box.width, transform.position.y);
                SwipeDirection();
            }
        }

        public override void Update()
        {
            base.Update();
            if (!projectile)
            {
                swipeDirectionTimer += Time.deltaTime;
                if (swipeDirectionTimer > swipeDirectionCooldown)
                    SwipeDirection();
            }

            //KeepInScreen();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            KeepInScreen();
        }
    }
}