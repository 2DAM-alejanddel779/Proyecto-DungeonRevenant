using UnityEngine;

public class EspadaHit : MonoBehaviour
{
    // Da�o de la espada
    public int da�o = 7;

    // Para objeto de las cajas
    public Sprite spriteArma;
    public int aumentoDa�o = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar que el objetivo es el slime
        ControllerEnemigoScript slime = other.GetComponent<ControllerEnemigoScript>();
        if (slime != null)
        {
            slime.TomarDa�o(da�o);
            Debug.Log("�Espada golpe� al slime!");
        }

        if (other.CompareTag("Caja"))
        {
            Debug.Log("�Golpeaste una caja!");

            // Crear nuevo GameObject vac�o
            GameObject nuevaArma = new GameObject("ArmaSprite");

            // A�adirle un SpriteRenderer
            SpriteRenderer sr = nuevaArma.AddComponent<SpriteRenderer>();
            sr.sprite = spriteArma;
            sr.sortingOrder = 5;

            // Colocarlo en la posici�n de la caja
            nuevaArma.transform.position = other.transform.position;

            // A�adirle un CircleCollider2D como trigger
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
            da�o += aumentoDa�o;
            Debug.Log("�Has recogido la espada! Da�o del hitbox aumentado en " + aumentoDa�o);

            Destroy(other.gameObject);
        }
    }
}