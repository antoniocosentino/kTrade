using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using System.Collections.Generic;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class kTradeBot : Robot
    {
        private HeikenAshi _heikenAshi;
        private int kcounter;
        private int pointscounter;
        private bool is_position_open;
        private string position_type;

        [Parameter("Contracts (EURO)", DefaultValue = 100000, MinValue = 1000, MaxValue = 100000000)]
        public int ncontracts { get; set; }

        [Parameter("Candles Sequence", DefaultValue = 2, MinValue = 1, MaxValue = 20)]
        public int csequence { get; set; }

        public static List<string> candleslist = new List<string>();


        protected override void OnStart()
        {
            // Put your initialization logic here
            Print("kTrade Started");
            _heikenAshi = Indicators.GetIndicator<HeikenAshi>(1);
            kcounter = 0;
            is_position_open = false;
            position_type = null;
        }

        protected override void OnBar()
        {
            kcounter++;
            pointscounter = 0;
            Print("Counter values is {0}", kcounter);

            if (_heikenAshi.xOpen.Last(1) < _heikenAshi.xClose.Last(1))
            {
                // Print("New bar. Last bar was: GREEN");
                candleslist.Add("green");
            }
            else
            {
                // Print("New bar. Last bar was: RED");
                candleslist.Add("red");
            }



            if (candleslist.Count <= csequence)
            {
                // do nothing
            }
            else
            {
                // remove first value from array
                candleslist.RemoveAt(0);
            }

            //candleslist.ForEach(kval => Print(kval));

            foreach (string value in candleslist)
            {
                if (value == "green")
                {
                    pointscounter++;
                }
                else
                {
                    pointscounter--;
                }
            }

            if (is_position_open == false)
            {
                if (pointscounter == csequence || pointscounter == (csequence * -1))
                {
                    Print("opening position");
                    is_position_open = true;

                    if (pointscounter == csequence)
                    {
                        var result = ExecuteMarketOrder(TradeType.Buy, Symbol, ncontracts, "kTrade");
                        position_type = "buy";
                    }
                    else
                    {
                        var result = ExecuteMarketOrder(TradeType.Sell, Symbol, ncontracts, "kTrade");
                        position_type = "sell";
                    }

                }
            }

            else
            {
                var position = Positions.Find("kTrade");
                if (pointscounter == csequence || pointscounter == (csequence * -1))
                {
                    if (pointscounter == csequence && position_type == "sell")
                    {
                        Print("closing position");
                        if (position != null)
                        {
                            ClosePosition(position);
                        }
                        Print("reversing position");
                        var result = ExecuteMarketOrder(TradeType.Buy, Symbol, ncontracts, "kTrade");
                        position_type = "buy";

                    }
                    else if (pointscounter == (csequence * -1) && position_type == "buy")
                    {
                        Print("closing position");
                        if (position != null)
                        {
                            ClosePosition(position);
                        }
                        Print("reversing position");
                        var result = ExecuteMarketOrder(TradeType.Sell, Symbol, ncontracts, "kTrade");
                        position_type = "sell";
                    }
                    else
                    {
                        Print("keep position open");
                    }
                }
            }


            Print(" - - - ");


        }
        // closing on bar case
        protected override void OnTick()
        {
            // Put your core logic here
        }

        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }
    }
}
