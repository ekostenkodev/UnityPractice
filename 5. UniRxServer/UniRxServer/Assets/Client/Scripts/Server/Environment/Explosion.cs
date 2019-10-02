using System;
using System.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private float _radius;
    [SerializeField] private float _force;
    private Transform _selfTransform;

    public float Radius => _radius;
    public float Force => _force;

    #region MonoBehaviour
    private void Awake()
    {
        _selfTransform = GetComponent<Transform>();
    }
    #endregion

    public async void Explode()
    {
        
        var explosion = Instantiate(_explosionEffectPrefab, _selfTransform.position, _selfTransform.rotation);

        Collider[] collidersToDestroy = Physics.OverlapSphere(_selfTransform.position, _radius);

        foreach (var nearbyObject in collidersToDestroy)
        {
            IDestructible nearDestructible = nearbyObject.GetComponent<IDestructible>();
            
            if (nearDestructible!=null)
            {
                nearDestructible.DestroyObject();
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(_selfTransform.position, _radius);

        foreach (var nearbyObject in collidersToMove)
        {
            Rigidbody nearRigidbody = nearbyObject.GetComponent<Rigidbody>();
            if (nearRigidbody)
            {
                nearRigidbody.AddExplosionForce(_force, _selfTransform.position, _radius);
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(3)); // время работы частиц

        if(explosion)
        {
            Destroy(explosion.gameObject);
        }
    }
}
