using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    //Checa se um gameobject tem a interface ICollectable
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectable collectable))
        {
            //Chama o método collect se tiver
            collectable.Collect();
        }
    }
}
