using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using GraphicsDLL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            this.controlPointsMirrored = GenerateMirroredControlPoints();
            AddAvailableStrategies();
            cbDecasteljau.SelectedIndex = 0;
            executeBtn.Focus();
        }

        private PointF[] GenerateControlPoints(int numberOfControlPoints, int maxWidth)
        {
            PointF[] points = new PointF[numberOfControlPoints];
            float frequency = 1 * (float)Math.PI / numberOfControlPoints;
            float spacing = maxWidth / numberOfControlPoints;
            const int AMPLITUDE = 600; // Height of the wave
            const int MARGIN_X = 60; // pixels from the left of the canvas
            const int MARGIN_Y = 50; // pixels from the top of the canvas

            for (int i = 0; i < numberOfControlPoints; i++)
            {
                float x = i * spacing; // spacing between points
                float y = AMPLITUDE * (float)Math.Sin(frequency * i); // if -1 it will be upside down
                points[i] = new PointF(x + MARGIN_X, y + MARGIN_Y);
            }

            return points;
        }

        private PointF[] GenerateMirroredControlPoints()
        {
            int n = this.controlPoints.Length;
            PointF[] points = new PointF[n];
            const int AMPLITUDE = 600; // Height of the wave
            const int MARGIN_Y = 50; // pixels from the top of the canvas
            for (int i = 0; i < n; i++)
            {
                PointF originalPoint = this.controlPoints[i];
                points[i] = new PointF(originalPoint.X, -originalPoint.Y + AMPLITUDE + MARGIN_Y);
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
            statusLbl.ForeColor = Color.White;
            statusLbl.BackColor = Color.DarkRed;
            statusLbl.Text = $"Error: {message}";
        }

        private void ExecuteButtonClicked(object sender, EventArgs e)
        {
            // Clear the canvas with a transparent background
            DrawBackground(graphics);
            ExecuteSelectedDeCasteljau();
            executeBtn.Focus();
        }

        private void ExecuteSelectedDeCasteljau()
        {
            if (cbDecasteljau.SelectedItem == null || string.IsNullOrWhiteSpace(cbDecasteljau.SelectedItem.ToString()))
            {
                DisplayErrorMessage("Please, select an implementation first!");
                return;
            }
            DeCasteljauStrategies selectedStrategy = (DeCasteljauStrategies)Enum.Parse(typeof(DeCasteljauStrategies), cbDecasteljau.SelectedItem.ToString(), false);
            DeCasteljauStrategy selectedImplementation = DeCasteljauFactory.Create(controlPoints, 0.01f, selectedStrategy);
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            PointF[] curvePoints = selectedImplementation.Iterate();
            selectedImplementation.ControlPoints = controlPointsMirrored;
            PointF[] mirroredCurvePoints = selectedImplementation.Iterate(); // reiterate with mirrored control points 
            stopwatch.Stop();
            statusLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
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

        private void DeCasteljauForm_Paint(object sender, PaintEventArgs e)
        {
            DrawBackground(e.Graphics);
        }

        private void DrawBackground(Graphics graphics)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle,
                              Color.LightBlue, Color.LightGreen, 45F))
            {
                graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
