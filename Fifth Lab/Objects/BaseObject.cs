using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fifth_Lab.Objects
{
    class BaseObject
    {
        public float X;
        public float Y;
        public float Angle;
        private readonly Random random = new();

        //Делегат вызываемый при пересечении двух объектов
        public Action<BaseObject, BaseObject>? OnOverlap;

        //Конструктор 
        public BaseObject(float x, float y, float angle)
        {
            X = x;
            Y = y;
            Angle = angle;
        }

        public void SetRandomPlace(int xMax, int yMax)
        {
            X = random.Next(20, xMax - 20);
            Y = random.Next(20, yMax - 20);
        }

        //Получение расположения объекта
        public Matrix GetTransform()
        {
            var matrix = new Matrix();
            matrix.Translate(X, Y);
            matrix.Rotate(Angle);

            return matrix;
        }

        //Получение ссылки на объект
        public virtual GraphicsPath GetGraphicsPath()
        {
            return new GraphicsPath();
        }

        //Рендер объекта
        public virtual void Render(Graphics g) { }

        //Проверка пересечения
        public virtual bool Overlaps(BaseObject obj, Graphics g)
        {
            var path1 = GetGraphicsPath();
            var path2 = obj.GetGraphicsPath();

            path1.Transform(GetTransform());
            path2.Transform(obj.GetTransform());

            var region = new Region(path1);
            region.Intersect(path2);
            return !region.IsEmpty(g);
        }

        //Вызов делегата
        public virtual void Overlap(BaseObject obj)
        {
            if (OnOverlap != null)
            {
                OnOverlap(this, obj);
            }
        }
    }
}
