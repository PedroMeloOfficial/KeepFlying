using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float _maxTime = 1.5f;
    [SerializeField] private float _heightRange = 0.45f;
    [SerializeField] private GameObject _obstacle;

    [Header("SFX")]
    [SerializeField] private AudioSource _audioSource;  // assign in Inspector
    [SerializeField] private AudioClip _whooshSfx;     // your whoosh clip

    private float _timer;

    private void Start()
    {
        SpawnObstacle();
    }

    private void Update()
    {
        if(_timer > _maxTime)
        {
            SpawnObstacle();
            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-_heightRange, _heightRange));
        GameObject obstacle = Instantiate(_obstacle, spawnPos, Quaternion.identity);

        Destroy(obstacle, 10f);

        //schedule the whoosh SFX after 4 seconds
        StartCoroutine(PlayWhooshAfterDelay(1f));
    }

    private IEnumerator PlayWhooshAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_audioSource != null && _whooshSfx != null)
            _audioSource.PlayOneShot(_whooshSfx);
    }
}
