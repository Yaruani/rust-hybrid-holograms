# HybridHolograms API

Call the hooks from another plugin using `PluginReference` and `Call()` on the server thread.
他プラグインからは、サーバーのメインスレッドで `PluginReference` と `Call()` を使います。

```csharp
[PluginReference]
private Plugin HybridHolograms;

// Inside an initialization hook / 初期化フック内
HybridHolograms?.Call(
    "CreateHologram",
    "myplugin_123",
    (Func<Vector3>)(() => position),
    new List<string> { "Hello" },
    30f,
    Color.cyan
);
```

See [ExamplePlugin.cs](ExamplePlugin.cs) for a complete plugin.
プラグイン全体の例は [ExamplePlugin.cs](ExamplePlugin.cs) を参照してください。

## CreateHologram

```csharp
void CreateHologram(string id, Func<Vector3> positionFunc, List<string> lines,
    float distance = 0f, Color? color = null)
```

Creates a hologram or replaces the existing entry with the same ID.
同じIDがある場合は、そのホログラムを置き換えます。

| Parameter / 引数 | Behavior / 動作 |
|---|---|
| `id` | Use a unique ID across plugins / プラグインをまたいで一意なID |
| `positionFunc` | Supplies the base position when a player is evaluated for drawing / プレイヤーごとの描画判定時に基準位置を返す関数 |
| `lines` | The text lines to display / 表示するテキスト行 |
| `distance` | Maximum draw distance in meters. A value of `0` or less uses `Tick.MaxDistance` / 最大表示距離（m）。`0` 以下では `Tick.MaxDistance` を使用 |
| `color` | Text color. `null` uses `Color.white` / テキスト色。`null` では `Color.white` を使用 |

The plugin adjusts the height from the base position and applies the distance and visibility checks before sending draw commands.
基準位置から高さを自動調整し、距離と可視性を判定してから描画コマンドを送ります。

## UpdateText

```csharp
void UpdateText(string id, List<string> lines)
```

Replaces the text for an existing ID. The next draw uses the new text. Unknown IDs are ignored.
既存IDのテキストを差し替え、次回の描画で反映します。未登録のIDは処理されません。

## RemoveHologram

```csharp
void RemoveHologram(string id)
```

Stops future draws for the ID. Already drawn text expires after its drawing lifetime.
指定IDの次回以降の描画を止めます。描画済みのテキストは描画コマンドの表示時間が過ぎると消えます。
