using UnityEngine;

[RequireComponent(typeof(Lamp))]
public class LampDestruct : MonoBehaviour, IDestructible
{
    [SerializeField] private GameObject _destroyedLampPrefab;
    private Transform _selfTransform;

    #region MonoBehaviour
    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }
    #endregion

    public void DestroyObject()
    {
        var destroyedLamp = Instantiate(_destroyedLampPrefab, _selfTransform.position, _selfTransform.rotation);
        Destroy(gameObject);
    }
}
