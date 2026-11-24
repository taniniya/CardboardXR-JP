# CardboardXR

Google Cardboard 型のビューアで使う Unity 製スマホアプリです。左右分割のステレオ表示とレンズ歪み補正に加えて、従来の Cardboard にはなかった機能を備えています。ARCore または ARKit による 6DoF のヘッドトラッキング、カメラのパススルー表示、検出した面に固定するオブジェクト、そしてスマホの背面カメラによるハンドトラッキング（ピンチ、つかむ、指し示すレイ）です。

これは [brenpoly/CardboardXR](https://github.com/brenpoly/CardboardXR) の日本語版です。アプリ内の表示とこの README を日本語にし、日本語フォントを同梱しています。

> **Custom by [taniniya](https://github.com/taniniya)**
>
> このリポジトリ [taniniya/CardboardXR-JP](https://github.com/taniniya/CardboardXR-JP) は、[taniniya](https://github.com/taniniya) が [brenpoly/CardboardXR](https://github.com/brenpoly/CardboardXR) を日本語化し、日本語フォントの同梱や GitHub Actions での APK ビルドなどを加えた**改良版（カスタム版）**です。本家 brenpoly 氏の公式配布ではありません。本家の最新版とリリースは [brenpoly/CardboardXR](https://github.com/brenpoly/CardboardXR) にあります。

**Pixel 9 Pro**（Android、ARCore）と **iPhone 15 Pro**（iOS、ARKit、LiDAR 付き）で動作を確認しています。

## 動画とビューア

プロジェクトを紹介する**動画**が YouTube にあります:

[![YouTube の CardboardXR](https://img.youtube.com/vi/XtNmiTohUcs/maxresdefault.jpg)](https://youtu.be/XtNmiTohUcs)

**ビューアを 3D プリントする**: 現代のスマートフォン向けに作ったモバイル VR ビューアの 3D プリント用ファイルは MakerWorld にあります: [Mobile VR viewer for modern smartphones](https://makerworld.com/en/models/3511377-mobile-vr-viewer-for-modern-smartphones)。
ビューアのスマホ用ドアを自分のスマホに合わせるには、[スマホ用ドア生成ツール](https://brenpoly.github.io/CardboardXR/door-generator/)にスマホの寸法を入力してください。3MF と STL のファイルをブラウザの中で作ります。

## インタラクティブなページ

プロジェクトには 3 つのインタラクティブなページがあります。画像をクリックすると開きます。

**Cardboard から VR グラスへ**: 2013 年以降のすべてのヘッドセットを発売時の価格または形態で並べ、スマホ VR がどこで消えていったかを示します。

[![ヘッドセットの年表: 2013 年から 2027 年までのすべての VR ヘッドセットを発売時の価格で並べたもの](docs/images/timeline.png)](https://brenpoly.github.io/CardboardXR/)

**iPhone ビルドの内部**: ハードウェア、Apple と Unity のフレームワーク、アプリがどう組み合わさっているかを層ごとに示します。

[![iPhone ビルドの構成: センサーとカメラから ARKit と AI ランタイムを通り、頭・手・部屋のトラッキング、描画、発熱の制御まで](docs/images/architecture.png)](https://brenpoly.github.io/CardboardXR/iphone-architecture.html)

**スマホ用ドア生成ツール**: スマホの寸法を入力すると、それに合う 3D プリントビューア用のドアを 3MF または STL でダウンロードできます。

[![スマホ用ドア生成ツール: スマホの寸法を入れるフォームの隣に、オレンジの TPU 製コーナーロケーター付きドアの 3D プレビュー](docs/images/door-generator.png)](https://brenpoly.github.io/CardboardXR/door-generator/)

ページは `docs/` にあり、GitHub Pages で公開しています。

## インストール

Android は [Releases](https://github.com/brenpoly/CardboardXR/releases) から APK をダウンロードし、ARCore 対応のスマホ（Android 10 以降、64bit、Vulkan 対応）で開いてください。ブラウザやファイルマネージャーからのアプリのインストールを許可するよう求められます。iOS 用のダウンロードはまだありません。下記の手順でソースからビルドしてください。

## 必要なもの

- Unity **6000.6.8f1** と、Android・iOS のビルドモジュール。スクリプトは Unity Hub がインストールする場所を前提にしています。別の場所にある場合は `UNITY=/path/to/Unity.app/Contents/MacOS/Unity` を設定してください
- Android の場合: `PATH` の通った `adb`（`~/Library/Android/sdk/platform-tools` にあります）
- iOS の場合: Xcode 一式と、Apple ID でのサインイン（無料の個人チームで足ります）

スクリプトを実行する前に、Editor でプロジェクトを閉じてください。スクリプトはバッチモードでビルドするため、プロジェクトのロックが必要です。

## ビルドして実行

どのスクリプトもアプリをビルドし、インストールし、起動して、セッションログの冒頭を表示します。

```sh
zsh Tools/build_and_run.sh                                   # Android
zsh Tools/ios_build_and_run.sh                               # iOS
zsh Tools/ios_build_and_run.sh --xcode                       # iOS、前回の Unity 書き出しを再利用
zsh Tools/ios_launch.sh                                      # iOS、ビルドせずに再起動
```

ビルドはリリースビルドです。開発用ビルド（デバッグでき、Unity Profiler がローカルネットワーク経由で接続できる）にするには、`CARDBOARDXR_DEVELOPMENT=1` を設定してください。アプリ ID は `com.brenpoly.cardboardxr` です。iOS のバンドル ID は最初に署名した Apple のチームに属するので、自分の iPhone 用にビルドするときは自分の ID を選んでください。どのスクリプトでも指定方法は同じです:

```sh
CARDBOARDXR_BUNDLE_ID=com.example.cardboardxr zsh Tools/ios_build_and_run.sh
```

Android のスクリプトは、先に EditMode テストも実行します。iOS では、最初のインストールのときにスマホの開発者モード（設定 > プライバシーとセキュリティ。最初に一度インストールを試みたあとで表示されます）と、開発元の信頼（設定 > 一般 > VPN とデバイス管理）が必要です。起動にはスマホのロック解除が要ります。ビルドには 5 分ほどかかるので、その間に自動ロックがかかったら、ロックを解除して `ios_launch.sh` を実行してください。

### リリース

`Tools/release_android.sh 1.2.3` は、公開リポジトリの新しいクローン（プレースホルダーの動画だけが入った状態）からリリース用 APK をビルドし、自分のリリース用キーストアで署名し、署名と中身を確認して、確認して公開するための下書きの GitHub リリースを作ります。キーストアとパスワードの置き場所は、スクリプト冒頭のコメントに書いてあります。

### GitHub Actions で APK をビルドする

`.github/workflows/android-apk.yml` は、バージョンタグ（`v0.0.1`）がプッシュされたとき、または手動（Actions > Android APK > Run workflow）で、既定では未署名の APK をビルドし、そのタグのリリースに追加します。スクリプトと同じビルド方法（`ProjectBootstrap.StereoBuilder.BuildAndroid`）を、[game-ci](https://game.ci) 経由の Unity `6000.6.8f1` で実行します。

Secrets（Settings > Secrets and variables > Actions）:

- `UNITY_LICENSE`: Unity の `.ulf` の中身。あわせて `UNITY_EMAIL` と `UNITY_PASSWORD`、または Plus/Pro シートの場合はこの 3 つの代わりに `UNITY_SERIAL` を設定します。https://game.ci/docs/github/activation
- 署名は任意: `ANDROID_KEYSTORE_BASE64`（base64 にした `.keystore`）、`ANDROID_KEYSTORE_PASS`、`ANDROID_KEY_ALIAS`（既定は `cardboardxr`）。設定しない場合、APK は Unity のデバッグキーで署名されます。インストールはできますが、別のキーで署名したビルドの上に更新することはできません。

日本語は M PLUS 1p で描画します。フォントは `Assets/_Project/UI/Resources/MPLUS1p-Regular.ttf` に同梱しています（SIL Open Font License 1.1。`THIRD_PARTY_NOTICES.md` を参照）。ビルドのたびに TMP Settings の動的フォールバックフォントに設定され（`ProjectBootstrap.JapaneseFontSetup`）、その手順を経ていないビルドでも、起動時に `CardboardXR.UI.JapaneseFont` が同じ設定を行います。

### 起動オプション

開発用の上書き設定です。どちらのプラットフォームでも、スクリプト自身の引数のあとに同じように指定します:

```sh
zsh Tools/build_and_run.sh --ei dof 3 --ei mono 1
zsh Tools/ios_launch.sh --ei dof 3 --ei mono 1
```

Android ではインテントの extra、iOS では環境変数（`CARDBOARDXR_<name>`）としてアプリに届きます。どちらも Mac から起動したときにだけ効き、スマホのホーム画面から起動したときには効きません。アプリは起動時に受け取ったオプションをログに出します（`[Stereo] Launch options`）。

| オプション | 効果 |
|---|---|
| `demo N` | ロビーではなくデモ N で始める（`Assets/_Project/Demos/DemoCatalog.asset` の 0 から始まる番号） |
| `pinch 0` | 3DoF でピンチによるクリックをオフにする。発熱を測るため（通常は常にオン） |
| `dof 3` | ロビーを 3DoF で始め、部屋のスキャンをしない（通常は 6DoF パススルーで始まる） |
| `passthrough 1` | カメラのパススルー表示で始める |
| `mono 1` | レンズ歪みのない 1 枚の全画面表示にする。ビューアなしで使う場合向け |
| `boundaryCm N` | 境界の警告をオンにする。VR では、N cm 以内に入った実在の面にグリッドを表示する |
| `arView 1` | デバッグ表示: AR カメラを全画面にし、検出したすべての面を描く（6DoF 固定） |
| `fps N` | 目標フレームレート（既定 120） |
| `renderScale N` | アイテクスチャの解像度を百分率で指定（既定 100） |
| `msaa N` | アンチエイリアスのサンプル数。1、2、4、8（既定 2） |
| `handHz N` | 手のランドマークモデルを 1 秒に最大 N 回動かす（既定 0: カメラ画像ごと） |
| `predictionMs N` | 頭の回転を何 ms 先まで予測するか（既定 30） |
| `tracker 1` | 頭の回転を、アプリ独自のセンサー融合ではなく Android の game rotation vector から取る |
| `nativeImu 0` | Android: センサーをネイティブプラグインではなく Input System 経由で読む |
| `imuPosition 0` | 6DoF の位置から加速度計を外す。比較用 |
| `handAxes 1..8` | ハンドトラッキング用に、カメラ画像の軸が画面上でどう向くかを上書きする |
| `pinchStudy 1` | ピンチの測定値をログに出す、指示付きの実行。ピンチ検出の調整用 |
| `uiScale N` | Material UI の大きさを Android XR の dp サイズの百分率で指定（既定 160。スマホ用ビューアの低い解像度向け） |
| `brightness N` | アプリが前面にある間の画面の明るさを百分率で指定（既定 90。0 なら変更しない） |
| `libraryRows N` | アプリライブラリの 1 ページあたりの行数。1〜4（既定 2）。デモが少ないときにページ送りを見るには `1` |

## アプリの使い方

- **画面右上の歯車ボタン**を押すと、小さなメニューが開きます。*ビューアを設定* はビューアの QR コードを読み取ります。*スマホ表示* / *ビューア表示* は、ビューア用の左右分割表示とスマホの全画面表示を切り替えます。*設定を開く* は設定パネルを開きます。
- **アプリライブラリ**は設定パネルの上にあり、ロビーと各デモのアイコンが並びます。アイコンに視線を置くと何のアプリかが表示され、ピンチかタップで開けます。どこからでも開けるので、デモからデモへ直接移れます。1 ページに 6 個表示され、それより多いときは左右の矢印でページを送ります。設定パネルの *アプリ* で表示と非表示を切り替えられます。ロビーはチェッカーボードの部屋で、ライブラリを表示した状態で開きます。アプリが起動すると、ロビーは 6DoF パススルーになり、まず部屋をスキャンします。床やテーブルに視線を向けて見回し、終わったと表示されるまで待ってください（*スキップ* も押せます）。こうするとデモは、トラッキングが動き面が見つかった状態で始まります。各デモは必要なもの（6DoF、パススルー、手のトラッキング）を自分でオンにし、デモを出ると元の状態に戻します。
- **入力はどこでも視線とピンチ**: 何かに視線を置き、カメラの前でピンチするとクリックになります。画面のタップでも同じです。カメラが手を捉えている間は、レティクルが水色になります。6DoF のデモではハンドトラッキングが担当し（物をつかむのも同じです）、3DoF ではピンチだけを見る軽い検出器が普通のカメラ映像から担当します（手が映っていない間は速度を落とします）。ピンチはすべて 1 つの順序（`PinchRouter`）を通ります。まず手のひらの設定ギア、次に視線の先のボタン、次にデモ自身の用途（ペイント、オーブを打つ）、次につかむ、最後に通常のクリックです。手がカメラに映っていないのにボタンに視線を置くと、レティクルの下に、手を上げるよう促すヒントが出ます。
- **360度動画** は 360 度の動画と VR180 動画を、2D でも 3D でも再生します。動画をピンチすると操作パネルが出ます。小さなテストクリップが 2 本同梱されています。ローカルビルドに自分の動画を入れるには、プロジェクトの隣に `360` フォルダを作って入れるか（`CARDBOARDXR_VIDEOS` でも指定できます）、ビルドスクリプトがそれを `Assets/StreamingAssets` にコピーします（テストクリップ以外は git の管理外です）。スマホのアプリの `Videos` フォルダに自分で追加することもできます。Android では `adb push video.mp4 /storage/emulated/0/Android/data/com.brenpoly.cardboardxr/files/Videos`、iPhone では Finder でスマホの「ファイル」タブを開き、CardboardXR > Videos。ファイル名のタグ（`_360`、`_180`、`_sbs`、`_tb`、`_mono`）で形式を指定します。Google Drive の動画は、そのフォルダの `videos.txt` に共有リンクを 1 行に 1 本ずつ並べてください。再生するたびに最初の 1 回だけダウンロードします。
- **空間ペイント** は Tilt Brush のように、手で空中に線を描きます。パススルーで始まるので、自分の部屋に描けます。ピンチで描き、左のツールパネルに視線を置いてピンチすると、色、ブラシ（インク、チューブ、グロー）、太さ（小・中・大）を選べます。ほかに戻す、全消去（2 回押し）、パススルーと VR の切り替えがあります。
- **オーブラッシュ** は片手で遊ぶゲームです。オーブが手前に飛んでくるので、リングに届くところで手で打ちます。遊びたい方向を向いて「スタート」を押してください。コース上のスコアの行にある「一時停止」からは、再開、やり直し、終了を選べます。ベストスコアは保存されます。
- **ポータル** は床に戸口を開け（見てピンチ）、その先に夜の世界が広がります。枠をくぐってケーブルから離れると、周囲がその世界になり、背後のドアからは自分の部屋が見えます。戻るときはもう一度くぐってください。パススルーの中で動くので、全体が平面的です。
- **設定パネル** はどのシーンにもあります。手のひらを自分に向けて手を上げると、親指と人差し指の間に歯車ボタンが現れます。ピンチすると目の前にパネルが開きます。*アプリ*（アプリライブラリ）、*正面に戻す*、*部屋スキャン*（6DoF の場合: もう一度スキャンする、またはスキャンを終える）、*6DoF* と *パススルー* のトグルがあります。体の向きに合わせてゆっくり追従し、下の *X* で閉じます。ボタンに視線を置いて画面をタップするか、ピンチしてください。タップを長押しすると正面に戻ります。
- **面の検出** は、起動時のロビーのスキャンで行い、新しい面が見つからなくなるまで続きます。あとで 6DoF を再び始めるとき（360度動画のあとなど）は、床が見つかるまでの短いスキャンを行い、手のモデルのために CPU を節約します。キューブ箱庭は、ロビーのスキャンで足りなかった場合に部屋全体をスキャンします。
- **オブジェクトの配置と移動**: 6DoF では、面に視線を置いてタップかピンチするとキューブが置かれ、その場所に固定されます。頭で狙ってピンチでクリックし、手（またはそのレイ）でキューブをつかみます。ピンチでつかめるキューブは水色になります。つかんでいる間は手と一緒に回り、ほかのキューブを押しのけます。指を開くと離すか投げます。自由なキューブには物理演算があり、落ちて、互いに跳ね返り、検出した面に着地します。キューブを持っている間にカメラが手を見失っても、落ちずにその場に残ります。

## ログ

起動するたびに、スマホにセッションログが書かれ、前回のログを置き換えます（前回分は `session-previous.log` として残ります）。スクリプトはログの冒頭を表示します。全体を読むには:

```sh
adb pull /storage/emulated/0/Android/data/com.brenpoly.cardboardxr/files/session.log   # Android
```

iOS では `ios_launch.sh` が `$TMPDIR/cardboardxr/ios-session.log` にコピーします。

行には分野ごとのタグが付きます。`[Pose]` 頭のトラッキング、`[AR]` AR セッション、`[Room]` 面と床、`[Anchor]` 置いたオブジェクト、`[Hands]` ハンドトラッキングとつかむ動作、`[Pinch]` ピンチ調査、`[Passthrough]`、`[Gaze]` クリック、`[Frames]` と `[Hitch]` フレームのタイミング、`[Stereo]` セットアップ、`[Tracking]` モーションセンサー。ほとんどの分野は 5 秒ごとに状態を 1 行記録します。警告とエラーは常に残ります。

## クリップの録画

片目に見えている映像を、レンズ歪みなしで撮るには、ビューアでアプリを使いながらスマホの画面を録画し、`Tools/record_eye.py` で片目を平らにします:

- **iPhone**: Mac で QuickTime Player > ファイル > 新規ムービー収録を選び、カメラとして iPhone を選びます（USB 接続）。スマホ側にファイルを書かないので、負担は小さい方法です。
- **Android**: USB 接続で `scrcpy --record clip.mp4`、またはスマホ標準の画面録画。

```sh
python3 Tools/record_eye.py clip.mov --log session.log          # その実行のログから画面とビューアを決める
python3 Tools/record_eye.py clip.mov --phone iphone --fov 70    # Cardboard v1。狭いほどくっきり
```

左目を切り出し（`--eye right` で右目）、アプリ自身のレンズ計算で歪みを戻して、1920x1080（`--size`）の H.264 クリップを入力の隣に書き出します。`--start` と `--duration` で範囲を切り取れます。パススルーでは両目に同じ映像が映るので、片目で全体がわかります。

## テスト

```sh
/Applications/Unity/Hub/Editor/6000.6.8f1/Unity.app/Contents/MacOS/Unity -batchmode \
    -projectPath . -runTests -testPlatform EditMode -testResults results.xml
```

EditMode テストは、実機を必要としない部分を対象にします。レンズ歪みとビューアの QR プロファイル、センサーと姿勢の融合、カメラの時計、手の形状、ピンチ検出、深度のサンプリングと手のひらのキャリブレーション、床の選択、パススルー、視線によるポインティングとアンカリングです。各デモのテストは、そのフォルダの `Tests` にあります。

## 仕組み

XR ローダーは同時に 1 つしか動かせず、6DoF、カメラ、面を提供するのは AR Foundation のもの（ARCore、ARKit）です。そのためこのアプリは Google の Cardboard プラグインを使わず、自前でステレオ表示を描画し、レンズ歪みは Cardboard SDK から移植しています。

コードは `Assets/_Project/Scripts` に、分野ごとに 1 つのアセンブリとして入っています:

- **Stereo** — ステレオのリグとレンズ歪み、頭のトラッキング（AR の姿勢で補正するジャイロの融合）、パススルー、面、アンカー、フレームのタイミング、セッションログ
- **Hands** — Unity Inference Engine 上の MediaPipe BlazeHand モデルによるハンドトラッキング。3D 空間に配置し、XR Hands と XR Interaction Toolkit に渡します。iOS では LiDAR の深度で手の大きさを学びます
- **Interaction** — 視線によるポインティングとハイライト
- **UI** — Material Design for XR のキット（Material 3 の基準ダーク配色、Roboto、Material Icons）を Android XR の dp で寸法指定したもので、各シーンの UI はこれで組み立てています
- **Demos** — ロビーとデモの間の移動と、各デモが Core に何を求めるか
- **Tracking** — Unity に依存しないセンサーの融合とフィルタ

**アプリは Core シーン 1 つに、ロビーとデモを上に読み込む構成です。** Core はデモをまたいで動き続ける必要のあるものを持ちます。リグ、頭のトラッキング、AR、手、視線、設定パネルです。ロビーと各デモは追加で読み込み、離れるときに読み込みを外すシーンで、`DemoManager` がその間を切り替え、デモのスクリプトは `CoreServices` を通じて Core に届きます。デモに必要なものはすべて `Assets/_Project/Demos/<Name>/` に入ります。シーン、アセット、スクリプト、テスト、そして `DemoCatalog.asset` の自分の項目（シーンと必要なもの）とスターターシーンを作る `DemoSetup` です。
[ADDING_A_DEMO.md](ADDING_A_DEMO.md) では、自分のビルド用にデモを追加する方法を、手作業でも、コーディングエージェントと `Tools/new-demo` のスキルでも説明しています。このリポジトリは新しいデモは受け付けません。

**Core は生成される**もので、手では編集しません。`Assets/Editor/ProjectBootstrap/StereoBuilder.cs` がビルドのたびに `Core.unity` とそのマテリアルを作るので、Editor で加えた変更は失われます。**ロビーとデモは手で編集します**: `DemoScenes.cs` がロビーとカタログを作り、各デモのセットアップがスターターシーンとカタログの項目を作ります。どちらも無いときにだけ作られ、上書きされることはありません。`ProjectSetup.Apply` が両プラットフォームのプレイヤー設定と XR 設定を行います。

現在の制限は [KNOWN_ISSUES.md](KNOWN_ISSUES.md)、移植したコードとモデルは [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) を参照してください。

## プライバシーとネットワーク

アプリはオフラインで動きます。トラッキング、パススルー、QR の読み取りに使うカメラ画像はスマホ上で処理され、保存も送信もされません。通信するのは、あなたが頼んだときだけです:

- **ビューアの QR コードの読み取り**: コード上の短いリンクを HTTPS でたどります（リダイレクトは最大 5 回）。Google の Cardboard アプリと同じように、Cardboard ビューアの URL に着くまで続けます。
- **`videos.txt` に並べた Google Drive の動画**: 再生するたびに最初の 1 回だけ Google Drive からダウンロードします。

Unity のアナリティクス、クラッシュレポート、エンジン診断はオフです。セッションログはスマホの中（アプリ自身のストレージ）に残り、トラッキングと性能の数値を含みますが、個人的な情報は含みません。

## ライセンス

Apache License 2.0 です。[LICENSE](LICENSE) を参照してください。同梱のサードパーティのコード、モデル、フォントはそれぞれのライセンスに従います。[THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) に一覧があります。
