using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fifth_Lab.Objects
{
    class GreenCircle : BaseObject
    {
        public Action<GreenCircle>? OnMinimumSize;

        public float x;
        public float y;
        //Конструктор
        public GreenCircle(float x, float y, float angle) : base(x, y, angle) 
        {
            this.x = 20;
            this.y = 20;
        }

        //Отрисовка объекта
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.LightGreen), -x, -y, x*2, y*2);
        }

        //Получение пути к объекту
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-x, -y, x * 2, y * 2);
            return path;
        }

        public void DecreaseSize()
        {
            x -= 0.1f; 
            y -= 0.1f;
            if (x <= 1)
            {
                OnMinimumSize?.Invoke(this);
            }
        }
    }
}
