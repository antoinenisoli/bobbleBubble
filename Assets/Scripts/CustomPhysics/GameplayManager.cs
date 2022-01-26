using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomPhysics2D
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager instance;
        public List<CustomBoxCollider> colliders;

        [Header("Tilemap generation")]
        [SerializeField] Tilemap tilemap;
        [SerializeField] GameObject blockPrefab;

        public int Score;

        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);

            if (tilemap && blockPrefab)
                CreateBlockTilemap();

            colliders = FindObjectsOfType<CustomBoxCollider>().ToList();
        }

        private void Start()
        {
            EventManager.Instance.onNewBodyCreated.AddListener(AddCollider);
            EventManager.Instance.onBodyRemove.AddListener(RemoveCollider);
        }

        public void AddCollider(CustomBoxCollider collider)
        {
            if (!colliders.Contains(collider))
                colliders.Add(collider);
        }

        public void RemoveCollider(CustomBoxCollider collider)
        {
            if (colliders.Contains(collider))
                colliders.Remove(collider);
        }

        private void CreateBlockTilemap()
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                    Instantiate(blockPrefab, place, Quaternion.identity);
            }
        }
    }
}