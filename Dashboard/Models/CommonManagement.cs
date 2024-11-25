using Dashboard.Database;
using System;

namespace Dashboard.Models
{
    public class CommonManagement
    {
        Executer _serv = new Executer();
        public string GetTransformerConsumptions(string Id, string fd, string td, string lol, string strTrans)
        {
            string ret = string.Empty;
            List<string> consumptions = new List<string>();

            string qry2 = string.Empty;


            qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER AS EM JOIN METERMASTER AS MM ON MM.METERID=EM.METERID WHERE  MM.TRANSFORMER='" + strTrans + "' AND CONVERT(DATETIME, SYNCDATETIME,103) " +
            "BETWEEN CONVERT(DATETIME, '" + fd + "',103) AND  CONVERT(DATETIME, '" + td + "',103)";


            var dt = _serv.GetDataTable(qry2);
            if (dt.Rows.Count > 0)
            {
                int j = 0;
                // Adding data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    j = i + 1;
                    if (j < dt.Rows.Count)
                    {
                        decimal val3 = Convert.ToDecimal(dt.Rows[i]["kwh"].ToString()) - Convert.ToDecimal(dt.Rows[j]["kwh"].ToString());
                        consumptions.Add(val3.ToString(".00"));
                    }
                }
            }
            if (consumptions.Count > 0)
            {
                decimal count = 0;
                foreach (var item in consumptions)
                {
                    count = count + Convert.ToDecimal(item);
                }
                ret = count.ToString(".00").Replace('-', ' ').Trim();
            }
            return ret;
        }


