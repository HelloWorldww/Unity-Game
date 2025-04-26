using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class CloudSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cloudPrefab;
        [SerializeField] private Transform spawnPos;


        void Start()
        {
            StartCoroutine(SpawnClouds());
        }

        private void SpawnCloud()
        {
            Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y + Random.Range(-0.5f, 0.5f), spawnPos.position.z);
            GameObject newCloud = Instantiate(cloudPrefab, newPos, Quaternion.identity);
            Cloud cloud = newCloud.GetComponent<Cloud>();
            cloud.SpawnPosition = spawnPos.position;
        }

        private IEnumerator SpawnClouds()
        {
            SpawnCloud();

            yield return new WaitForSeconds(3f);

            SpawnCloud();

            yield return new WaitForSeconds(3f);

            SpawnCloud();

            yield return new WaitForSeconds(3f);

            SpawnCloud();

            yield return new WaitForSeconds(3f);

            SpawnCloud();

            yield return new WaitForSeconds(3f);
        }
    }
}