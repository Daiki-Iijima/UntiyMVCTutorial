# このプロジェクトについて

このプロジェクトは、Unityを使ってアプリやゲームを作る際によく使われるアーキテクチャであるMVCパターンのサンプルのプロジェクトになります。

実装している動き自体はシンプルで、ボタンを押すとカウンターの値が増減します。

![demo](https://github.com/Daiki-Iijima/UntiyMVCTutorial/blob/main/ApplicationDemo.gif)

## Q&A

- CounterControllerクラス内のCounterModelのSetメソッド部分でのvalueという変数はどこから出てきた？ 

    - valueは、利用者側のクラスで代入された値がプロパティの設定メソッド（setter）内で格納される予約語。 
https://learn.microsoft.com/ja-jp/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties

### CounterControllerクラス内のCounterControllerメソッドで、CounterModel = model;としているのに、_counterView = view;と命名している。CounterViewとせずアンダーバーを使う理由。 

- privateかつプロパティじゃないもののとき、「_小文字始まり」で書く。viewは一度だけ設定したら付け替えることは基本ないため。 

### CounterModel = new CounterModel(CounterModel.Count + 1);で、ボタン押すたびにインスタンスを作り直す理由 

- 別の実装方法としてCounterModelを使い回す方法もあるが、非同期処理で値の代入がバッティングする危険性がある。最近の開発傾向として、メモリに余裕があるのでnewで気にせず毎回インスタンス化し直すことが多い。 

### public Button CountDownBtn => countDownBtn;のアロー演算子は何をしているのか。 

- 以下の書き方と同じ。setを書かなくてよいので簡潔に書ける。 

```c#
public Button CountDownBtn { 
    get { return countDownBtn; }  
}
```

### CounterModelクラス内の記述について

[SerializeField] private int count; 

public int Count => count; 

というcountの使い分けの意図。 

回答：JsonUtilityは少し癖があり、private なプロパティをシリアライズ(JSON形式の文字列へ変換)してくれないため、シリアライズ用のpublic変数Countを別途用意している。 

 

6. return JsonUtility.ToJson(this);に値というよりCounterModelクラスが入るというのがしっくりこない。 

回答：オブジェクトが入っていると考える。thisはインスタンス自身。よく見られる書き方。 
