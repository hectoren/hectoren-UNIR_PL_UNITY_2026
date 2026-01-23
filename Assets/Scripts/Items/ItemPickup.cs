using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private string collectorTag = "PlayerHitBox";
    [SerializeField] private bool destroyOnPickup = true;

    private IItemEffect effect;

    private void Awake()
    {
        // El efecto está en el mismo GameObject (ítem autocontenido)
        effect = GetComponent<IItemEffect>();

        // Seguridad: asegurar trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(collectorTag)) return;

        // Aplica efecto si existe
        effect?.Apply(other.gameObject);

        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
