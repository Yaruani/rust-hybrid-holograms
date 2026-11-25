# HybridHolograms

Server-side text holograms for Rust plugins on Carbon or Oxide/uMod.
Rustの他プラグインからテキストホログラムを表示するためのCarbon／Oxide（uMod）向けプラグインです。

## Features / 特徴

- Sends `ddraw.text` commands to connected players within range and with a clear line of sight.
- Schedules redraws per player based on distance and adjusts the text height automatically.
- Sends drawing commands without checking a HybridHolograms permission.

- 範囲内かつ表示位置が見える接続中プレイヤーに `ddraw.text` を送ります。
- プレイヤーごとに距離に応じた再描画間隔を適用し、テキストの高さを自動調整します。
- 描画コマンドの送信にプラグイン独自の権限は不要です。

## Installation / 導入方法

Copy [HybridHolograms.cs](HybridHolograms.cs) into `carbon/plugins` or `oxide/plugins`.
The framework generates `HybridHolograms.json` in `carbon/configs` or `oxide/config`.

[HybridHolograms.cs](HybridHolograms.cs) をCarbonでは `carbon/plugins`、Oxideでは `oxide/plugins` に配置します。
初回ロード時に、対応する `carbon/configs` または `oxide/config` に `HybridHolograms.json` が生成されます。

## Configuration / 設定

| Key / 設定キー | Default / 既定値 | Behavior / 動作 |
|---|---:|---|
| `Tick.NearDistance`, `Tick.FarDistance` | `8`, `25` m | Distance thresholds for redraws / 再描画間隔を切り替える距離 |
| `Tick.NearTick`, `Tick.FarTick`, `Tick.MaxTick` | `0.1`, `0.5`, `1.2` s | Redraw intervals at near, intermediate and greater distances / 近距離・中距離・それ以遠の再描画間隔 |
| `Tick.MaxDistance` | `60` m | Default maximum draw distance for newly created holograms / 新規ホログラム作成時の既定の最大表示距離 |
| `Debug.HeightPadding`, `Debug.FallbackHeight` | `0.25`, `1.8` m | Height above nearby geometry and height offset when no geometry is found / 周囲の形状に対する高さ余白と、形状が見つからない場合の高さ補正 |
| `Debug.Enabled` | `false` | Enables the debug overrides / デバッグ時の上書き設定を有効化 |
| `Debug.ForceDistance` | `0` m | A positive value overrides the draw distance in debug mode / デバッグ時に正の値で最大表示距離を上書き |
| `Debug.IgnoreVisibilityCheck`, `Debug.PrefixDebugText` | `false`, `true` | Skip visibility checks or add a debug line in debug mode / デバッグ時の可視性判定の省略とデバッグ行の追加 |

## Public API / 公開API

Use `PluginReference` and `Call()` from another plugin.
他プラグインからは `PluginReference` と `Call()` を使用してください。

- [API signatures and behavior / APIの引数と動作](API.md)
- [API usage policy / API利用方針](API_POLICY.md)
- [Example plugin / サンプルプラグイン](ExamplePlugin.cs)
- [Changelog / 変更履歴](CHANGELOG.md)

## Author / 作者

Yaruani

## License / ライセンス

[MIT License](LICENSE)
