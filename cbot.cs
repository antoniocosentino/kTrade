///-----------------------------------------------------------------
///   Class:            kTrade BOT
///   Description:      cAlgo trading bot
///   Author:           Antonio Cosentino
///   Version:          1.5.1
///   Updated:          08/06/2017
///-----------------------------------------------------------------


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
        [Parameter("00:00 - 00:14", DefaultValue = true)]
        public bool timeslot_00 { get; set; }

        [Parameter("00:15 - 00:29", DefaultValue = true)]
        public bool timeslot_01 { get; set; }

        [Parameter("00:30 - 00:44", DefaultValue = true)]
        public bool timeslot_02 { get; set; }

        [Parameter("00:45 - 00:59", DefaultValue = true)]
        public bool timeslot_03 { get; set; }

        [Parameter("01:00 - 01:14", DefaultValue = true)]
        public bool timeslot_04 { get; set; }

        [Parameter("01:15 - 01:29", DefaultValue = true)]
        public bool timeslot_05 { get; set; }

        [Parameter("01:30 - 01:44", DefaultValue = true)]
        public bool timeslot_06 { get; set; }

        [Parameter("01:45 - 01:59", DefaultValue = true)]
        public bool timeslot_07 { get; set; }

        [Parameter("02:00 - 02:14", DefaultValue = true)]
        public bool timeslot_08 { get; set; }

        [Parameter("02:15 - 02:29", DefaultValue = true)]
        public bool timeslot_09 { get; set; }

        [Parameter("02:30 - 02:44", DefaultValue = true)]
        public bool timeslot_10 { get; set; }

        [Parameter("02:45 - 02:59", DefaultValue = true)]
        public bool timeslot_11 { get; set; }

        [Parameter("03:00 - 03:14", DefaultValue = true)]
        public bool timeslot_12 { get; set; }

        [Parameter("03:15 - 03:29", DefaultValue = true)]
        public bool timeslot_13 { get; set; }

        [Parameter("03:30 - 03:44", DefaultValue = true)]
        public bool timeslot_14 { get; set; }

        [Parameter("03:45 - 03:59", DefaultValue = true)]
        public bool timeslot_15 { get; set; }

        [Parameter("04:00 - 04:14", DefaultValue = true)]
        public bool timeslot_16 { get; set; }

        [Parameter("04:15 - 04:29", DefaultValue = true)]
        public bool timeslot_17 { get; set; }

        [Parameter("04:30 - 04:44", DefaultValue = true)]
        public bool timeslot_18 { get; set; }

        [Parameter("04:45 - 04:59", DefaultValue = true)]
        public bool timeslot_19 { get; set; }

        [Parameter("05:00 - 05:14", DefaultValue = true)]
        public bool timeslot_20 { get; set; }

        [Parameter("05:15 - 05:29", DefaultValue = true)]
        public bool timeslot_21 { get; set; }

        [Parameter("05:30 - 05:44", DefaultValue = true)]
        public bool timeslot_22 { get; set; }

        [Parameter("05:45 - 05:59", DefaultValue = true)]
        public bool timeslot_23 { get; set; }

        [Parameter("06:00 - 06:14", DefaultValue = true)]
        public bool timeslot_24 { get; set; }

        [Parameter("06:15 - 06:29", DefaultValue = true)]
        public bool timeslot_25 { get; set; }

        [Parameter("06:30 - 06:44", DefaultValue = true)]
        public bool timeslot_26 { get; set; }

        [Parameter("06:45 - 06:59", DefaultValue = true)]
        public bool timeslot_27 { get; set; }

        [Parameter("07:00 - 07:14", DefaultValue = true)]
        public bool timeslot_28 { get; set; }

        [Parameter("07:15 - 07:29", DefaultValue = true)]
        public bool timeslot_29 { get; set; }

        [Parameter("07:30 - 07:44", DefaultValue = true)]
        public bool timeslot_30 { get; set; }

        [Parameter("07:45 - 07:59", DefaultValue = true)]
        public bool timeslot_31 { get; set; }

        [Parameter("08:00 - 08:14", DefaultValue = true)]
        public bool timeslot_32 { get; set; }

        [Parameter("08:15 - 08:29", DefaultValue = true)]
        public bool timeslot_33 { get; set; }

        [Parameter("08:30 - 08:44", DefaultValue = true)]
        public bool timeslot_34 { get; set; }

        [Parameter("08:45 - 08:59", DefaultValue = true)]
        public bool timeslot_35 { get; set; }

        [Parameter("09:00 - 09:14", DefaultValue = true)]
        public bool timeslot_36 { get; set; }

        [Parameter("09:15 - 09:29", DefaultValue = true)]
        public bool timeslot_37 { get; set; }

        [Parameter("09:30 - 09:44", DefaultValue = true)]
        public bool timeslot_38 { get; set; }

        [Parameter("09:45 - 09:59", DefaultValue = true)]
        public bool timeslot_39 { get; set; }

        [Parameter("10:00 - 10:14", DefaultValue = true)]
        public bool timeslot_40 { get; set; }

        [Parameter("10:15 - 10:29", DefaultValue = true)]
        public bool timeslot_41 { get; set; }

        [Parameter("10:30 - 10:44", DefaultValue = true)]
        public bool timeslot_42 { get; set; }

        [Parameter("10:45 - 10:59", DefaultValue = true)]
        public bool timeslot_43 { get; set; }

        [Parameter("11:00 - 11:14", DefaultValue = true)]
        public bool timeslot_44 { get; set; }

        [Parameter("11:15 - 11:29", DefaultValue = true)]
        public bool timeslot_45 { get; set; }

        [Parameter("11:30 - 11:44", DefaultValue = true)]
        public bool timeslot_46 { get; set; }

        [Parameter("11:45 - 11:59", DefaultValue = true)]
        public bool timeslot_47 { get; set; }

        [Parameter("12:00 - 12:14", DefaultValue = true)]
        public bool timeslot_48 { get; set; }

        [Parameter("12:15 - 12:29", DefaultValue = true)]
        public bool timeslot_49 { get; set; }

        [Parameter("12:30 - 12:44", DefaultValue = true)]
        public bool timeslot_50 { get; set; }

        [Parameter("12:45 - 12:59", DefaultValue = true)]
        public bool timeslot_51 { get; set; }

        [Parameter("13:00 - 13:14", DefaultValue = true)]
        public bool timeslot_52 { get; set; }

        [Parameter("13:15 - 13:29", DefaultValue = true)]
        public bool timeslot_53 { get; set; }

        [Parameter("13:30 - 13:44", DefaultValue = true)]
        public bool timeslot_54 { get; set; }

        [Parameter("13:45 - 13:59", DefaultValue = true)]
        public bool timeslot_55 { get; set; }

        [Parameter("14:00 - 14:14", DefaultValue = true)]
        public bool timeslot_56 { get; set; }

        [Parameter("14:15 - 14:29", DefaultValue = true)]
        public bool timeslot_57 { get; set; }

        [Parameter("14:30 - 14:44", DefaultValue = true)]
        public bool timeslot_58 { get; set; }

        [Parameter("14:45 - 14:59", DefaultValue = true)]
        public bool timeslot_59 { get; set; }

        [Parameter("15:00 - 15:14", DefaultValue = true)]
        public bool timeslot_60 { get; set; }

        [Parameter("15:15 - 15:29", DefaultValue = true)]
        public bool timeslot_61 { get; set; }

        [Parameter("15:30 - 15:44", DefaultValue = true)]
        public bool timeslot_62 { get; set; }

        [Parameter("15:45 - 15:59", DefaultValue = true)]
        public bool timeslot_63 { get; set; }

        [Parameter("16:00 - 16:14", DefaultValue = true)]
        public bool timeslot_64 { get; set; }

        [Parameter("16:15 - 16:29", DefaultValue = true)]
        public bool timeslot_65 { get; set; }

        [Parameter("16:30 - 16:44", DefaultValue = true)]
        public bool timeslot_66 { get; set; }

        [Parameter("16:45 - 16:59", DefaultValue = true)]
        public bool timeslot_67 { get; set; }

        [Parameter("17:00 - 17:14", DefaultValue = true)]
        public bool timeslot_68 { get; set; }

        [Parameter("17:15 - 17:29", DefaultValue = true)]
        public bool timeslot_69 { get; set; }

        [Parameter("17:30 - 17:44", DefaultValue = true)]
        public bool timeslot_70 { get; set; }

        [Parameter("17:45 - 17:59", DefaultValue = true)]
        public bool timeslot_71 { get; set; }

        [Parameter("18:00 - 18:14", DefaultValue = true)]
        public bool timeslot_72 { get; set; }

        [Parameter("18:15 - 18:29", DefaultValue = true)]
        public bool timeslot_73 { get; set; }

        [Parameter("18:30 - 18:44", DefaultValue = true)]
        public bool timeslot_74 { get; set; }

        [Parameter("18:45 - 18:59", DefaultValue = true)]
        public bool timeslot_75 { get; set; }

        [Parameter("19:00 - 19:14", DefaultValue = true)]
        public bool timeslot_76 { get; set; }

        [Parameter("19:15 - 19:29", DefaultValue = true)]
        public bool timeslot_77 { get; set; }

        [Parameter("19:30 - 19:44", DefaultValue = true)]
        public bool timeslot_78 { get; set; }

        [Parameter("19:45 - 19:59", DefaultValue = true)]
        public bool timeslot_79 { get; set; }

        [Parameter("20:00 - 20:14", DefaultValue = true)]
        public bool timeslot_80 { get; set; }

        [Parameter("20:15 - 20:29", DefaultValue = true)]
        public bool timeslot_81 { get; set; }

        [Parameter("20:30 - 20:44", DefaultValue = true)]
        public bool timeslot_82 { get; set; }

        [Parameter("20:45 - 20:59", DefaultValue = true)]
        public bool timeslot_83 { get; set; }

        [Parameter("21:00 - 21:14", DefaultValue = true)]
        public bool timeslot_84 { get; set; }

        [Parameter("21:15 - 21:29", DefaultValue = true)]
        public bool timeslot_85 { get; set; }

        [Parameter("21:30 - 21:44", DefaultValue = true)]
        public bool timeslot_86 { get; set; }

        [Parameter("21:45 - 21:59", DefaultValue = true)]
        public bool timeslot_87 { get; set; }

        [Parameter("22:00 - 22:14", DefaultValue = true)]
        public bool timeslot_88 { get; set; }

        [Parameter("22:15 - 22:29", DefaultValue = true)]
        public bool timeslot_89 { get; set; }

        [Parameter("22:30 - 22:44", DefaultValue = true)]
        public bool timeslot_90 { get; set; }

        [Parameter("22:45 - 22:59", DefaultValue = true)]
        public bool timeslot_91 { get; set; }

        [Parameter("23:00 - 23:14", DefaultValue = true)]
        public bool timeslot_92 { get; set; }

        [Parameter("23:15 - 23:29", DefaultValue = true)]
        public bool timeslot_93 { get; set; }

        [Parameter("23:30 - 23:44", DefaultValue = true)]
        public bool timeslot_94 { get; set; }

        [Parameter("23:45 - 23:59", DefaultValue = true)]
        public bool timeslot_95 { get; set; }
        //

        public static List<string> candleslist = new List<string>();
        public static List<string> outlist = new List<string>();
        public static List<int> blackslots = new List<int>();

        protected override void OnStart()
        {
            // Put your initialization logic here
            Print("kTrade 1.5.1 started");
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
            if (!timeslot_48)
            {
                blackslots.Add(48);
            }
            if (!timeslot_49)
            {
                blackslots.Add(49);
            }
            if (!timeslot_50)
            {
                blackslots.Add(50);
            }
            if (!timeslot_51)
            {
                blackslots.Add(51);
            }
            if (!timeslot_52)
            {
                blackslots.Add(52);
            }
            if (!timeslot_53)
            {
                blackslots.Add(53);
            }
            if (!timeslot_54)
            {
                blackslots.Add(54);
            }
            if (!timeslot_55)
            {
                blackslots.Add(55);
            }
            if (!timeslot_56)
            {
                blackslots.Add(56);
            }
            if (!timeslot_57)
            {
                blackslots.Add(57);
            }
            if (!timeslot_58)
            {
                blackslots.Add(58);
            }
            if (!timeslot_59)
            {
                blackslots.Add(59);
            }
            if (!timeslot_60)
            {
                blackslots.Add(60);
            }
            if (!timeslot_61)
            {
                blackslots.Add(61);
            }
            if (!timeslot_62)
            {
                blackslots.Add(62);
            }
            if (!timeslot_63)
            {
                blackslots.Add(63);
            }
            if (!timeslot_64)
            {
                blackslots.Add(64);
            }
            if (!timeslot_65)
            {
                blackslots.Add(65);
            }
            if (!timeslot_66)
            {
                blackslots.Add(66);
            }
            if (!timeslot_67)
            {
                blackslots.Add(67);
            }
            if (!timeslot_68)
            {
                blackslots.Add(68);
            }
            if (!timeslot_69)
            {
                blackslots.Add(69);
            }
            if (!timeslot_70)
            {
                blackslots.Add(70);
            }
            if (!timeslot_71)
            {
                blackslots.Add(71);
            }
            if (!timeslot_72)
            {
                blackslots.Add(72);
            }
            if (!timeslot_73)
            {
                blackslots.Add(73);
            }
            if (!timeslot_74)
            {
                blackslots.Add(74);
            }
            if (!timeslot_75)
            {
                blackslots.Add(75);
            }
            if (!timeslot_76)
            {
                blackslots.Add(76);
            }
            if (!timeslot_77)
            {
                blackslots.Add(77);
            }
            if (!timeslot_78)
            {
                blackslots.Add(78);
            }
            if (!timeslot_79)
            {
                blackslots.Add(79);
            }
            if (!timeslot_80)
            {
                blackslots.Add(80);
            }
            if (!timeslot_81)
            {
                blackslots.Add(81);
            }
            if (!timeslot_82)
            {
                blackslots.Add(82);
            }
            if (!timeslot_83)
            {
                blackslots.Add(83);
            }
            if (!timeslot_84)
            {
                blackslots.Add(84);
            }
            if (!timeslot_85)
            {
                blackslots.Add(85);
            }
            if (!timeslot_86)
            {
                blackslots.Add(86);
            }
            if (!timeslot_87)
            {
                blackslots.Add(87);
            }
            if (!timeslot_88)
            {
                blackslots.Add(88);
            }
            if (!timeslot_89)
            {
                blackslots.Add(89);
            }
            if (!timeslot_90)
            {
                blackslots.Add(90);
            }
            if (!timeslot_91)
            {
                blackslots.Add(91);
            }
            if (!timeslot_92)
            {
                blackslots.Add(92);
            }
            if (!timeslot_93)
            {
                blackslots.Add(93);
            }
            if (!timeslot_94)
            {
                blackslots.Add(94);
            }
            if (!timeslot_95)
            {
                blackslots.Add(95);
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

            if (currentminute >= 0 && currentminute < 15)
            {
                currentslotnumber = (currenthour * 4) + 0;
            }
            else if (currentminute >= 15 && currentminute < 30)
            {
                currentslotnumber = (currenthour * 4) + 1;
            }
            else if (currentminute >= 30 && currentminute < 45)
            {
                currentslotnumber = (currenthour * 4) + 2;
            }
            else if (currentminute >= 45 && currentminute <= 59)
            {
                currentslotnumber = (currenthour * 4) + 3;
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
