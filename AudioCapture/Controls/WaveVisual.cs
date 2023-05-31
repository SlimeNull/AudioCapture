using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioCapture.Controls
{
    public class WaveVisual : Panel
    {

        public double[]? Data { get; set; }


        public bool EnableSmoothCurve { get; set; }
        public bool CenterWave { get; set; }

        public float Magnification { get; set; } = 1;


        public void Render()
        {
            double[]? data = this.Data;
            if (data == null)
                return;

            using Graphics g = CreateGraphics();
            using BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(g, ClientRectangle);

            using Brush brush = new SolidBrush(ForeColor);
            using Pen pen = new Pen(brush, 1);

            // save the instance to field
            Graphics bgg = bg.Graphics;

            float magnification = Magnification;

            int height = CenterWave ? Height / 2 : Height;

            PointF[] points = data
                .Select((v, i) => new PointF(Width * ((float)i / data.Length), (float)(height - (v * magnification))))
                .ToArray();

            bgg.Clear(BackColor);

            try
            {
                if (EnableSmoothCurve)
                    bgg.DrawCurve(pen, points);
                else
                    bgg.DrawLines(pen, points);
            }
            catch { }

            bg.Render();
        }
    }
}
