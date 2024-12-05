using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button countUpBtn;
    [SerializeField] private Button countDownBtn;
    
    public TextMeshProUGUI CountText => countText;
    public Button CountUpBtn => countUpBtn;
    
    public Button CountDownBtn => countDownBtn;
}
