using UnityEngine;

public class EndpointObject : MonoBehaviour
{
    public delegate void Collided();

    public static event Collided OnCollision;

    void OnTriggerEnter(Collider collision)
    {
        if (OnCollision != null)
        {
            //Debug.LogWarning("Trigger " + collision.name);
            Destroy(gameObject.GetComponent<Collider>());
            OnCollision();
        }

    }
    public void DestroyObject()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}