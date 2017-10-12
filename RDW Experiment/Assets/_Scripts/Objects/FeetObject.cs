
using UnityEngine;

public class FeetObject : MonoBehaviour
{
    public delegate void Collided();

    public static event Collided OnCollision;

    void OnEnable()
    {
        if (Manager.SceneSwitcher.CurrentSceneIndex() == 2)
        {
            Level2.Reached += DestroyObject;
        }
        if (Manager.SceneSwitcher.CurrentSceneIndex() == 3)
        {
            Level3.reset += DestroyObject;
        }
        if (Manager.SceneSwitcher.CurrentSceneIndex() == 4)
        {
            Level4.reset += DestroyObject;
        }
        if (Manager.SceneSwitcher.CurrentSceneIndex() == 5)
        {
            Level5.Reached += DestroyObject;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (OnCollision != null && collision.tag == "MainCamera")
        {
            Debug.LogWarning("Trigger " + collision.name);
            OnCollision();
            Destroy(gameObject.GetComponent<Collider>());
            DestroyObject();
        }
        
    }

    public void DestroyObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
