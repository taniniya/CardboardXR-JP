# Webシアター (Theater)

VR の中に浮かぶ大画面（3.2m × 1.8m）で YouTube などの Web ページを見るデモです。
アプリライブラリでは「Webシアター」として追加されます（デモ番号 5）。

## 操作

- デモを開くと、正面に画面が置かれ、YouTube が開きます
- 画面の操作は視線＋ピンチ：クリックはピンチ、スクロールはピンチしながら視線を動かす
- 画面の下にある「コントロール」ボタンでパネルが開きます
  - ショートカット：YouTube / 登録チャンネル / YouTube Music
  - 戻る・進む・更新・ホーム
- ロビーへ戻るのは設定パネルから（他のデモと同じ）

## ブラウザを有効にする手順（最初に1回だけ）

このデモのブラウザ表示には、有料プラグイン **Vuplex「3D WebView for Android」** が必要です。
プラグインが無い間はダミー画面で起動し、パネルにもその旨が出ます（ビルドはそのまま通ります）。

1. Unity Asset Store で「3D WebView for Android」（Vuplex、$179.99）を購入し、プロジェクトにインポートする
   - iOS でも使う場合は「3D WebView for Android and iOS」版
2. Unity の **Project Settings > Player > Other Settings > Scripting Define Symbols** に
   `VUPLEX_PRESENT` を追加する
3. ビルドし直す

これで `VuplexScreen` が有効になり、ダミー画面が本物のブラウザに置き換わります。

## 困ったとき

- **画面のクリックがページに届かない場合**：シーンの `Screen Anchor` にある
  `TheaterScreen` の **Force Relay Clicks** を ON にしてビルドし直してください。
  クリックがリレー経由（`IWebView.Click`）で送られるようになります（この場合ドラッグでの
  スクロールは使えません）。
- **文字がぼやける場合**：`VuplexScreen` の `k_Resolution`（現在 800 px/m）を上げると
  シャープになりますが、処理は重くなります。

## 制限

- 360°動画・VR動画は平面のまま再生されます（普通の動画専用です）
- 検索などの文字入力は視線操作では大変なので、ショートカット中心の設計にしています
- ログインが必要なページ（登録チャンネルなど）は、ブラウザ内で一度ログインすれば
  次回以降も保持されます

## 中身

- `TheaterDemo`：画面の配置とコントロールパネル
- `TheaterScreen`：画面（ベゼル＋当たり判定）とブラウザの差し替え口
- `IWebScreen` / `VuplexScreen` / `PlaceholderScreen`：ブラウザ本体とダミー
- `TheaterClickRelay`：画面クリックの座標を渡すリレー
- `TheaterLinks` / `TheaterMath`：ショートカットと座標変換（テストあり）
