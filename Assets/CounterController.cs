using System.IO;
using UnityEngine;

public class CounterController
{
    private readonly string _filePath = Path.Combine(Application.dataPath , CounterModel.FileName());
    
    private CounterModel _counterModel;
    private CounterModel CounterModel
    {
        get => _counterModel;
        set
        {
            //  nullを設定しないようにする
            if (value == null)
            {
                Debug.LogError("nullを設定しないでください");
                return;
            }
            
            //  バリデーションチェックをするならここかModel
            if (value.Count < 0)
            {
                Debug.LogError("カウントは0以下にはなりません");
                return;
            }
            
            //  モデルの更新
            _counterModel = value;
            
            //  ビューの更新
            _counterView.CountText.text = $"Count : {value.Count.ToString()}";
        }
    }
    
    private readonly CounterView _counterView;
    
    public CounterController(CounterModel model, CounterView view)
    {
        _counterView = view;
        CounterModel = model;
        
        //  UIイベントの紐づけ
        view.CountUpBtn.onClick.AddListener(() =>
        {
            CounterModel = new CounterModel(CounterModel.Count + 1);
            SaveCount(CounterModel);
        });
        view.CountDownBtn.onClick.AddListener(() =>
        {
            CounterModel = new CounterModel(CounterModel.Count - 1);
            SaveCount(CounterModel);
        });
        
        //  カウントの読み込み
        LoadCount();
    }
    
    //  保存
    private void SaveCount(CounterModel model)
    {
        var wr = new StreamWriter(_filePath, false);
        wr.WriteLine(model.ToJson());
        wr.Close();
    }
    
    //  読み込み
    private void LoadCount()
    {
        // ファイルがないとき、ファイル作成
        if (!File.Exists(_filePath)) {
            return;
        }
        
        var rr = new StreamReader(_filePath);
        var json=rr.ReadLine();
        rr.Close();
        
        var tmpCounterModel = JsonUtility.FromJson<CounterModel>(json);

        if (tmpCounterModel == null) 
        {
            Debug.LogError("Jsonの読み込みに失敗しました");
            return;
        }
        
        CounterModel = tmpCounterModel;
    }
}
