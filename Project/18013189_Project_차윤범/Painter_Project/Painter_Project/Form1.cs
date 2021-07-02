using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter_Project
{
    public partial class Form1 : Form
    {

        public static PaintTools toolType { get; set; }
        //그림판 툴 enum 변수 선언
        public enum PaintTools
        {
            IDLE = default,
            DrawLine,
            DrawRectangle,
            DrawCircle,
        }

        //Point
        private static Point clickPoint;
        private static Point UpPoint;
        private static Rectangle imgRect;

        //배경 bmp
        private static Bitmap OriginalBmp;
        private static Bitmap DrawBmp;

        //default 굵기
        public static int DefWidth = 3;
        public int width = DefWidth;

        /// <summary>
        /// 그림 위치, 도형형태 저장할 List 컬렉션을 이용해서 저장
        /// </summary>
        public List<Rectangle> listRect = new List<Rectangle>();
        public List<Rectangle> tempRect = new List<Rectangle>();
        public List<PaintTools> listTool = new List<PaintTools>();
        public List<PaintTools> tempTool = new List<PaintTools>();

        //선, 곡선을 그리는 객체
        Pen pn = new Pen(Color.Black, DefWidth);

        //초기화
        public Form1()
        {
            InitializeComponent();
            //White BackgroundImage Load
            pictureBox1.Image = new Bitmap(Application.StartupPath + @"\DefaultBackground.png");
            //Load한 이미지 크기를 PictureBox 컨트롤 크기와 동일하게 맞추어 줍니다.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            OriginalBmp = (Bitmap)pictureBox1.Image;
            imgRect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
        }

        //먼저 마우스 처음 클릭 위치를 clickPoint에 저장
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ////마우스 클릭 위치 저장
            clickPoint.X = e.X;
            clickPoint.Y = e.Y;
        }
        //마우스 이동마다 그림을 새로 그려준다
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //좌클릭 시
            if (e.Button == MouseButtons.Left)
            {
                //높이, 너비
                float w = Math.Abs(clickPoint.X - e.X);
                float h = Math.Abs(clickPoint.Y - e.Y);

                //pictureBox1의 Graphic 생성
                Graphics g = pictureBox1.CreateGraphics();
                pictureBox1.Refresh();
                if (toolType == PaintTools.DrawRectangle) //사각형 그리기
                {
                    if (e.X > clickPoint.X)
                    {
                        if (e.Y > clickPoint.Y) g.DrawRectangle(pn, clickPoint.X, clickPoint.Y, w, h);
                        else g.DrawRectangle(pn, clickPoint.X, e.Y, w, h);
                    }
                    else
                    {
                        if (e.Y > clickPoint.Y) g.DrawRectangle(pn, e.X, clickPoint.Y, w, h);
                        else g.DrawRectangle(pn, e.X, e.Y, w, h);
                    }
                }
                else if (toolType == PaintTools.DrawCircle) //원형 그리기
                {
                    if (e.X > clickPoint.X)
                    {
                        if (e.Y > clickPoint.Y) g.DrawEllipse(pn, clickPoint.X, clickPoint.Y, w, h);
                        else g.DrawEllipse(pn, clickPoint.X, e.Y, w, h);
                    }
                    else
                    {
                        if (e.Y > clickPoint.Y) g.DrawEllipse(pn, e.X, clickPoint.Y, w, h);
                        else g.DrawEllipse(pn, e.X, e.Y, w, h);
                    }
                }
                else if (toolType == PaintTools.DrawLine) //선 그리기
                {
                    g.DrawLine(pn, clickPoint.X, clickPoint.Y, e.X, e.Y);
                }
                
            }
        }
        
        //지우기 기능
        private void button1_Click(object sender, EventArgs e)
        {
            //기존 이미지 지우고
            pictureBox1.Image = null;
            OriginalBmp = null;

            //새로 초기화 선언
            listRect = new List<Rectangle>();
            tempRect = new List<Rectangle>();
            listTool = new List<PaintTools>();
            tempTool = new List<PaintTools>();
            pictureBox1.Image = new Bitmap(Application.StartupPath + @"\DefaultBackground.png");

            //Load한 이미지 크기를 PictureBox 컨트롤 크기와 동일하게 맞추어 줍니다.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            OriginalBmp = (Bitmap)pictureBox1.Image;
            imgRect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);

        }

        //선 그리기 버튼
        private void red_Click(object sender, EventArgs e)
        {
            toolType = PaintTools.DrawLine;
        }

        //사각형 그리기 버튼
        private void green_Click(object sender, EventArgs e)
        {
            toolType = PaintTools.DrawRectangle;
        }
        
        //사용자 색깔 지정
        private void button7_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pn = new Pen(dlg.Color, width);
            }
        }


        //마우스 왼쪽버튼을 뗐을 때 배경이미지에 그림을 그리고 리스트에 그린 정보를 저장한 후 DrawBmp으로 저장
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpPoint.X = e.X;
                UpPoint.Y = e.Y;

                float w = Math.Abs(clickPoint.X - e.X);
                float h = Math.Abs(clickPoint.Y - e.Y);

                Rectangle rect = new Rectangle();
                Graphics g = pictureBox1.CreateGraphics();

                if (toolType == PaintTools.DrawRectangle) //사각형 그리기
                {
                    if (e.X > clickPoint.X)
                    {
                        if (e.Y > clickPoint.Y) rect = new Rectangle(clickPoint.X, clickPoint.Y, (int)w, (int)h);
                        else rect = new Rectangle(clickPoint.X, e.Y, (int)w, (int)h);
                    }
                    else
                    {
                        if (e.Y > clickPoint.Y) rect = new Rectangle(e.X, clickPoint.Y, (int)w, (int)h);
                        else rect = new Rectangle(e.X, e.Y, (int)w, (int)h);
                    }
                }
                else if (toolType == PaintTools.DrawCircle) //원형 그리기
                {
                    if (e.X > clickPoint.X)
                    {
                        if (e.Y > clickPoint.Y) rect = new Rectangle(clickPoint.X, clickPoint.Y, (int)w, (int)h);
                        else rect = new Rectangle(clickPoint.X, e.Y, (int)w, (int)h);
                    }
                    else
                    {
                        if (e.Y > clickPoint.Y) rect = new Rectangle(e.X, clickPoint.Y, (int)w, (int)h);
                        else rect = new Rectangle(e.X, e.Y, (int)w, (int)h);
                    }
                }
                else if (toolType == PaintTools.DrawLine) //선형 그리기
                {
                    rect = new Rectangle(clickPoint.X, clickPoint.Y, clickPoint.X + e.X, clickPoint.Y + e.Y);
                }
                
                //리스트에 Rectangle 정보, Tool Type 정보 저장하기
                listRect.Add(rect);
                listTool.Add(toolType);
                DrawBitmap();
            }
        }

        private void DrawBitmap()
        {
            if (OriginalBmp != null)
            {
                DrawBmp = (Bitmap)OriginalBmp.Clone(); 
                for (int i = 0; i < listRect.Count; i++)
                {
                    double wRatio = (double)OriginalBmp.Width / pictureBox1.Width;
                    double hRatio = (double)OriginalBmp.Height / pictureBox1.Height;
                    Rectangle rect = new Rectangle((int)(listRect[i].X * wRatio), (int)((listRect[i].Y) * hRatio),
                            (int)(listRect[i].Width * wRatio), (int)(listRect[i].Height * hRatio));
                    
                    using (Graphics g = Graphics.FromImage(DrawBmp))
                    {
                        
                        if (listTool[i] == PaintTools.DrawRectangle) g.DrawRectangle(pn, rect);
                        else if (listTool[i] == PaintTools.DrawCircle) g.DrawEllipse(pn, rect);
                        else if (listTool[i] == PaintTools.DrawLine) g.DrawLine(pn, new Point(rect.X, rect.Y), new Point(rect.Width - rect.X, rect.Height - rect.Y));
                    }
                }
                pictureBox1.Image = DrawBmp;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (OriginalBmp != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                
                if (listRect.Count > 0 && DrawBmp != null)
                {
                    e.Graphics.DrawImage(DrawBmp, imgRect);
                }
                else
                {
                    e.Graphics.DrawImage(OriginalBmp, imgRect);
                }
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        // 원 그리기 버튼
        private void button2_Click(object sender, EventArgs e) 
        {
            toolType = PaintTools.DrawCircle;
        }

        
        //undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listRect.Count > 0)
            {
                //Undo 실행 취소
                //맨 마지막 list에 있던 거 temp에 저장하고 마지막 list 삭제
                tempRect.Add(listRect[listRect.Count - 1]);
                listRect.RemoveAt(listRect.Count - 1);
                tempTool.Add(listTool[listTool.Count - 1]);
                listTool.RemoveAt(listTool.Count - 1);
                pictureBox1.Refresh();
                DrawBitmap();
            }
        }
        //redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tempRect.Count > 0)
            {
                //Redo 다시 실행
                //맨 마지막 temp에 있던 거 list에 저장하고 마지막 temp 삭제
                listRect.Add(tempRect[tempRect.Count - 1]);
                tempRect.RemoveAt(tempRect.Count - 1);
                listTool.Add(tempTool[tempTool.Count - 1]);
                tempTool.RemoveAt(tempTool.Count - 1);
                pictureBox1.Refresh();
                DrawBitmap();
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Undo & Redo Shortcut 실행 취소 & 다시 실행 단축키
            if (e.Control && !e.Shift && e.KeyCode == Keys.Z)
            {
                //CTRL + Z : Undo 실행 취소
                if (listRect.Count > 0)
                {
                    //맨 마지막 list에 있던 거 temp에 저장하고 마지막 list 삭제
                    tempRect.Add(listRect[listRect.Count - 1]);
                    listRect.RemoveAt(listRect.Count - 1);
                    tempTool.Add(listTool[listTool.Count - 1]);
                    listTool.RemoveAt(listTool.Count - 1);
                    pictureBox1.Refresh();
                    DrawBitmap();
                }
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Z)
            {
                //CTRL + SHIFT + Z : Redo 다시 실행
                if (tempRect.Count > 0)
                {
                    //맨 마지막 temp에 있던 거 list에 저장하고 마지막 temp 삭제
                    listRect.Add(tempRect[tempRect.Count - 1]);
                    tempRect.RemoveAt(tempRect.Count - 1);
                    listTool.Add(tempTool[tempTool.Count - 1]);
                    tempTool.RemoveAt(tempTool.Count - 1);
                    pictureBox1.Refresh();
                    DrawBitmap();
                }
            }
        }

        //menustrip 선, 원, 사각형 그리기 클릭 이벤트
        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolType = PaintTools.DrawRectangle;
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolType = PaintTools.DrawCircle;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolType = PaintTools.DrawLine;
        }

        
        //선 굵기 지정
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                width = Convert.ToInt32("0");
                pn.Width = width;
            }
            else
            {
                width = Convert.ToInt32(textBox1.Text);
                pn.Width = width;
            }
            
        }
        // 이미지 불러오기
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //이미지 불러오기
                pictureBox1.Load(openFileDialog1.FileName);
               
                //새로 초기화
                listRect = new List<Rectangle>();
                tempRect = new List<Rectangle>();
                listTool = new List<PaintTools>();
                tempTool = new List<PaintTools>();
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                OriginalBmp = (Bitmap)pictureBox1.Image;
                imgRect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            }
        }
        // 이미지 저장하기
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        //파일 save, open 다이얼로그
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

       
    }
}
