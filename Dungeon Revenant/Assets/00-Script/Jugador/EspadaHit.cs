using UnityEngine;

public class EspadaHit : MonoBehaviour
{
    // Daño de la espada
    public int daño = 7;

    // Para objeto de las cajas
    public Sprite spriteArma;
    public int aumentoDaño = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar que el objetivo es el slime
        ControllerEnemigoScript slime = other.GetComponent<ControllerEnemigoScript>();
        if (slime != null)
        {
            slime.TomarDaño(daño);
            Debug.Log("¡Espada golpeó al slime!");
        }

        if (other.CompareTag("Caja"))
        {
            Debug.Log("¡Golpeaste una caja!");

            // Crear nuevo GameObject vacío
            GameObject nuevaArma = new GameObject("ArmaSprite");

            // Añadirle un SpriteRenderer
            SpriteRenderer sr = nuevaArma.AddComponent<SpriteRenderer>();
            sr.sprite = spriteArma;
            sr.sortingOrder = 5;

            // Colocarlo en la posición de la caja
            nuevaArma.transform.position = other.transform.position;

            // Añadirle un CircleCollider2D como trigger
            CircleCollider2D col = nuevaArma.AddComponent<CircleCollider2D>();
            col.isTrigger = true;

            // Destruir la caja
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Detectar si estamos encima del sprite del arma
        if (other.gameObject.name == "ArmaSprite")
        {
            daño += aumentoDaño;
            Debug.Log("¡Has recogido la espada! Daño del hitbox aumentado en " + aumentoDaño);

            Destroy(other.gameObject);
        }
    }
}