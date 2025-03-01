using System;
using System.Diagnostics;
using System.Drawing;
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
            controlPoints = new PointF[] {
                new PointF(526, 585), new PointF(303, 5), new PointF(1080, 671),
                new PointF(1053, 538), new PointF(426, 666), new PointF(874, 468),
                new PointF(1097, 41), new PointF(1064, 318), new PointF(413, 135),
                new PointF(220, 481), new PointF(725, 628), new PointF(800, 417),
                new PointF(297, 95), new PointF(1, 94), new PointF(584, 110),
                new PointF(1085, 156), new PointF(887, 359), new PointF(747, 307),
                new PointF(1152, 687), new PointF(819, 627), new PointF(157, 695),
                new PointF(567, 32), new PointF(184, 122), new PointF(367, 61),
                new PointF(93, 534), new PointF(45, 128), new PointF(304, 714),
                new PointF(596, 708), new PointF(1109, 263), new PointF(200, 664),
                new PointF(1142, 529), new PointF(694, 71), new PointF(13, 296),
                new PointF(547, 659), new PointF(579, 112), new PointF(1071, 576),
                new PointF(38, 240), new PointF(899, 583), new PointF(669, 57),
                new PointF(607, 530)
            };
            InitializeComponent();
            elapsedTimeLbl.Text = "Elapsed time: ms";
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = canvas.CreateGraphics();
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
        }

        private async void executeBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            Stopwatch stopwatch = new Stopwatch();  // Ensure new Stopwatch instance
            stopwatch.Reset();
            stopwatch.Start();
            await executeCheckedDecasteljau();
            stopwatch.Stop();
            Console.WriteLine($"DeCasteljau execution time: {stopwatch.ElapsedMilliseconds} ms");
            elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
