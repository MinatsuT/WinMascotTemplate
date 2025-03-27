using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WinMascot
{
    public partial class Form1 : Form
    {
        private PictureBox characterPictureBox;
        private bool isDragging = false;
        private Point lastMousePos;
        private Point dragOffset;

        public Form1()
        {
            InitializeComponent();

            // フォームの設定
            this.FormBorderStyle = FormBorderStyle.None;  // 枠を非表示
            this.TransparencyKey = Color.Magenta;  // マゼンタ色を透過色に設定
            this.BackColor = Color.Magenta;  // 背景色をマゼンタに設定
            this.TopMost = true;  // 常に最前面に表示
            this.ShowInTaskbar = false;  // タスクバーに表示しない

            // キャラクター用のPictureBoxを追加
            characterPictureBox = new PictureBox();
            characterPictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Zoomモードに設定
            characterPictureBox.BackColor = Color.Transparent;
            characterPictureBox.Image = Properties.Resources.Character;

            // 画像の高さを設定（幅は比率を維持して自動計算）
            int desiredHeight = 512; // 希望する高さ（ピクセル）
            if (characterPictureBox.Image != null)
            {
                // 元の画像のアスペクト比を計算
                float aspectRatio = (float)characterPictureBox.Image.Width / characterPictureBox.Image.Height;

                // 新しい幅を計算（アスペクト比を維持）
                int newWidth = (int)(desiredHeight * aspectRatio);

                // PictureBoxのサイズを設定
                characterPictureBox.Size = new Size(newWidth, desiredHeight);
            }
            this.Controls.Add(characterPictureBox);

            // フォームサイズをPictureBoxのサイズに合わせる（修正部分）
            this.ClientSize = characterPictureBox.Size;

            // マウスイベントハンドラーを追加
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            
            // characterPictureBoxにもイベントハンドラーを追加
            characterPictureBox.MouseDown += Form1_MouseDown;
            characterPictureBox.MouseMove += Form1_MouseMove;
            characterPictureBox.MouseUp += Form1_MouseUp;

            // タスクバーの上に配置
            PositionFormAboveTaskbar();

            // リサイズやディスプレイ設定変更時にも位置を再調整
            this.Resize += (s, e) => PositionFormAboveTaskbar();
            SystemEvents.DisplaySettingsChanged += (s, e) => PositionFormAboveTaskbar();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                // カーソル位置とフォーム位置の差を記録
                dragOffset = new Point(Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // フォームの位置をカーソル位置とオフセットに基づいて更新（横方向のみ）
                this.Location = new Point(Cursor.Position.X - dragOffset.X, this.Location.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void PositionFormAboveTaskbar()
        {
            // タスクバーの情報を取得
            var taskbarRect = GetTaskbarRect();

            // タスクバーの上端に配置
            switch (GetTaskbarPosition())
            {
                case TaskbarPosition.Bottom:
                    this.Location = new Point(
                        taskbarRect.Right - this.Width - 50,  // タスクバーの右端から少し左に
                        taskbarRect.Top - this.Height        // タスクバーの上に配置
                    );
                    break;
                    // 他のタスクバー位置の場合も対応可能...
            }
        }

        private Rectangle GetTaskbarRect()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Rectangle workArea = Screen.PrimaryScreen.WorkingArea;

            // タスクバーの位置に基づいて矩形を計算
            switch (GetTaskbarPosition())
            {
                case TaskbarPosition.Bottom:
                    return new Rectangle(
                        workArea.Left,
                        workArea.Bottom,
                        workArea.Width,
                        screenBounds.Bottom - workArea.Bottom);
                case TaskbarPosition.Top:
                    return new Rectangle(
                        workArea.Left,
                        screenBounds.Top,
                        workArea.Width,
                        workArea.Top - screenBounds.Top);
                case TaskbarPosition.Left:
                    return new Rectangle(
                        screenBounds.Left,
                        workArea.Top,
                        workArea.Left - screenBounds.Left,
                        workArea.Height);
                case TaskbarPosition.Right:
                    return new Rectangle(
                        workArea.Right,
                        workArea.Top,
                        screenBounds.Right - workArea.Right,
                        workArea.Height);
                default:
                    return Rectangle.Empty;
            }
        }

        private enum TaskbarPosition { Top, Bottom, Left, Right }

        private TaskbarPosition GetTaskbarPosition()
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            var workArea = Screen.PrimaryScreen.WorkingArea;

            if (workArea.Top > screenBounds.Top) return TaskbarPosition.Top;
            if (workArea.Left > screenBounds.Left) return TaskbarPosition.Left;
            if (workArea.Right < screenBounds.Right) return TaskbarPosition.Right;

            return TaskbarPosition.Bottom;  // デフォルト
        }
    }
}
