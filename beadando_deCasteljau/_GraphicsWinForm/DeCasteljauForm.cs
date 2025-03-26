using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicsDLL;

namespace DeCasteljauForm
{
    public partial class DeCasteljauForm : Form
    {
        Graphics graphics;

        Random rnd = new Random();
        PointF[] controlPoints;


        private readonly Stopwatch stopwatch;
        public DeCasteljauForm()
        {
            this.stopwatch = new Stopwatch();
            this.controlPoints = new PointF[] {};
            InitializeComponent();
            elapsedTimeLbl.Text = "Elapsed time: ms";
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            graphics = canvas.CreateGraphics();
        }

        private void executeSelectedDecasteljauMethod()
        {
            PointF[] controlPointsCopy = new PointF[controlPoints.Length];
            Array.Copy(controlPoints, controlPointsCopy, controlPoints.Length);
            DeCasteljauStrategy selectedDecasteljauStrategy = CreateSelectedStrategy(graphics);
            float actualDistance = 0.0f;
            float step = 0.1f;
            while (actualDistance < 1.0f)
            {
                try
                {
                    selectedDecasteljauStrategy.Draw(controlPoints, actualDistance);
                }
                catch (Exception exception)
                {
                    elapsedTimeLbl.ForeColor = Color.DarkRed;
                    elapsedTimeLbl.Text = "Error: " + exception.Message;
                }
                actualDistance += step;
            }
        }

        private DeCasteljauStrategy CreateSelectedStrategy(Graphics graphics)
        {
            if (rbCasteljauRec.Checked)
                return new RecursiveParallelDeCasteljau(graphics);
            if (rbIterMultiThread.Checked)
                return new IterativeParallelDeCasteljau(graphics);
            return new IterativeSingleThreadedDeCasteljau(graphics);
        }

        private void executeBtn_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            Stopwatch stopwatch = new Stopwatch();  // Ensure new Stopwatch instance
            stopwatch.Reset();
            stopwatch.Start();
            executeSelectedDecasteljauMethod();
            stopwatch.Stop();
            //elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
