using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private float minMoveSpeed = 2f;
        [SerializeField] private float maxMoveSpeed = 2f;

        public Vector3 SpawnPosition { get; set; }

        float randomMoveSpeed;

        private void Start()
        {
            randomMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        }

        private void Update()
        {
            transform.Translate(Vector3.left * Time.deltaTime * randomMoveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            transform.position = new Vector3(SpawnPosition.x, transform.position.y + Random.Range(-0.5f, 0.5f));
        }
    }
}