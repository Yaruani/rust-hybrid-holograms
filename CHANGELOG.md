# Changelog

All notable changes to this project will be documented in this file.
このプロジェクトの主な変更点をここに記録します。

## Current source / 現行ソース（1.0.4）

- Redraws are scheduled per player.
- Drawing stops beyond the configured or API-supplied maximum distance.
- プレイヤーごとに再描画時刻を管理します。
- 設定またはAPIで指定した最大表示距離を超えると描画を停止します。

The plugin version was reset from `2.0.0` to `1.0.0` during development. The entries below use the original version numbers. See [API.md](API.md) for the current hooks.
開発中にプラグインのバージョン番号を `2.0.0` から `1.0.0` に振り直しました。以下は当時のバージョン番号で記載しています。現在のフックは [API.md](API.md) を参照してください。

## Earlier development / 旧開発履歴

[1.6.0] - 2026-01-02
Added

Public API fully documented with XML comments

Developer notes added for safe API usage

Changed

Author name changed to Yaruani

Notes

No behavioral changes from v1.5

Safe update (no breaking changes)

追加

Public API を XML コメントで完全ドキュメント化

API 利用時の注意点を開発者向けに明記

変更

作成者名を Yaruani に変更

補足

v1.5 から挙動変更なし

破壊的変更なし（安全な更新）

[1.5.0] - 2026-01-02
Added

Full JSON configuration support

Tick rates, distances and render parameters are configurable

Notes

No recompilation required after tuning

追加

JSON Config 完全対応

Tick 間隔・距離・描画パラメータを設定可能に

補足

再コンパイル不要

[1.4.0] - 2026-01-02
Added

NetworkVisibility integration

Changed

Invisible holograms now consume zero processing

Visibility checks occur before raycasting

追加

NetworkVisibility 判定を統合

変更

見えないホログラムは処理ゼロ

Raycast 前に可視判定を実施

[1.3.0] - 2026-01-02
Added

Adaptive tick rate system

Changed

Tick interval scales automatically based on player proximity

Performance

Significant CPU reduction on idle servers

追加

Tick 間隔自動可変システムを実装

変更

プレイヤー距離に応じて Tick を自動調整

パフォーマンス

アイドル時の CPU 使用率を大幅削減

[1.2.0] - 2026-01-02
Added

Hybrid rendering system (TMP + ddraw)

Changed

Near players see high-quality 3D text

Far players use lightweight ddraw rendering

追加

ハイブリッド描画（TMP + ddraw）を導入

変更

近距離は高品質 3D テキスト

遠距離は軽量 ddraw 描画

[1.1.0] - 2026-01-02
Added

TextMeshPro billboard behavior

Changed

Improved readability from all viewing angles

追加

TMP のビルボード化（常にプレイヤーを向く）

変更

全方向からの視認性を改善

[1.0.0] - 2026-01-02
Added

Initial release

追加

初期リリース

Legend

Added / 追加: New features

Changed / 変更: Behavior or internal changes

Notes / 補足: Non-functional information
