using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifespan = 0.3f;

    void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
