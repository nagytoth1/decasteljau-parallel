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
            this.controlPoints = new PointF[] {
                new PointF( 100, 300 ),
                new PointF( 200, 100 ),
                new PointF( 500, 100 ),
                new PointF( 600, 300 )
            };
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

        private void ExecuteSelectedDecasteljau()
        {
            if (cbDecasteljau.SelectedItem == null || string.IsNullOrWhiteSpace(cbDecasteljau.SelectedItem.ToString()))
            {
                DisplayErrorMessage("Please, select an implementation first!");
                return;
            }

            DeCasteljauStrategies selectedStrategyValue = (DeCasteljauStrategies) Enum.Parse(typeof(DeCasteljauStrategies), cbDecasteljau.SelectedItem.ToString(), false);
            DeCasteljauStrategy selectedDeCasteljau = StrategyFactory.Create(graphics, selectedStrategyValue);
            Iterate(selectedDeCasteljau, 0.03f);
        }

        private void Iterate(DeCasteljauStrategy selectedDeCasteljau, float step = 0.25f)
        {
            PointF[] controlPointsCopy = new PointF[controlPoints.Length];
            float actualDistance;
            for(actualDistance = 0.0f; actualDistance <= 1.0f; actualDistance += step)
            {
                try
                {
                    Array.Copy(controlPoints, controlPointsCopy, controlPoints.Length);
                    selectedDeCasteljau.Draw(controlPointsCopy, actualDistance);
                }
                catch (Exception exception)
                {
                    DisplayErrorMessage(exception.Message);
                }
            }
            if (actualDistance < 1.0f) // run one last time if it hasn't overshoot 1 yet
            {
                Array.Copy(controlPoints, controlPointsCopy, controlPoints.Length);
                selectedDeCasteljau.Draw(controlPointsCopy, actualDistance);
            }
        }

        private void executeBtn_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            Stopwatch stopwatch = new Stopwatch();  // Ensure new Stopwatch instance
            stopwatch.Reset();
            stopwatch.Start();
            ExecuteSelectedDecasteljau();
            stopwatch.Stop();
            //elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
