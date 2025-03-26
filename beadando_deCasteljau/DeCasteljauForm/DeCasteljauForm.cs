using System;
using System.Diagnostics;
using System.Drawing;
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
            AddAvailableStrategies();
        }

        private void AddAvailableStrategies()
        {
            var availableStrategies = Enum.GetValues(typeof(DeCasteljauStrategies));
            foreach (var strategy in availableStrategies)
            {
                cbDecasteljau.Items.Add(strategy);
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            graphics = canvas.CreateGraphics();
        }

        private void executeSelectedDecasteljauMethod()
        {
            PointF[] controlPointsCopy = new PointF[controlPoints.Length];
            Array.Copy(controlPoints, controlPointsCopy, controlPoints.Length);
            DeCasteljauStrategies selectedStrategy = (DeCasteljauStrategies) Enum.Parse(typeof(DeCasteljauStrategies), cbDecasteljau.SelectedItem.ToString(), false);
            DeCasteljauStrategy deCasteljau = StrategyFactory.Create(graphics, selectedStrategy);
            float actualDistance = 0.0f;
            float step = 0.1f;
            while (actualDistance < 1.0f)
            {
                try
                {
                    deCasteljau.Draw(controlPoints, actualDistance);
                }
                catch (Exception exception)
                {
                    elapsedTimeLbl.ForeColor = Color.DarkRed;
                    elapsedTimeLbl.Text = "Error: " + exception.Message;
                }
                actualDistance += step;
            }
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
