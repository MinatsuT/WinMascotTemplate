# Windows マスコット C# テンプレート

ほぼほぼ全てGitHub Copilotに書いてもらった、タスクバーの上に透過ウィンドウを作って透過PNGを表示するだけの、C# Windows Form アプリです。

- Visual Studio 2022 のプロジェクトです。
- キャラをドラッグして移動することだけできます。
- 終了処理が無いので、実行ファイルを素で実行した場合は、タスクマネージャーからプロセスを終了してください。
  - Visual Studioのデバッグ機能で実行した場合は、デバッグ終了ボタンでアプリケーションを終了できます。
- `Resources`フォルダにある`Character.png`を差し替えてプロジェクトをビルドし直すと、キャラクターを変更できるかもしれません（未確認）。
  - キャラを表示する際の高さはソース内で決め打ちしているので、変更したい場合は`Form1.cs`の`desiredHeight`を変更してください。