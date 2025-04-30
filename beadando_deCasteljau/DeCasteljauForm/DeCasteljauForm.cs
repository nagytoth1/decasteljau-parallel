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
        PointF[] controlPoints;
        private readonly Stopwatch stopwatch = new Stopwatch();
        public DeCasteljauForm()
        {
            this.stopwatch = new Stopwatch();
            InitializeComponent();
            this.controlPoints = InitializeControlPoints(50);
            AddAvailableStrategies();
        }

        private PointF[] InitializeControlPoints(int quantity)
        {
            PointF[] pointsArray = new PointF[quantity];
            int minX = 10;
            int maxX = canvas.Width - 10;
            int step = (maxX - minX) / quantity;
            int actualX = minX;
            pointsArray[0] = new PointF(actualX, 100);
            actualX += step;
            pointsArray[1] = new PointF(actualX, 100);
            for (int i = 2; i < quantity - 2; i++)
            {
                actualX += step;
                pointsArray[i] = new PointF(actualX, 400);
            }
            actualX += step;
            pointsArray[quantity-2] = new PointF(actualX, 100);
            actualX += step;
            pointsArray[quantity-1] = new PointF(actualX, 100);
            return pointsArray;
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

        private async void ExecuteButtonClicked(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            stopwatch.Reset();
            stopwatch.Start();
            await ExecuteSelectedDeCasteljau();
            stopwatch.Stop();
            elapsedTimeLbl.Text = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms";
        }

        private async Task ExecuteSelectedDeCasteljau()
        {
            if (cbDecasteljau.SelectedItem == null || string.IsNullOrWhiteSpace(cbDecasteljau.SelectedItem.ToString()))
            {
                DisplayErrorMessage("Please, select an implementation first!");
                return;
            }
            DeCasteljauStrategies selectedStrategyValue = (DeCasteljauStrategies)Enum.Parse(typeof(DeCasteljauStrategies), cbDecasteljau.SelectedItem.ToString(), false);
            await Task.Run(() =>
            {
                DeCasteljauStrategy selectedDeCasteljau = DeCasteljauFactory.Create(graphics, selectedStrategyValue);
                Iterate(selectedDeCasteljau);
            });
        }
    }
}
