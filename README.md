# このプロジェクトについて

このプロジェクトは、Unityを使ってアプリやゲームを作る際によく使われるアーキテクチャであるMVCパターンのサンプルのプロジェクトになります。

実装している動き自体はシンプルで、ボタンを押すとカウンターの値が増減します。

![demo](https://github.com/Daiki-Iijima/UntiyMVCTutorial/blob/main/ApplicationDemo.gif)

## Q&A

実際の質問に対する回答を載せています。
コードを実際読んで動かしてわからない箇所などの参考にしてください。

---

### Q: `CounterController`クラス内のCounterModelのSetメソッド部分でのvalueという変数はどこから出てきた？

```c#
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
```

上記コードが対象のコードで、`value`は、利用者側のクラスで代入された値がプロパティの設定メソッド（setter）内で格納される予約語になります。

動作としては、 `CounterModel`プロパティに値を代入する際に、`value`という変数に代入された値が入ってきます。

以下の公式ドキュメントを参考にしてください。

[Microsoft C# Auto Implemented Properties](https://learn.microsoft.com/ja-jp/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties)

---

### Q: CounterControllerクラス内のイニシャライザで、`CounterModel = model;`としているのに、`_counterView = view;`と命名している。`CounterView`とせずアンダーバーを使う理由は何か？

```c#
//  イニシャライザ
public CounterController(CounterModel model, CounterView view)
{
    _counterView = view;
    CounterModel = model;

    //  ....
}
```

CounterControllerの上の方に定義してある変数を本体を見ると、`CounterModel`はプロパティであり、`CounterView`はフィールドであるため、プロパティとフィールドを区別するためにアンダースコアを使っています。

```c#
private CounterModel _counterModel;
private CounterModel CounterModel
{
    get => _counterModel;
    set
    {
        //  ...
    }
}

private readonly CounterView _counterView;
```

- プロパティ : アッパーキャメルケース
- フィールド : アンダースコア + ローワーキャメルケース

---

### Q: CounterModel = new CounterModel(CounterModel.Count + 1);で、ボタン押すたびにインスタンスを作り直す理由

```c#
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
```

インスタンスを作り直すことで、オブジェクトが不変（イミュータブル）になることを目指しています。

イミュータブルな設計には以下のメリットがあります

- 予測可能性の向上: オブジェクトが変更されることがないため、状態遷移の追跡が容易になります。
- スレッドセーフ: 他のスレッドとデータ競合を気にする必要がなくなり、マルチスレッド環境でも安全に扱えます。
- バグの減少: 意図しない状態変更のリスクがなくなり、デバッグが容易になります。

また、インスタンスを作り直すことで、前後の状態が独立した形で保持されます。

このため、以下のような機能を簡単に実装できます

- アンドゥ・リドゥ機能: 古いインスタンスを保持しておけば、簡単に元の状態に戻すことができます。
- 状態の履歴管理: 各操作ごとの状態をリストとして保存しておけば、後から状態の変化を追跡できます。

---

### Q: `public Button CountDownBtn => countDownBtn;`のアロー演算子は何をしているのか

以下の書き方と同じ。setを書かなくてよいので簡潔に書ける

```c#
public Button CountDownBtn { 
    get { return countDownBtn; }  
}
```

---

### Q: CounterModelクラス内にCountメンバ変数が2つある意味について

```c#
[SerializeField] private int count; 

public int Count => count; 
```

UnityのJsonUtilityは少し癖があり、privateなプロパティをシリアライズ(JSON形式の文字列へ変換)してくれないため、シリアライズ用のpublic変数Countを別途用意している

もっとスマートに買いたい場合は、Newtonsoft.Jsonを使うとよい

---

### Q: return JsonUtility.ToJson(this);に値というよりCounterModelのインスタンスが入っているのはなぜか

オブジェクトが入っていると考える。thisはインスタンス自身。よく見られる書き方
