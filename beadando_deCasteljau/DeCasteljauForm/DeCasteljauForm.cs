using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicsDLL;

namespace DeCasteljauForm
{
    public partial class DeCasteljauForm : Form
    {
        Graphics graphics;
        PointF[] controlPoints;
        PointF[] controlPointsMirrored;
        public DeCasteljauForm()
        {
            InitializeComponent();
            this.controlPoints = GenerateControlPoints(200, canvas.Width);
            this.controlPointsMirrored = GenerateControlPoints(200, canvas.Width, -1);
            AddAvailableStrategies();
        }

        private static PointF[] GenerateControlPoints(int numberOfControlPoints, int maxWidth, int multiplier = 1)
        {
            PointF[] points = new PointF[numberOfControlPoints];
            float frequency = 1 * (float)Math.PI / numberOfControlPoints; // Controls wave length
            float spacing = maxWidth / numberOfControlPoints;
            const int AMPLITUDE = 600; // Height of the wave
            const int MARGIN_X = 60; // pixels from the left of the canvas
            const int MARGIN_Y = 30; // pixels from the top of the canvas

            for (int i = 0; i < numberOfControlPoints; i++)
            {
                float x = i * spacing; // spacing between points
                float y = multiplier * AMPLITUDE * (float)Math.Sin(frequency * i); // if -1 it will be upside down
                points[i] = new PointF(x + MARGIN_X, y + MARGIN_Y);
            }

            return points;
        }


        private void AddAvailableStrategies()
        {
            var availableStrategies = Enum.GetValues(typeof(DeCasteljauStrategies));
            foreach (var strategy in availableStrategies)
            {
                cbDecasteljau.Items.Add(strategy);
            }
        }

        private void OnCanvasPainted(object sender, PaintEventArgs e)
        {
            graphics = canvas.CreateGraphics();
        }

        private void DisplayErrorMessage(string message)
        {
            elapsedTimeLbl.ForeColor = Color.White;
            elapsedTimeLbl.BackColor = Color.DarkRed;
            elapsedTimeLbl.Text = $"Error: {message}";
        }

        private void ExecuteButtonClicked(object sender, EventArgs e)
        {
            graphics.Clear(SystemColors.Control);
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            ExecuteSelectedDeCasteljau();
            stopwatch.Stop();
            elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }

        private void ExecuteSelectedDeCasteljau()
        {
            if (cbDecasteljau.SelectedItem == null || string.IsNullOrWhiteSpace(cbDecasteljau.SelectedItem.ToString()))
            {
                DisplayErrorMessage("Please, select an implementation first!");
                return;
            }
            DeCasteljauStrategies selectedStrategy = (DeCasteljauStrategies)Enum.Parse(typeof(DeCasteljauStrategies), cbDecasteljau.SelectedItem.ToString(), false);
            DeCasteljauStrategy selectedImplementation1 = DeCasteljauFactory.Create(controlPoints, 0.01f, selectedStrategy);
            DeCasteljauStrategy selectedImplementation2 = DeCasteljauFactory.Create(controlPointsMirrored, 0.01f, selectedStrategy);
            PointF[] curvePoints = selectedImplementation1.Iterate();
            PointF[] mirroredCurvePoints = selectedImplementation2.Iterate();
            DrawResult(curvePoints);
            DrawResult(mirroredCurvePoints);
        }

        private void DrawResult(PointF[] curvePoints)
        {
            Pen pen = new Pen(Color.Black, 3f);
            PointF previousPoint = curvePoints[0];
            for (int i = 1; i < curvePoints.Length; i++)
            {
                graphics.DrawLine(pen, previousPoint, curvePoints[i]);
                previousPoint = curvePoints[i];
            }
        }
    }
}
