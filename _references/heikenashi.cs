using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC)]
    public class HeikenAshi : Indicator
    {

        [Parameter("Period", DefaultValue = 1, MinValue = 1, MaxValue = 2147483647)]
        public int HeikenPeriod { get; set; }

        [Output("xClose")]
        public IndicatorDataSeries xClose { get; set; }

        [Output("xHigh")]
        public IndicatorDataSeries xHigh { get; set; }

        [Output("xLow")]
        public IndicatorDataSeries xLow { get; set; }

        [Output("xOpen")]
        public IndicatorDataSeries xOpen { get; set; }


        //private IndicatorDataSeries xClose;
        //private IndicatorDataSeries xHigh;
        //private IndicatorDataSeries xLow;
        //private IndicatorDataSeries xOpen;


        protected override void Initialize()
        {
            //xClose = CreateDataSeries();
            //xHigh = CreateDataSeries();
            //xLow = CreateDataSeries();
            //xOpen = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            if (index <= HeikenPeriod)
            {
                xOpen[index] = Math.Round((MarketSeries.Open[index] + MarketSeries.Close[index]) / 2, Symbol.Digits);
                xClose[index] = Math.Round((MarketSeries.Open[index] + MarketSeries.Low[index] + MarketSeries.High[index] + MarketSeries.Close[index]) / 4, Symbol.Digits);
                xHigh[index] = MarketSeries.High[index];
                xLow[index] = MarketSeries.Low[index];

                return;
            }

            xOpen[index] = Math.Round((xOpen[index - 1] + xClose[index - 1]) / 2, Symbol.Digits);
            xClose[index] = Math.Round((MarketSeries.Open[index] + MarketSeries.Low[index] + MarketSeries.High[index] + MarketSeries.Close[index]) / 4, Symbol.Digits);
            xHigh[index] = Math.Max(MarketSeries.High[index], Math.Max(xOpen[index], xClose[index]));
            xLow[index] = Math.Min(MarketSeries.Low[index], Math.Min(xOpen[index], xClose[index]));

            if (xClose[index] < xOpen[index])
            {
                ChartObjects.DrawLine("OpenClose-" + index.ToString(), index, xOpen[index], index, xClose[index], Colors.Red, 5, LineStyle.Solid);
                ChartObjects.DrawLine("HighLow-" + index.ToString(), index, xHigh[index], index, xLow[index], Colors.Red, 1, LineStyle.Solid);
            }
            else
            {
                ChartObjects.DrawLine("OpenClose-" + index.ToString(), index, xOpen[index], index, xClose[index], Colors.Green, 5, LineStyle.Solid);
                ChartObjects.DrawLine("HighLow-" + index.ToString(), index, xHigh[index], index, xLow[index], Colors.Green, 1, LineStyle.Solid);
            }
        }
    }
}
