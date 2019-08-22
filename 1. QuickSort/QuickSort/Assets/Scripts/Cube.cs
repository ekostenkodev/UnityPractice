using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private TextMesh _valueMesh;

    private int _value;
    public int Value
    {
        set
        {
            _value = value;
            _valueMesh.text = _value.ToString();
        }

        get => _value;
    }

    #region MonoBehaviour

    void Awake()
    {
        if (_valueMesh == null) _valueMesh = GetComponentInChildren<TextMesh>();
    }

    #endregion

   
}
