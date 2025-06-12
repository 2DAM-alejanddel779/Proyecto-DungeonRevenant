using UnityEngine;
using System.Collections;

public class EnemigoShootAI : MonoBehaviour
{
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private float tiempoDisparo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Disparo());
    }

    IEnumerator Disparo()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoDisparo);
            Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        }
    }
}
