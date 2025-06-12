using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ConfigurarColision : MonoBehaviour
{
    void Reset()
    {
        // Configura el Rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        // Configura el TilemapCollider para que use Composite
        TilemapCollider2D tilemapCol = GetComponent<TilemapCollider2D>();
        tilemapCol.usedByComposite = true;

        // Configura el CompositeCollider
        CompositeCollider2D composite = GetComponent<CompositeCollider2D>();
        composite.geometryType = CompositeCollider2D.GeometryType.Polygons;
        composite.generationType = CompositeCollider2D.GenerationType.Synchronous;
    }
}