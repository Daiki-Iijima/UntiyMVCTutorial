using System;
using UnityEngine;

[Serializable]
public class CounterModel
{
    public static string FileName() => "counter.json";
    
    [SerializeField]
    private int count;
    public int Count => count;

    public CounterModel(int count)
    {
        this.count = count;
    }
    
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
