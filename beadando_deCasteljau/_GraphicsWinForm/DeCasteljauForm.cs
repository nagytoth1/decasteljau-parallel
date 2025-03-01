using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _GraphicsDLL;

namespace _GraphicsWinForm
{
    public partial class DeCasteljauForm : Form
    {
        Graphics g;

        Random rnd = new Random();
        PointF[] controlPoints;


        Stopwatch stopwatch;
        public DeCasteljauForm()
        {
            stopwatch = new Stopwatch();
            int n = 10;
            controlPoints = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                controlPoints[i] = new PointF(rnd.Next(50, 1000), rnd.Next(50, 1000));
            }
            InitializeComponent();
            elapsedTimeLbl.Text = "Elapsed time: ms";
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = canvas.CreateGraphics();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private async Task executeCheckedDecasteljau()
        {
            PointF[] controlPointsCopy = new PointF[controlPoints.Length];
            Array.Copy(controlPoints, controlPointsCopy, controlPoints.Length);
            if (rbCasteljauRec.Checked)
            {
                Console.WriteLine("Recursive execution selected");
                await Task.Run(() => Bezier3Curve.CallDeCasteljauRecursiveParallel(g, controlPointsCopy));
            }
            else if (rbIterMultiThread.Checked)
            {
                Console.WriteLine("Multithreaded execution");
                await Task.Run(() => Bezier3Curve.DeCasteljauParallel(g, controlPointsCopy));
            }
            else
            {
                Console.WriteLine("Iterative execution");
                await Task.Run(() => Bezier3Curve.DeCasteljau(g, controlPointsCopy));
            }
            Console.WriteLine($"DeCasteljau execution time: {stopwatch.ElapsedMilliseconds} ms");
        }

        private async void executeBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            Stopwatch stopwatch = new Stopwatch();  // Ensure new Stopwatch instance
            stopwatch.Reset();
            stopwatch.Start();
            await executeCheckedDecasteljau();
            stopwatch.Stop();
            elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
