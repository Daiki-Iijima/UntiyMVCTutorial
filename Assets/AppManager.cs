using UnityEngine;

public class AppManager : MonoBehaviour
{
    [SerializeField] private CounterView counterView;
    
    private CounterController _counterController;

    private void Awake()
    {
        // ここでUIを変更する可能性があるので、counterControllerを初期化しては駄目
    }

    private void Start()
    {
        _counterController = new CounterController(new CounterModel(0), counterView);
    }

}
