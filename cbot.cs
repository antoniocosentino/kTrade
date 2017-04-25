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
        private int outpoints;
        private bool is_position_open;
        private string position_type;
        private string stringdate;
        private int currenthour;
        private int currentminute;
        private int currentslotnumber;
        private bool botactive;
        private int hasclosedft;

        [Parameter("Contracts (EURO)", DefaultValue = 100000, MinValue = 1, MaxValue = 100000000)]
        public int ncontracts { get; set; }

        [Parameter("Candles Seq. IN", DefaultValue = 2, MinValue = 1, MaxValue = 100)]
        public int csequence { get; set; }

        [Parameter("Candles Seq. OUT", DefaultValue = 2, MinValue = 1, MaxValue = 100)]
        public int outsequence { get; set; }

        [Parameter("Invert Buy/Sell", DefaultValue = false)]
        public bool inverted { get; set; }

        [Parameter("Take Profit (pips)", DefaultValue = 0, MinValue = 0, MaxValue = 100000000)]
        public int takeprofit { get; set; }

        [Parameter("Stop Loss (pips)", DefaultValue = 0, MinValue = 0, MaxValue = 100000000)]
        public int stoploss { get; set; }

        [Parameter("Restart After Profit", DefaultValue = false)]
        public bool restartap { get; set; }

        // Time Parameters
        [Parameter("0:00 - 0:29", DefaultValue = true)]
        public bool timeslot_00 { get; set; }

        [Parameter("0:30 - 0:59", DefaultValue = true)]
        public bool timeslot_01 { get; set; }

        [Parameter("1:00 - 1:29", DefaultValue = true)]
        public bool timeslot_02 { get; set; }

        [Parameter("1:30 - 1:59", DefaultValue = true)]
        public bool timeslot_03 { get; set; }

        [Parameter("2:00 - 2:29", DefaultValue = true)]
        public bool timeslot_04 { get; set; }

        [Parameter("2:30 - 2:59", DefaultValue = true)]
        public bool timeslot_05 { get; set; }

        [Parameter("3:00 - 3:29", DefaultValue = true)]
        public bool timeslot_06 { get; set; }

        [Parameter("3:30 - 3:59", DefaultValue = true)]
        public bool timeslot_07 { get; set; }

        [Parameter("4:00 - 4:29", DefaultValue = true)]
        public bool timeslot_08 { get; set; }

        [Parameter("4:30 - 4:59", DefaultValue = true)]
        public bool timeslot_09 { get; set; }

        [Parameter("5:00 - 5:29", DefaultValue = true)]
        public bool timeslot_10 { get; set; }

        [Parameter("5:30 - 5:59", DefaultValue = true)]
        public bool timeslot_11 { get; set; }

        [Parameter("6:00 - 6:29", DefaultValue = true)]
        public bool timeslot_12 { get; set; }

        [Parameter("6:30 - 6:59", DefaultValue = true)]
        public bool timeslot_13 { get; set; }

        [Parameter("7:00 - 7:29", DefaultValue = true)]
        public bool timeslot_14 { get; set; }

        [Parameter("7:30 - 7:59", DefaultValue = true)]
        public bool timeslot_15 { get; set; }

        [Parameter("8:00 - 8:29", DefaultValue = true)]
        public bool timeslot_16 { get; set; }

        [Parameter("8:30 - 8:59", DefaultValue = true)]
        public bool timeslot_17 { get; set; }

        [Parameter("9:00 - 9:29", DefaultValue = true)]
        public bool timeslot_18 { get; set; }

        [Parameter("9:30 - 9:59", DefaultValue = true)]
        public bool timeslot_19 { get; set; }

        [Parameter("10:00 - 10:29", DefaultValue = true)]
        public bool timeslot_20 { get; set; }

        [Parameter("10:30 - 10:59", DefaultValue = true)]
        public bool timeslot_21 { get; set; }

        [Parameter("11:00 - 11:29", DefaultValue = true)]
        public bool timeslot_22 { get; set; }

        [Parameter("11:30 - 11:59", DefaultValue = true)]
        public bool timeslot_23 { get; set; }

        [Parameter("12:00 - 12:29", DefaultValue = true)]
        public bool timeslot_24 { get; set; }

        [Parameter("12:30 - 12:59", DefaultValue = true)]
        public bool timeslot_25 { get; set; }

        [Parameter("13:00 - 13:29", DefaultValue = true)]
        public bool timeslot_26 { get; set; }

        [Parameter("13:30 - 13:59", DefaultValue = true)]
        public bool timeslot_27 { get; set; }

        [Parameter("14:00 - 14:29", DefaultValue = true)]
        public bool timeslot_28 { get; set; }

        [Parameter("14:30 - 14:59", DefaultValue = true)]
        public bool timeslot_29 { get; set; }

        [Parameter("15:00 - 15:29", DefaultValue = true)]
        public bool timeslot_30 { get; set; }

        [Parameter("15:30 - 15:59", DefaultValue = true)]
        public bool timeslot_31 { get; set; }

        [Parameter("16:00 - 16:29", DefaultValue = true)]
        public bool timeslot_32 { get; set; }

        [Parameter("16:30 - 16:59", DefaultValue = true)]
        public bool timeslot_33 { get; set; }

        [Parameter("17:00 - 17:29", DefaultValue = true)]
        public bool timeslot_34 { get; set; }

        [Parameter("17:30 - 17:59", DefaultValue = true)]
        public bool timeslot_35 { get; set; }

        [Parameter("18:00 - 18:29", DefaultValue = true)]
        public bool timeslot_36 { get; set; }

        [Parameter("18:30 - 18:59", DefaultValue = true)]
        public bool timeslot_37 { get; set; }

        [Parameter("19:00 - 19:29", DefaultValue = true)]
        public bool timeslot_38 { get; set; }

        [Parameter("19:30 - 19:59", DefaultValue = true)]
        public bool timeslot_39 { get; set; }

        [Parameter("20:00 - 20:29", DefaultValue = true)]
        public bool timeslot_40 { get; set; }

        [Parameter("20:30 - 20:59", DefaultValue = true)]
        public bool timeslot_41 { get; set; }

        [Parameter("21:00 - 21:29", DefaultValue = true)]
        public bool timeslot_42 { get; set; }

        [Parameter("21:30 - 21:59", DefaultValue = true)]
        public bool timeslot_43 { get; set; }

        [Parameter("22:00 - 22:29", DefaultValue = true)]
        public bool timeslot_44 { get; set; }

        [Parameter("22:30 - 22:59", DefaultValue = true)]
        public bool timeslot_45 { get; set; }

        [Parameter("23:00 - 23:29", DefaultValue = true)]
        public bool timeslot_46 { get; set; }

        [Parameter("23:30 - 23:59", DefaultValue = true)]
        public bool timeslot_47 { get; set; }

        //

        public static List<string> candleslist = new List<string>();
        public static List<string> outlist = new List<string>();
        public static List<int> blackslots = new List<int>();

        protected override void OnStart()
        {
            // Put your initialization logic here
            Print("kTrade 1.3.1 started");
            Print("Server time is {0}", Server.Time.AddHours(0));
            _heikenAshi = Indicators.GetIndicator<HeikenAshi>(1);
            kcounter = 0;
            is_position_open = false;
            position_type = null;
            stringdate = "";
            currenthour = 0;
            currentminute = 0;
            currentslotnumber = 0;
            botactive = false;
            hasclosedft = 0;

            candleslist.Clear();
            outlist.Clear();
            blackslots.Clear();

            Positions.Closed += PositionsOnClosed;

            // Adding "black" hours

            if (!timeslot_00)
            {
                blackslots.Add(0);
            }

            if (!timeslot_01)
            {
                blackslots.Add(1);
            }

            if (!timeslot_02)
            {
                blackslots.Add(2);
            }

            if (!timeslot_03)
            {
                blackslots.Add(3);
            }

            if (!timeslot_04)
            {
                blackslots.Add(4);
            }

            if (!timeslot_05)
            {
                blackslots.Add(5);
            }

            if (!timeslot_06)
            {
                blackslots.Add(6);
            }

            if (!timeslot_07)
            {
                blackslots.Add(7);
            }

            if (!timeslot_08)
            {
                blackslots.Add(8);
            }

            if (!timeslot_09)
            {
                blackslots.Add(9);
            }

            if (!timeslot_10)
            {
                blackslots.Add(10);
            }

            if (!timeslot_11)
            {
                blackslots.Add(11);
            }

            if (!timeslot_12)
            {
                blackslots.Add(12);
            }

            if (!timeslot_13)
            {
                blackslots.Add(13);
            }

            if (!timeslot_14)
            {
                blackslots.Add(14);
            }

            if (!timeslot_15)
            {
                blackslots.Add(15);
            }

            if (!timeslot_16)
            {
                blackslots.Add(16);
            }

            if (!timeslot_17)
            {
                blackslots.Add(17);
            }

            if (!timeslot_18)
            {
                blackslots.Add(18);
            }

            if (!timeslot_19)
            {
                blackslots.Add(19);
            }

            if (!timeslot_20)
            {
                blackslots.Add(20);
            }

            if (!timeslot_21)
            {
                blackslots.Add(21);
            }

            if (!timeslot_22)
            {
                blackslots.Add(22);
            }

            if (!timeslot_23)
            {
                blackslots.Add(23);
            }

            if (!timeslot_24)
            {
                blackslots.Add(24);
            }

            if (!timeslot_25)
            {
                blackslots.Add(25);
            }

            if (!timeslot_26)
            {
                blackslots.Add(26);
            }

            if (!timeslot_27)
            {
                blackslots.Add(27);
            }

            if (!timeslot_28)
            {
                blackslots.Add(28);
            }

            if (!timeslot_29)
            {
                blackslots.Add(29);
            }

            if (!timeslot_30)
            {
                blackslots.Add(30);
            }

            if (!timeslot_31)
            {
                blackslots.Add(31);
            }

            if (!timeslot_32)
            {
                blackslots.Add(32);
            }

            if (!timeslot_33)
            {
                blackslots.Add(33);
            }

            if (!timeslot_34)
            {
                blackslots.Add(34);
            }

            if (!timeslot_35)
            {
                blackslots.Add(35);
            }

            if (!timeslot_36)
            {
                blackslots.Add(36);
            }

            if (!timeslot_37)
            {
                blackslots.Add(37);
            }

            if (!timeslot_38)
            {
                blackslots.Add(38);
            }

            if (!timeslot_39)
            {
                blackslots.Add(39);
            }

            if (!timeslot_40)
            {
                blackslots.Add(40);
            }

            if (!timeslot_41)
            {
                blackslots.Add(41);
            }

            if (!timeslot_42)
            {
                blackslots.Add(42);
            }

            if (!timeslot_43)
            {
                blackslots.Add(43);
            }

            if (!timeslot_44)
            {
                blackslots.Add(44);
            }

            if (!timeslot_45)
            {
                blackslots.Add(45);
            }

            if (!timeslot_46)
            {
                blackslots.Add(46);
            }

            if (!timeslot_47)
            {
                blackslots.Add(47);
            }

            // "black" hours setup end

        }

        protected override void OnBar()
        {
            kcounter++;
            pointscounter = 0;
            outpoints = 0;

            stringdate = Server.Time.AddHours(1).ToString("HH:mm");

            currenthour = int.Parse(stringdate.Substring(0, 2));
            currenthour = Convert.ToInt32(stringdate.Substring(0, 2));

            currentminute = int.Parse(stringdate.Substring(3, 2));
            currentminute = Convert.ToInt32(stringdate.Substring(3, 2));

            if (currentminute >= 30)
            {
                currentslotnumber = (currenthour * 2) + 1;
            }
            else
            {
                currentslotnumber = currenthour * 2;
            }


            if (blackslots.Any(item => item == currentslotnumber))
            {
                botactive = false;

                if (hasclosedft == 1)
                {
                    hasclosedft = 2;
                }

                if (is_position_open)
                {
                    Print("Closing active position");
                    var position = Positions.Find("kTrade");
                    ClosePosition(position);
                    is_position_open = false;
                    position_type = null;
                }


            }
            else
            {
                if (restartap)
                {
                    botactive = true;
                }
                else if (restartap == false && (hasclosedft == 2 || hasclosedft == 0))
                {
                    botactive = true;
                }
            }








            if (_heikenAshi.xOpen.Last(1) < _heikenAshi.xClose.Last(1))
            {
                candleslist.Add("green");
                outlist.Add("green");
            }
            else
            {
                candleslist.Add("red");
                outlist.Add("red");
            }



            if (candleslist.Count <= csequence)
            {
                // do nothing
            }
            else
            {
                candleslist.RemoveAt(0);
            }

            foreach (string value in candleslist)
            {
                if (value == "green")
                {
                    if (inverted)
                    {
                        pointscounter--;
                    }
                    else
                    {
                        pointscounter++;
                    }
                }
                else
                {
                    if (inverted)
                    {
                        pointscounter++;
                    }
                    else
                    {
                        pointscounter--;
                    }
                }
            }

            if (outlist.Count <= outsequence)
            {
                // do nothing
            }
            else
            {
                outlist.RemoveAt(0);
            }

            foreach (string value in outlist)
            {
                if (value == "green")
                {
                    if (inverted)
                    {
                        outpoints--;
                    }
                    else
                    {
                        outpoints++;
                    }
                }
                else
                {
                    if (inverted)
                    {
                        outpoints++;
                    }
                    else
                    {
                        outpoints--;
                    }
                }
            }




            if (is_position_open == false)
            {
                if (pointscounter == csequence || pointscounter == (csequence * -1))
                {
                    if (botactive)
                    {
                        Print("Opening position");
                        is_position_open = true;

                        if (pointscounter == csequence)
                        {
                            var result = ExecuteMarketOrder(TradeType.Buy, Symbol, ncontracts, "kTrade", stoploss, takeprofit);
                            position_type = "buy";
                        }
                        else
                        {
                            var result = ExecuteMarketOrder(TradeType.Sell, Symbol, ncontracts, "kTrade", stoploss, takeprofit);
                            position_type = "sell";
                        }
                    }
                }
            }

            else
            {
                var position = Positions.Find("kTrade");
                if (outpoints == outsequence || outpoints == (outsequence * -1))
                {
                    if (outpoints == outsequence && position_type == "sell")
                    {
                        Print("Closing position");
                        if (position != null)
                        {
                            ClosePosition(position);
                            is_position_open = false;
                            position_type = null;
                            candleslist.Clear();
                            outlist.Clear();
                        }
                    }
                    else if (outpoints == (outsequence * -1) && position_type == "buy")
                    {
                        Print("Closing position");
                        if (position != null)
                        {
                            ClosePosition(position);
                            is_position_open = false;
                            position_type = null;
                            candleslist.Clear();
                            outlist.Clear();
                        }
                    }
                    else
                    {
                        // Keep position open
                    }
                }
            }
        }
        // end of on bar event
        protected override void OnTick()
        {
            // no onTick events
        }

        protected override void OnStop()
        {
            Print("kTrade stopped");
        }

        private void PositionsOnClosed(PositionClosedEventArgs args)
        {
            var pos = args.Position;
            Print("Position closed with €{0} profit", pos.GrossProfit);
            is_position_open = false;
            position_type = null;

            if (!restartap)
            {
                hasclosedft = 1;
                botactive = false;
            }

        }

    }
}
