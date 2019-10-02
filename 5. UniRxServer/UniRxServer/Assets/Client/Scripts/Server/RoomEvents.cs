using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Server))]
public class RoomEvents : MonoBehaviour
{
    [SerializeField] private Explosion _bomb;
    [SerializeField] private Lamp _lamp;

    private bool _isExplode;
    private Server _server;

    #region MonoBehaviour
    private void Awake()
    {
        _server = GetComponent<Server>();
        _isExplode = false;
    }

    void Start()
    {
        _server.Data.ObserveOnMainThread().Subscribe(data => DataReceive(data));
    }
    #endregion

    void DataReceive(string data)
    {
        data = Regex.Replace(data, @"\t|\n|\r", string.Empty);

        switch (data) // todo: заменить на json
        {
            case "light":
                ActivateLamp();
                break;

            case "explode":
                ActivateBomb();
                break;

            default:
                break;
        }
    }

    private void ActivateBomb()
    {
        _isExplode = true;
        _bomb.Explode();
    }

    private void ActivateLamp()
    {
        if(!_isExplode)
            _lamp.ChangeLampStatus();
    }

}
