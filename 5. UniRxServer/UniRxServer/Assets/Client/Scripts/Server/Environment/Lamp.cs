using UnityEngine;

public class Lamp : MonoBehaviour {

    [SerializeField] private GameObject _lampLight;
    [SerializeField] private GameObject _domeOff;
    [SerializeField] private GameObject _domeOn;

    private bool _lampStatus;

    public void ChangeLampStatus()
    {
        _lampStatus = !_lampStatus;
        _lampLight.SetActive(_lampStatus);
        _domeOff.SetActive(!_lampStatus);
        _domeOn.SetActive(_lampStatus);
    }

}
