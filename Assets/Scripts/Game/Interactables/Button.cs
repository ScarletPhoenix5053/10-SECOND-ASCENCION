using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField]
    private Material _inactiveMaterial;
    [SerializeField]
    private UnityEvent OnActivation;

    private void Awake()
    {
        if (_inactiveMaterial == null) throw new MissingReferenceException("Please assign something to " + name + "'s Inactive Material");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().material = _inactiveMaterial;
        transform.position -= new Vector3(0, 0, -0.5f);
        OnActivation.Invoke();
    }
}
