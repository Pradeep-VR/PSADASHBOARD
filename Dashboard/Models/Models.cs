namespace Dashboard.Models
{
    public class GROUPS
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<METERMASTER> MMASTER { get; set; }
        public List<GROUPMASTER> GMaster { get; set; }
        public List<TBLENERGYMETER> EMETER { get; set; }

    }

    public class ChartData
    {
        public DateTime Date { get; set; }
        public double PowerFactor { get; set; }
        public double MaxDemand { get; set; }
        public double ActiveEnergy { get; set; }
    }

    public class METERMASTER
    {
        public string METERID { get; set; }
        public string METERNAME { get; set; }
        public Nullable<int> GROUPID { get; set; }
        public Nullable<bool> FLAG { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
    }

    public class GROUPMASTER
    {
        public int GROUPID { get; set; }
        public string GROUPNAME { get; set; }
        public Nullable<bool> FLAG { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
    }

    public class TBLENERGYMETER
    {
        public int ID { get; set; }
        public string STATIONS { get; set; }
        public Nullable<int> METERID { get; set; }
        public Nullable<int> GROUPID { get; set; }
        public string CURRENTA { get; set; }
        public string CURRENTB { get; set; }
        public string CURRENTC { get; set; }
        public string VOLTAGEAB { get; set; }
        public string VOLTAGEBC { get; set; }
        public string VOLTAGECA { get; set; }
        public string VOLTAGELL { get; set; }
        public string VOLTAGEAN { get; set; }
        public string VOLTAGEBN { get; set; }
        public string VOLTAGECN { get; set; }
        public string VOLTAGELN { get; set; }
        public string MAXDEMAND { get; set; }
        public string POWERFACTOR { get; set; }
        public string ACTIVEENERGYDELIVERED { get; set; }
        public string SYNCDATETIME { get; set; }
        public string YeasterdayConsume { get; set; }
        public string TodayConsume { get; set; }
        public string variable_TodayCon { get; set; }
        public string variable_YesterdayCon { get; set; }
        public string fixed_TodayCon { get; set; }
        public string fixed_YesterdayCon { get; set; }
        public List<string> MeterNames { get; set; }
        public List<string> GroupNames { get; set; }
    }

    public class EnergyMeterModel
    {
        public string[] MeterName { get; set; }
        public string[] DateAndTime { get; set; }
        public string[] Current_A { get; set; }
        public string[] Current_B { get; set; }
        public string[] Current_C { get; set; }
        public string[] Voltage_A { get; set; }
        public string[] Voltage_B { get; set; }
        public string[] Voltage_C { get; set; }
        public string[] PowerFactor { get; set; }
        public string[] MaxDemand { get; set; }
        public string[] ActiveEnergy { get; set; }
        public string[] Consumption { get; set; }
        public string[] syncdatetime { get; set; }
    }

    public class TestModel
    {
        public string[] PowerFactor { get; set; }
        public string[] MeterName { get; set; }
        public string[] SYNCDATETIME { get; set; }
        public string[] MaxDemand { get; set; }
        public string[] ActiveEnergy { get; set; }


        public string[] BODY_SHOP_ActiveEnergy { get; set; }
        public string[] GENERAL_ASSEMBLY_ActiveEnergy { get; set; }
        public string[] HOSTED_SERVICES_ActiveEnergy { get; set; }
        public string[] PAINT_SHOP_ActiveEnergy { get; set; }
        public string[] TRANSFORMER_WISE_ActiveEnergy { get; set; }
        public string[] UTILITIES_ActiveEnergy { get; set; }
    }
}
