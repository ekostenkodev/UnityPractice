using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Tooltip("Скорость вращения камеры")]
    [SerializeField] private float _turnSpeed = 1.0f;
    private Transform _transform;

    #region MonoBehaviour
    private void Awake() => _transform = GetComponent<Transform>();
    void FixedUpdate () => _transform.Rotate(0, Time.fixedDeltaTime * _turnSpeed, 0);
    #endregion
}
