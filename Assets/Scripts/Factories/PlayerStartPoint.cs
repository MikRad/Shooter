using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    private void Start()
    {
        EventBus.Get.RaiseEvent(this, new PlayerStartPointCreatedEvent(transform.position));
        
        Destroy(gameObject);
    }
}