        public string GetVFConsumptions(string Id, string fd, string td, string lol, string strMeterDiv)
        {
            string ret = string.Empty;
            List<string> consumptions = new List<string>();
            string lols = string.Empty;
            if (lol == "G")
            {
                lols = "GROUPID = '" + Id + "'";
            }
            else if (lol == "M")
            {
                lols = "METERID = '" + Id + "'";
            }
            string qry2 = string.Empty;

            if (strMeterDiv != "")
            {
                //qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER AS EM LEFT JOIN METERMASTER AS MM ON MM.GROUPID=EM.GROUPID WHERE EM." + lols + " AND MM.METERDIVISION='" + strMeterDiv + "' AND CONVERT(datetime, SYNCDATETIME,103) " +
                //"BETWEEN CONVERT(datetime, '" + fd + "',103) AND  CONVERT(datetime, '" + td + "',103)";
                qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE " + lols + " AND METERID IN (SELECT METERID FROM METERMASTER WHERE " + lols + " AND METERDIVISION='" + strMeterDiv + "' AND FLAG=1)" +
                    "  AND CONVERT(DATETIME, SYNCDATETIME,103) BETWEEN CONVERT(DATETIME, '" + fd + "',103) AND  CONVERT(DATETIME, '" + td + "',103)";
            }
            else
            {
                qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE " + lols + " AND CONVERT(datetime, SYNCDATETIME,103) " +
                "BETWEEN CONVERT(datetime, '" + fd + "',103) AND  CONVERT(datetime, '" + td + "',103)";
            }

            var dt = _serv.GetDataTable(qry2);
            if (dt.Rows.Count > 0)
            {
                int j = 0;
                // Adding data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    j = i + 1;
                    if (j < dt.Rows.Count)
                    {
                        decimal val3 = Convert.ToDecimal(dt.Rows[i]["kwh"].ToString()) - Convert.ToDecimal(dt.Rows[j]["kwh"].ToString());
                        consumptions.Add(val3.ToString(".00"));
                    }
                }
            }
            if (consumptions.Count > 0)
            {
                decimal count = 0;
                foreach (var item in consumptions)
                {
                    count = count + Convert.ToDecimal(item);
                }
                ret = count.ToString(".00").Replace('-', ' ').Trim();
            }
            return ret;
        }

        public string GetConsumptions(string Id, string fd, string td, string lol)
        {
            string ret = string.Empty;
            List<string> consumptions = new List<string>();
            string lols = string.Empty;
            if (lol == "G")
            {
                lols = "GROUPID = '" + Id + "'";
            }
            else if (lol == "M")
            {
                lols = "METERID = '" + Id + "'";
            }

            //CONVERT(DATETIME, '" + td + "', 121)

            string qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE " + lols + " AND CONVERT(DATETIME, SYNCDATETIME,103) " +
                "BETWEEN CONVERT(DATETIME, '" + fd + "',103) AND  CONVERT(DATETIME, '" + td + "',103)";

            //string qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE METERID = 'M10'"+
            //          "AND CONVERT(DATETIME, SYNCDATETIME,103) BETWEEN CONVERT(DATETIME, '2024-07-10 13:06:04.000',121) "+
            //                            "AND CONVERT(DATETIME, '2024-07-10 13:06:04.000',121)";
            var dt = _serv.GetDataTable(qry2);
            if (dt.Rows.Count > 0)
            {
                int j = dt.Rows.Count - 1;
                int i = 0;
                decimal val3 = Convert.ToDecimal(dt.Rows[j]["kwh"].ToString()) - Convert.ToDecimal(dt.Rows[i]["kwh"].ToString());
                consumptions.Add(val3.ToString(".00"));


                //int j = 0;
                //// Adding data
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    j = i + 1;
                //    if (j < dt.Rows.Count)
                //    {
                //        decimal val3 = Convert.ToDecimal(dt.Rows[i]["kwh"].ToString()) - Convert.ToDecimal(dt.Rows[j]["kwh"].ToString());
                //        consumptions.Add(val3.ToString(".00"));
                //    }
                //}
            }
            if (consumptions.Count > 0)
            {
                decimal count = 0;
                foreach (var item in consumptions)
                {
                    count = count + Convert.ToDecimal(item);
                }
                ret = count.ToString(".00").Replace('-', ' ').Trim();
            }
            return ret;
        }


        // -- Commented By Pradeep On Oct-24
        public string GetConsumptionss(string Id, string fd, string td, string lol)
        {
            string ret = string.Empty;
            List<string> consumptions = new List<string>();
            string lols = string.Empty;
            if (lol == "G")
            {
                lols = "GROUPID = '" + Id + "'";
            }
            else if (lol == "M")
            {
                lols = "METERID = '" + Id + "'";
            }

            //CONVERT(DATETIME, '" + td + "', 121)

            string qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE " + lols + " AND CONVERT(DATETIME, SYNCDATETIME,103) " +
                "BETWEEN CONVERT(DATETIME, '" + fd + "',103) AND  CONVERT(DATETIME, '" + td + "',103)";

            //string qry2 = "SELECT ACTIVEENERGYDELIVERED AS kwh FROM TBL_ENERGYMETER WHERE METERID = 'M10'"+
            //          "AND CONVERT(DATETIME, SYNCDATETIME,103) BETWEEN CONVERT(DATETIME, '2024-07-10 13:06:04.000',121) "+
            //                            "AND CONVERT(DATETIME, '2024-07-10 13:06:04.000',121)";
            var dt = _serv.GetDataTable(qry2);
            if (dt.Rows.Count > 0)
            {
                int j = 0;
                // Adding data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    j = i + 1;
                    if (j < dt.Rows.Count)
                    {
                        decimal val3 = Convert.ToDecimal(dt.Rows[i]["kwh"].ToString()) - Convert.ToDecimal(dt.Rows[j]["kwh"].ToString());
                        consumptions.Add(val3.ToString(".00"));
                    }
                }
            }
            if (consumptions.Count > 0)
            {
                decimal count = 0;
                foreach (var item in consumptions)
                {
                    count = count + Convert.ToDecimal(item);
                }
                ret = count.ToString(".00").Replace('-', ' ').Trim();
            }
            return ret;
        }
        /* */
    }
}
