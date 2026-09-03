using UnityEngine;

public class Enemy_HeadPart : MonoBehaviour
{
    private Enemy_Head parents;

    private void Awake()
    {
        parents = GetComponentInParent<Enemy_Head>();
    }

    private void OnTriggerEnter(Collider other)
    {
        parents.OnChildTriggerEnter(other, gameObject.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        parents.OnChildTriggerOut(other, gameObject.tag);
    } 
}
