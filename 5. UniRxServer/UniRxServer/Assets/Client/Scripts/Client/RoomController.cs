using UnityEngine;
using UniRx;
using UniRx.Triggers;

using UnityEngine.UI;

[RequireComponent(typeof(Client))]
public class RoomController : MonoBehaviour
{
    [SerializeField] private Button _lightSwitch;
    [SerializeField] private Button _bombActivate;

    private Client _client;

    #region MonoBehaviour
    void Start()
    {
        _client = GetComponent<Client>();

        _lightSwitch.OnPointerClickAsObservable().Subscribe(_=> _client.SendMessageToServer("light"));
        _bombActivate.OnPointerDownAsObservable().Subscribe(_=> _client.SendMessageToServer("explode"));
    }
    #endregion
}
