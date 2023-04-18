using Exchange.Model;
using Exchange.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System.Collections.Generic;
using LiveCharts.Defaults;
using LiveCharts.Configurations;

namespace Exchange
{
    public class DateModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        bool _closed;
        private readonly Security _security;
        public Func<double, string> XFormatter { get; set; } =
            value =>
            {
                if (value >= 0) { return new System.DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("dd MMM"); }
                return "";
            };

        public event PropertyChangedEventHandler PropertyChanged;

        public SeriesCollection SeriesCollection { get;set; }

        public Window1()
        {
            InitializeComponent();
            this.Closed += Window1_Closed;

        }
        private void Window1_Closed(object sender, EventArgs e)
        {
            _closed = true;
        }

        public Window1(Security security)
        {
            InitializeComponent();
            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(dayModel => dayModel.Value);
            SeriesCollection = new SeriesCollection(dayConfig);
            DataContext = this;
            _security = security;
            Closed += Window1_Closed;
            Task.Run(() => LoadData());
            Task.Run(() => LoadHistory());
        }

        void LoadHistory()
        {
            HistoryLoader history = new HistoryLoader();
            var hs = history.LoadHistory(_security.secId, DateTime.Today.AddDays(-60).ToString("yyyy-MM-dd").Replace('.', '-'), DateTime.Today.ToString("yyyy-MM-dd").Replace('.', '-'));
            var prices = hs.Select(s => new DateModel
            {
                DateTime = DateTime.ParseExact(s.tradeDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Value = s.close
            }).ToArray();

            Dispatcher.Invoke(() =>
            {
                var series = new LineSeries
                {
                    Title = "Цена",
                    LineSmoothness = 0,
                    PointGeometrySize = 3,
                    PointForeground = Brushes.Gray
                };
                series.Values = new ChartValues<DateModel>(prices);
                SeriesCollection.Add(series);
            });
        }

        void LoadData()
        {
            MarketLoader marketData = new MarketLoader();
            var mk = marketData.LoadMarket(_security.secId);
            Dispatcher.Invoke(() =>
            {
                tbName.Text = _security.secName;
                tbSecId.Text = _security.secId;
                tbValToday.Text = mk[0].valToday.ToString() + " ₽";
                tbLast.Text = mk[0].last.ToString() + " ₽";
                tbVolToday.Text = mk[0].volToday.ToString();
                tbLastChange.Text = mk[0].lastChange < 0 ? $"{mk[0].lastChange}%" : $"+{mk[0].lastChange}%";
                tbTime.Text = mk[0].time.ToString();
            });
            if (_closed != true) {
                Thread.Sleep(5000);
                Task.Run(() => LoadData());
            }
        }



        void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HistoryLoader history = new HistoryLoader();
            DateTime currentDate = DateTime.Today;
            var hs = history.LoadHistory(_security.secId, currentDate.AddDays(-60).ToString("yyyy-MM-dd").Replace('.', '-'), currentDate.ToString("yyyy-MM-dd").Replace('.', '-'));
            var prices = hs.Select(p => (float)p.close).ToArray();
            var ml = new MLContext();
            var dataView = ml.Data.LoadFromEnumerable(hs);
            var inputColumnName = "floatClose";
            var outputColumnName = nameof(ForecastResult.Forecast);
            var model = ml.Forecasting.ForecastBySsa(outputColumnName, inputColumnName, 10, 20, prices.Length, 5);
            var transformer = model.Fit(dataView);
            var forecastEngine = transformer.CreateTimeSeriesEngine<History, ForecastResult>(ml);
            var forecast = forecastEngine.Predict();

            //последняя точка основного графика:
            var lastHistoryPoint = new DateModel { DateTime = currentDate = DateTime.Parse(hs.Last().tradeDate), Value = hs.Last().floatClose };

            Dispatcher.Invoke(() =>
            {                
                List<DateModel> predictedPrices = new List<DateModel>();
                predictedPrices.Add(lastHistoryPoint);
                for( int i = 0;i < forecast.Forecast.Length; ++i) {
                    DateModel dm = new DateModel { DateTime = currentDate.AddDays(i+1), Value = forecast.Forecast[i] };
                    predictedPrices.Add(dm);
                }
                
                var forecastSeries = new LineSeries
                {
                    Title = "Прогноз",
                    LineSmoothness = 0,
                    PointGeometrySize = 3,
                    PointForeground = Brushes.Red,
                };
                forecastSeries.Values = new ChartValues<DateModel>(predictedPrices);
                SeriesCollection.Add(forecastSeries);
            });
        }

        class ForecastResult
        {
            public float[] Forecast { get; set; }
        }

        private void Window_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void Window_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }
    }
}
