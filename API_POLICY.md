# HybridHolograms API Stability Policy

## Public API / 公開API

The current source exposes three hooks for other plugins.
現行ソースでは、他プラグイン向けに次の3つのフックを公開しています。

- `CreateHologram`
- `UpdateText`
- `RemoveHologram`

Use the signatures in [API.md](API.md) for the installed version. Earlier development used different version numbers, as noted in the [changelog](CHANGELOG.md).
導入するバージョンの引数は [API.md](API.md) を参照してください。旧開発版のバージョン番号については [変更履歴](CHANGELOG.md) を参照してください。

## Compatibility / 互換性

Within the same major version, keep the public hooks, signatures and behavior compatible. If a breaking change is necessary, increment the major version, document the deprecation and keep the old API available for at least one major version.
同じメジャーバージョン内では公開フックの名前・引数・動作の互換性を維持します。破壊的変更が必要な場合はメジャーバージョンを更新し、非推奨の案内を記載して、最低1メジャーバージョンは旧APIを維持します。

## Usage guidelines / 利用時の注意

- Access the hooks through `PluginReference` and `Call()`. Keep the plugin reference nullable so integrations can handle an unloaded dependency.
- Give each hologram a unique ID such as `myplugin_objectId`.
- Keep the position provider lightweight. Update text when it changes and remove your holograms when your plugin unloads.

- `PluginReference` と `Call()` でフックを呼び出します。参照が取得できない場合に備えて null を確認してください。
- `myplugin_objectId` のように、他プラグインと重複しないIDを使います。
- 位置取得関数の処理は軽く保ち、テキストは内容が変わったときに更新します。呼び出し側のプラグインをアンロードするときは、そのプラグインが作ったホログラムを削除してください。

The calling plugin manages the position, text and lifecycle. HybridHolograms handles drawing and visibility checks.
位置、テキスト、作成・削除は呼び出し側が管理し、描画と可視性の判定はHybridHologramsが行います。
