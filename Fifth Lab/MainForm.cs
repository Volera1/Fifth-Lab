using Fifth_Lab.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fifth_Lab
{
    public partial class MainForm : Form
    {
        List<BaseObject> objects = new();
        GreenCircle[] greenCircles;
        Player player;
        Marker? marker;
        int score;

        public MainForm()
        {
            InitializeComponent();

            score = 0;
            greenCircles = new GreenCircle[2];
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            for (int i = 0; i < greenCircles.Length; i++)
            {
                greenCircles[i] = new GreenCircle(0, 0, 0);
                greenCircles[i].SetRandomPlace(pbMain.Width, pbMain.Height);
                greenCircles[i].OnMinimumSize += (c) =>
                {
                    c.SetRandomPlace(pbMain.Width, pbMain.Height);
                    c.x = 20;
                    c.y = 20;
                };

                objects.Add(greenCircles[i]);
            }

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj.GetType()}\n" + txtLog.Text;
            };
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            player.OnGreenCircleOverlap += (c) =>
            {
                score++;
                c.SetRandomPlace(pbMain.Width, pbMain.Height);
                c.x = 20;
                c.y = 20;
            };

            objects.Add(player);
        }

        //Перерасчёт позиции игрока
        private void UpdatePlayerPostiton()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        //Рендер объектов и проверка пересечений
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            UpdatePlayerPostiton();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
                if (obj is GreenCircle)
                {
                    var circle = (GreenCircle)obj;
                    circle.DecreaseSize();
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
            scoreLbl.Text = $"Очки: {score}";
        }

        //Тик таймера
        private void timer_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        //Нажатие на PaintBox и создание маркера на месте курсора
        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
