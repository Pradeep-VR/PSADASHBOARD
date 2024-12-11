using Dashboard.Database;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        #region DefaultMethod
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        #endregion

        Executer _serv = new Executer();
        CommonManagement _mgnt = new CommonManagement();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShopWiseIndex(string groupId, string groupName)
        {
            var vals = groupId.Split(',');
            ViewBag.groupId = vals[0];
            ViewBag.groupName = groupName;
            ViewBag.yesCon = vals[1];
            ViewBag.toCon = vals[2];

            return View();
        }
        public IActionResult TransFormer(string groupId, string groupName)
        {
            var vals = groupId.Split(',');
            ViewBag.groupId = vals[0];
            ViewBag.groupName = groupName;
            //ViewBag.yesCon = vals[1];
            //ViewBag.toCon = vals[2];

            return View();
        }


        public IActionResult OverAllIndex()
        {
            return View();
        }



        [HttpPost]
        public JsonResult GetGroupsWithMeters()
        {
            var groups = new List<GROUPS>();

            string query = "SELECT GROUPID,GROUPNAME FROM  GroupMaster WHERE  FLAG = '1' ORDER BY GROUPORDER ASC;";
            DataTable dt = _serv.GetDataTable(query);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var groupId = dt.Rows[i]["GROUPID"].ToString();
                    var groupName = dt.Rows[i]["GROUPNAME"].ToString();

                    var group = new Models.GROUPS
                    {
                        GroupId = groupId,
                        GroupName = groupName,
                        MMASTER = new List<METERMASTER>()
                    };
                    groups.Add(group);
                }
            }
            return Json(groups);
        }
        /* Home Index */

        [HttpPost]
        public JsonResult GetMetersName(string Groupid)
        {
            var group = new GROUPS();
            string mtrnameqry = "";
            if (Groupid == "GTW")
            {
                mtrnameqry = "SELECT METERID,METERNAME FROM METERMASTER WHERE FLAG=1";
            }
            else
            {
                mtrnameqry = "SELECT METERID,METERNAME FROM METERMASTER WHERE GROUPID='" + Groupid + "' AND FLAG=1";
            }

            DataTable dt = _serv.GetDataTable(mtrnameqry);
            if (dt.Rows.Count > 0)
            {
                var meters = new List<METERMASTER>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    meters.Add(new METERMASTER
                    {
                        METERID = dt.Rows[i]["METERID"].ToString(),
                        METERNAME = dt.Rows[i]["METERNAME"].ToString(),
                    });
                    group.MMASTER = meters;
                }
            }

            return Json(group);
        }


        [HttpPost]
        public JsonResult GetMetersDtls(string strGroupId, bool strCon)
        {
            var group = new GROUPS();
            string BqCon = string.Empty;
            if (strCon == true)
            {
                BqCon = "GROUPID='" + strGroupId + "' AND";
            }
            else
            {
                BqCon = "";
            }

            string mtrnameqry = "SELECT METERID,METERNAME FROM METERMASTER WHERE " + BqCon + " FLAG=1";
            DataTable dt = _serv.GetDataTable(mtrnameqry);
            if (dt.Rows.Count > 0)
            {
                var meters = new List<METERMASTER>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    meters.Add(new METERMASTER
                    {
                        METERID = dt.Rows[i]["METERID"].ToString(),
                        METERNAME = dt.Rows[i]["METERNAME"].ToString(),
                    });
                    group.MMASTER = meters;
                }
            }

            return Json(group);
        }

        [HttpPost]
        public JsonResult GetTsWData(string strTransformer)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            string formYesDay = yesterday.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toYesDay = yesterday.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            DateTime today = DateTime.Today;
            string fromToday = today.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toToday = today.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            decimal strYesCon = 0;
            decimal strToCon = 0;

            var YesDayCon = _mgnt.GetTransformerConsumptions("GTW", formYesDay, toYesDay, "G", strTransformer);
            var ToDayCon = _mgnt.GetTransformerConsumptions("GTW", fromToday, toToday, "G", strTransformer);

            //if (YesDayCon != null && YesDayCon != "")
            if (!string.IsNullOrEmpty(YesDayCon))
            {
                strYesCon = Convert.ToDecimal(strYesCon) + Convert.ToDecimal(YesDayCon);
            }

            //if (ToDayCon != null && ToDayCon != "")
            if (!string.IsNullOrEmpty(ToDayCon))
            {
                strToCon = Convert.ToDecimal(strToCon) + Convert.ToDecimal(ToDayCon);
            }

            var data = new
            {
                YesdayCon = strYesCon.ToString(".00"),
                TodayCon = strToCon.ToString(".00")
            };

            return Json(data);
        }


        [HttpPost]
        public JsonResult GetEnergyData_ReWork(string groupId)
        {
            TBLENERGYMETER data = new TBLENERGYMETER();
            DateTime yesterday = DateTime.Today.AddDays(-1);
            string formYesDay = yesterday.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toYesDay = yesterday.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            DateTime today = DateTime.Today;
            string fromToday = today.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toToday = today.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            decimal YesDayCon = 0;
            decimal ToDayCon = 0;
            decimal Var_YesdayCon = 0; decimal Var_TodayCon = 0; decimal Fix_YesdayCon = 0; decimal Fix_TodayCon = 0;

            string Qry = "SELECT METERID FROM METERMASTER WHERE GROUPID='" + groupId + "' AND FLAG=1";
            var Meterdata = _serv.GetDataTable(Qry);
            if (Meterdata.Rows.Count > 0)
            {
                List<decimal> lst1 = new List<decimal>();
                List<decimal> lst2 = new List<decimal>();
                List<decimal> lstF1 = new List<decimal>();
                List<decimal> lstF2 = new List<decimal>();
                List<decimal> lstV1 = new List<decimal>();
                List<decimal> lstV2 = new List<decimal>();

                for (int i = 0; i < Meterdata.Rows.Count; i++)
                {
                    string meterId = Meterdata.Rows[i]["METERID"].ToString();

                    var Yescon = _mgnt.GetConsumptions(meterId, formYesDay, toYesDay, "M");
                    var Tocon = _mgnt.GetConsumptions(meterId, fromToday, toToday, "M");
                    if (Yescon != null && Yescon != "")
                    {
                        lst1.Add(Convert.ToDecimal(Yescon));
                    }

                    if (Tocon != null && Tocon != "")
                    {
                        lst2.Add(Convert.ToDecimal(Tocon));
                    }

                    var Fix_YesCon = _mgnt.GetVFConsumptions(meterId, formYesDay, toYesDay, "M", "F");
                    if (Fix_YesCon != null && Fix_YesCon != "")
                    {
                        lstF1.Add(Convert.ToDecimal(Fix_YesCon));
                        //Fix_YesdayCon = Convert.ToDecimal(Fix_YesCon);
                    }
                    var Fix_ToCon = _mgnt.GetVFConsumptions(meterId, fromToday, toToday, "M", "F");
                    if (Fix_ToCon != null && Fix_ToCon != "")
                    {
                        lstF2.Add(Convert.ToDecimal(Fix_ToCon));
                        //Fix_TodayCon = Convert.ToDecimal(Fix_ToCon);
                    }

                    var Var_YesCon = _mgnt.GetVFConsumptions(meterId, formYesDay, toYesDay, "M", "V");
                    if (Var_YesCon != null && Var_YesCon != "")
                    {
                        lstV1.Add(Convert.ToDecimal(Var_YesCon));
                        //Var_YesdayCon = Convert.ToDecimal(Var_YesCon);
                    }
                    var Var_ToCon = _mgnt.GetVFConsumptions(meterId, fromToday, toToday, "M", "V");
                    if (Var_ToCon != null && Var_ToCon != "")
                    {
                        lstV2.Add(Convert.ToDecimal(Var_ToCon));
                        //Var_TodayCon = Convert.ToDecimal(Var_ToCon);
                    }
                }

                foreach (var item in lst1)
                {
                    YesDayCon += item;
                }

                foreach (var item in lst2)
                {
                    ToDayCon += item;
                }

                foreach (var item in lstF1)
                {
                    Fix_YesdayCon += item;
                }

                foreach (var item in lstF2)
                {
                    Fix_TodayCon += item;
                }

                foreach (var item in lstV1)
                {
                    Var_YesdayCon += item;
                }

                foreach (var item in lstV2)
                {
                    Var_TodayCon += item;
                }
            }

            data.YeasterdayConsume = YesDayCon.ToString(".00");
            data.TodayConsume = ToDayCon.ToString(".00");

            data.variable_YesterdayCon = Var_YesdayCon.ToString(".00");
            data.variable_TodayCon = Var_TodayCon.ToString(".00");

            data.fixed_YesterdayCon = Fix_YesdayCon.ToString(".00");
            data.fixed_TodayCon = Fix_TodayCon.ToString(".00");

            return Json(data);
        }


        [HttpPost]
        public JsonResult GetEnergyData([FromBody] string groupId)
        {
            TBLENERGYMETER data = new TBLENERGYMETER();
            DateTime yesterday = DateTime.Today.AddDays(-1);
            string formYesDay = yesterday.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toYesDay = yesterday.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            DateTime today = DateTime.Today;
            string fromToday = today.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toToday = today.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            decimal YesDayCon = 0;
            decimal ToDayCon = 0;
            decimal Var_YesdayCon = 0; decimal Var_TodayCon = 0; decimal Fix_YesdayCon = 0; decimal Fix_TodayCon = 0;

            var Yescon = _mgnt.GetConsumptions(groupId, formYesDay, toYesDay, "G");
            var Tocon = _mgnt.GetConsumptions(groupId, fromToday, toToday, "G");
            if (Yescon != null && Yescon != "")
            {
                YesDayCon = Convert.ToDecimal(YesDayCon) + Convert.ToDecimal(Yescon);
            }

            if (Tocon != null && Tocon != "")
            {
                ToDayCon = Convert.ToDecimal(ToDayCon) + Convert.ToDecimal(Tocon);
            }

            var Fix_YesCon = _mgnt.GetVFConsumptions(groupId, formYesDay, toYesDay, "G", "F");
            if (Fix_YesCon != null && Fix_YesCon != "")
            {
                Fix_YesdayCon = Convert.ToDecimal(Fix_YesCon);
            }
            var Fix_ToCon = _mgnt.GetVFConsumptions(groupId, fromToday, toToday, "G", "F");
            if (Fix_ToCon != null && Fix_ToCon != "")
            {
                Fix_TodayCon = Convert.ToDecimal(Fix_ToCon);
            }

            var Var_YesCon = _mgnt.GetVFConsumptions(groupId, formYesDay, toYesDay, "G", "V");
            if (Var_YesCon != null && Var_YesCon != "")
            {
                Var_YesdayCon = Convert.ToDecimal(Var_YesCon);
            }
            var Var_ToCon = _mgnt.GetVFConsumptions(groupId, fromToday, toToday, "G", "V");
            if (Var_ToCon != null && Var_ToCon != "")
            {
                Var_TodayCon = Convert.ToDecimal(Var_ToCon);
            }


            data.YeasterdayConsume = YesDayCon.ToString(".00");
            data.TodayConsume = ToDayCon.ToString(".00");

            data.variable_YesterdayCon = Var_YesdayCon.ToString(".00");
            data.variable_TodayCon = Var_TodayCon.ToString(".00");

            data.fixed_YesterdayCon = Fix_YesdayCon.ToString(".00");
            data.fixed_TodayCon = Fix_TodayCon.ToString(".00");

            string qry2 = "";
            var dt2 = _serv.GetDataTable(qry2);
            if (dt2.Rows.Count > 0)
            {

            }

            string qry6 = " SELECT  GROUPNAME,GROUPID FROM GROUPMASTER   WHERE FLAG='1'";
            DataTable dt6 = _serv.GetDataTable(qry6);
            data.GroupNames = new List<string>();
            foreach (DataRow row in dt6.Rows)
            {
                string formattedGroupName = $"{row["GROUPNAME"]} - {row["GROUPID"]}";
                data.GroupNames.Add(formattedGroupName);
            }

            return Json(data);
        }


        [HttpPost]
        public JsonResult StationsData(string groupId, string groupName)
        {
            List<object> dataList = new List<object>();
            DateTime today = DateTime.Today;
            string fromToday = today.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toToday = today.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            string qry = " WITH HourlyData AS (SELECT DATEPART(HOUR, SYNCDATETIME) AS Hour,SUM(CONVERT(float, MAXDEMAND)) AS MAXDEMAND,  " +
                         " SUM(CONVERT(float, POWERFACTOR)) AS POWERFACTOR,SUM(CONVERT(float, ACTIVEENERGYDELIVERED)) AS ACTIVEENERGYDELIVERED FROM TBL_ENERGYMETER   " +
                         " WHERE GROUPID = '" + groupId + "' AND CONVERT(datetime, SYNCDATETIME,103) >= CONVERT(datetime, '" + fromToday + "',103)   AND CONVERT(datetime, SYNCDATETIME,103)  " +
                         " <= CONVERT(datetime, '" + toToday + "',103)  " +
                         " GROUP BY  DATEPART(HOUR, SYNCDATETIME)),HourlyDifferences AS (SELECT h1.Hour,h1.MAXDEMAND,h1.POWERFACTOR, h1.ACTIVEENERGYDELIVERED, " +
                         " (h1.MAXDEMAND - ISNULL(h2.MAXDEMAND, 0)) AS MAXDEMAND_DIFF,(h1.POWERFACTOR - ISNULL(h2.POWERFACTOR, 0)) AS POWERFACTOR_DIFF, " +
                         " (h1.ACTIVEENERGYDELIVERED - ISNULL(h2.ACTIVEENERGYDELIVERED, 0)) AS ACTIVEENERGYDELIVERED_DIFF FROM HourlyData h1  LEFT JOIN  HourlyData h2 ON h1.Hour = h2.Hour + 1)  " +
                         " SELECT  Hour, MAXDEMAND, POWERFACTOR, ACTIVEENERGYDELIVERED, MAXDEMAND_DIFF,  POWERFACTOR_DIFF, ACTIVEENERGYDELIVERED_DIFF FROM  HourlyDifferences ORDER BY  Hour; ";

            DataTable dt = _serv.GetDataTable(qry);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var maxDemandDiff = Convert.ToDouble(row["MAXDEMAND_DIFF"]);
                    var powerFactorDiff = Convert.ToDouble(row["POWERFACTOR_DIFF"]);
                    var activeEnergyDeliveredDiff = Convert.ToDouble(row["ACTIVEENERGYDELIVERED_DIFF"]);

                    // Ensure negative values are converted to positive
                    if (maxDemandDiff < 0)
                    {
                        maxDemandDiff *= -1;
                    }

                    if (powerFactorDiff < 0)
                    {
                        powerFactorDiff *= -1;
                    }

                    if (activeEnergyDeliveredDiff < 0)
                    {
                        activeEnergyDeliveredDiff *= -1;
                    }

                    var data = new
                    {
                        Hour = row["Hour"].ToString(),
                        MAXDEMAND = row["MAXDEMAND"].ToString(),
                        POWERFACTOR = row["POWERFACTOR"].ToString(),
                        ACTIVEENERGYDELIVERED = row["ACTIVEENERGYDELIVERED"].ToString(),
                        MAXDEMAND_DIFF = maxDemandDiff.ToString(), // Converted to positive
                        POWERFACTOR_DIFF = powerFactorDiff.ToString(), // Converted to positive
                        ACTIVEENERGYDELIVERED_DIFF = activeEnergyDeliveredDiff.ToString() // Converted to positive
                    };
                    dataList.Add(data);
                }
            }

            return Json(dataList);
        }

        [HttpPost]
        public JsonResult GetMeters(string groupId, string strMeterDiv)
        {
            string query = String.Empty;
            var group = new GROUPS();
            if (groupId == "GTW")
            {
                query = "SELECT GM.GROUPID,GM.GROUPNAME,MM.METERID,MM.METERNAME,MM.METERDIVISION FROM GROUPMASTER GM LEFT JOIN  METERMASTER MM ON GM.GROUPID = MM.GROUPID " +
              "WHERE GM.FLAG = '1' AND MM.FLAG='1'  ORDER BY MM.TRANSFORMER DESC ";
            }
            else
            {
                query = "SELECT GM.GROUPID,GM.GROUPNAME,MM.METERID,MM.METERNAME,MM.METERDIVISION FROM GROUPMASTER GM LEFT JOIN  METERMASTER MM ON GM.GROUPID = MM.GROUPID " +
               "WHERE GM.FLAG = '1' AND MM.FLAG='1' AND GM.GROUPID = '" + groupId + "' AND MM.METERDIVISION IN (" + strMeterDiv + ")";
            }

            //string query = "SELECT gm.GroupId,gm.GroupName,mm.MeterId,mm.MeterName FROM GroupMaster gm LEFT JOIN  MeterMaster mm ON gm.GroupId = mm.GroupId WHERE gm.FLAG = '1' AND mm.FLAG='1' AND gm.GroupId = '" + groupId + "';";

            DataTable dt = _serv.GetDataTable(query);
            if (dt.Rows.Count > 0)
            {
                var meters = new List<METERMASTER>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (group.GroupId == null || group.GroupId.Length == 0)
                    {
                        group.GroupId = dt.Rows[i]["GROUPID"].ToString();
                        group.GroupName = dt.Rows[i]["GROUPNAME"].ToString();
                    }


                    meters.Add(new METERMASTER
                    {
                        METERID = dt.Rows[i]["MeterId"].ToString(),
                        METERNAME = dt.Rows[i]["MeterName"].ToString(),
                    });

                }
                group.MMASTER = meters;
            }

            return Json(group);
        }


        [HttpPost]
        public JsonResult GetEnergyDataUsingMeterid(string meterId)
        {
            TBLENERGYMETER data = null;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            string fromYesday = yesterday.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toYesDay = yesterday.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            DateTime today = DateTime.Today;
            string fromToday = today.ToString("dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture);
            string toToday = today.ToString("dd/MM/yyyy 23:59:59", CultureInfo.InvariantCulture);

            string Qry = "SELECT * FROM (SELECT CURRENTA AS CRY,CURRENTB AS CYB,CURRENTC AS CBR,VoltageAB AS VRY,VoltageBC AS VYB,VoltageCA AS VBR, " +
                "ACTIVEENERGYDELIVERED AS ADE , CONVERT(datetime, SYNCDATETIME,103) as Datetimes FROM TBL_ENERGYMETER WHERE METERID = '" + meterId + "' ) AS C " +
                "WHERE CONVERT(date,C.Datetimes,103) = CONVERT(date, '" + fromToday + "',103) ORDER BY C.Datetimes DESC";
            var dt = _serv.GetDataTable(Qry);

            decimal CA, CB, CC, VA, VB, VC, AED, YesCon, ToCon = 0;

            string YesConsum = _mgnt.GetConsumptions(meterId, fromYesday, toYesDay, "M");
            string ToConsum = _mgnt.GetConsumptions(meterId, fromToday, toToday, "M");
            YesCon = Convert.ToDecimal(YesConsum == "" ? "0" : YesConsum);
            ToCon = Convert.ToDecimal(ToConsum == "" ? "0" : ToConsum);
            CA = dt.Rows.Count > 0 && dt.Rows[0]["CRY"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["CRY"]) : 0;
            CB = dt.Rows.Count > 0 && dt.Rows[0]["CYB"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["CYB"]) : 0;
            CC = dt.Rows.Count > 0 && dt.Rows[0]["CBR"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["CBR"]) : 0;
            VA = dt.Rows.Count > 0 && dt.Rows[0]["VRY"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["VRY"]) : 0;
            VB = dt.Rows.Count > 0 && dt.Rows[0]["VYB"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["VYB"]) : 0;
            VC = dt.Rows.Count > 0 && dt.Rows[0]["VBR"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["VBR"]) : 0;
            AED = dt.Rows.Count > 0 && dt.Rows[0]["ADE"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["ADE"]) : 0;

            data = new TBLENERGYMETER
            {
                YeasterdayConsume = YesCon.ToString("0.00"),
                TodayConsume = ToCon.ToString("0.00"),
                CURRENTA = CA.ToString("0.00"),
                CURRENTB = CB.ToString("0.00"),
                CURRENTC = CC.ToString("0.00"),
                VOLTAGEAB = VA.ToString("0.00"),
                VOLTAGEBC = VB.ToString("0.00"),
                VOLTAGECA = VC.ToString("0.00"),
                ACTIVEENERGYDELIVERED = AED.ToString("0.00"),
            };
            return Json(data);
        }
    }
}
