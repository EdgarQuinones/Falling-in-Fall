
using UnityEngine;

public class LeafTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<LeafController>()?.TriggerLeaf();
        }
    }
}
