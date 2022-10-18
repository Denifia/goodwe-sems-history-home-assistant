namespace Goodwe.Sems;

public class FullExtract
{
    public Dictionary<DateTimeOffset, Dictionary<string, double>> Import { get; set; } = new Dictionary<DateTimeOffset, Dictionary<string, double>>();
    public Dictionary<DateTimeOffset, Dictionary<string, double>> Export { get; set; } = new Dictionary<DateTimeOffset, Dictionary<string, double>>();
    public Dictionary<DateTimeOffset, Dictionary<string, double>> Load { get; set; } = new Dictionary<DateTimeOffset, Dictionary<string, double>>();
}

public class DayExtract
{
    public Dictionary<string, double> Import { get; set; } = new Dictionary<string, double>();
    public Dictionary<string, double> Export { get; set; } = new Dictionary<string, double>();
    public Dictionary<string, double> Load { get; set; } = new Dictionary<string, double>();
}

// Below is Auto-Generated from the Sems api response json
// todo - trim this down to only what we use

public class ChartData
{
    public class Axis
    {
        public int axisId { get; set; }
        public string unit { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Components
    {
        public string para { get; set; }
        public int langVer { get; set; }
        public int timeSpan { get; set; }
        public string api { get; set; }
        public object msgSocketAdr { get; set; }
    }

    public class Data
    {
        public List<LeftLabel> leftLabels { get; set; }
        public List<Line> lines { get; set; }
        public NestedGraph nestedGraph { get; set; }
        public List<Axis> axis { get; set; }
        public object modelData { get; set; }
        public int chartNum { get; set; }
    }

    public class LeftLabel
    {
        public string img { get; set; }
        public string label { get; set; }
    }

    public class Line
    {
        public string label { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public bool isActive { get; set; }
        public int axis { get; set; }
        public int sort { get; set; }
        public string type { get; set; }
        public string frontColor { get; set; }
        public List<Xy> xy { get; set; }
    }

    public class NestedGraph
    {
        public bool useNested { get; set; }
    }

    public class Root
    {
        public string language { get; set; }
        public object function { get; set; }
        public bool hasError { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
        public Data data { get; set; }
        public Components components { get; set; }
    }

    public class Xy
    {
        public string x { get; set; }
        public double y { get; set; }
        public object z { get; set; }
    }
}

public class MonitorData
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class D
    {
        public string pw_id { get; set; }
        public string capacity { get; set; }
        public string model { get; set; }
        public string output_power { get; set; }
        public string output_current { get; set; }
        public string grid_voltage { get; set; }
        public string backup_output { get; set; }
        public string soc { get; set; }
        public string soh { get; set; }
        public string last_refresh_time { get; set; }
        public string work_mode { get; set; }
        public string dc_input1 { get; set; }
        public string dc_input2 { get; set; }
        public string battery { get; set; }
        public string bms_status { get; set; }
        public string warning { get; set; }
        public string charge_current_limit { get; set; }
        public string discharge_current_limit { get; set; }
        public double firmware_version { get; set; }
        public string creationDate { get; set; }
        public double eDay { get; set; }
        public double eTotal { get; set; }
        public double pac { get; set; }
        public double hTotal { get; set; }
        public double vpv1 { get; set; }
        public double vpv2 { get; set; }
        public double vpv3 { get; set; }
        public double vpv4 { get; set; }
        public double ipv1 { get; set; }
        public double ipv2 { get; set; }
        public double ipv3 { get; set; }
        public double ipv4 { get; set; }
        public double vac1 { get; set; }
        public double vac2 { get; set; }
        public double vac3 { get; set; }
        public double iac1 { get; set; }
        public double iac2 { get; set; }
        public double iac3 { get; set; }
        public double fac1 { get; set; }
        public double fac2 { get; set; }
        public double fac3 { get; set; }
        public double istr1 { get; set; }
        public double istr2 { get; set; }
        public double istr3 { get; set; }
        public double istr4 { get; set; }
        public double istr5 { get; set; }
        public double istr6 { get; set; }
        public double istr7 { get; set; }
        public double istr8 { get; set; }
        public double istr9 { get; set; }
        public double istr10 { get; set; }
        public double istr11 { get; set; }
        public double istr12 { get; set; }
        public double istr13 { get; set; }
        public double istr14 { get; set; }
        public double istr15 { get; set; }
        public double istr16 { get; set; }
    }

    public class DailyForecast
    {
        public string cond_code_d { get; set; }
        public string cond_code_n { get; set; }
        public string cond_txt_d { get; set; }
        public string cond_txt_n { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public object hum { get; set; }
        public object pcpn { get; set; }
        public object pop { get; set; }
        public object pres { get; set; }
        public string tmp_max { get; set; }
        public string tmp_min { get; set; }
        public object uv_index { get; set; }
        public object vis { get; set; }
        public object wind_deg { get; set; }
        public object wind_dir { get; set; }
        public object wind_sc { get; set; }
        public object wind_spd { get; set; }
    }

    public class Dict
    {
        public List<Left> left { get; set; }
        public List<Right> right { get; set; }
    }

    public class EnergeStatisticsCharts
    {
        public double contributingRate { get; set; }
        public double selfUseRate { get; set; }
        public double sum { get; set; }
        public double buy { get; set; }
        public double buyPercent { get; set; }
        public double sell { get; set; }
        public double sellPercent { get; set; }
        public double selfUseOfPv { get; set; }
        public double consumptionOfLoad { get; set; }
        public int chartsType { get; set; }
        public bool hasPv { get; set; }
        public bool hasCharge { get; set; }
        public double charge { get; set; }
        public double disCharge { get; set; }
    }

    public class EnergeStatisticsTotals
    {
        public double contributingRate { get; set; }
        public double selfUseRate { get; set; }
        public double sum { get; set; }
        public double buy { get; set; }
        public double buyPercent { get; set; }
        public double sell { get; set; }
        public double sellPercent { get; set; }
        public double selfUseOfPv { get; set; }
        public double consumptionOfLoad { get; set; }
        public int chartsType { get; set; }
        public bool hasPv { get; set; }
        public bool hasCharge { get; set; }
        public double charge { get; set; }
        public double disCharge { get; set; }
    }

    public class Equipment
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public object statusText { get; set; }
        public object capacity { get; set; }
        public object actionThreshold { get; set; }
        public string subordinateEquipment { get; set; }
        public string powerGeneration { get; set; }
        public string eday { get; set; }
        public string brand { get; set; }
        public bool isStored { get; set; }
        public string soc { get; set; }
        public bool isChange { get; set; }
        public string relationId { get; set; }
        public string sn { get; set; }
        public bool has_tigo { get; set; }
        public bool is_sec { get; set; }
        public bool is_secs { get; set; }
        public object targetPF { get; set; }
        public object exportPowerlimit { get; set; }
    }

    public class HeWeather6
    {
        public List<DailyForecast> daily_forecast { get; set; }
        public object basic { get; set; }
        public object update { get; set; }
        public string status { get; set; }
    }

    public class Hjgx
    {
        public double co2 { get; set; }
        public double tree { get; set; }
        public double coal { get; set; }
    }

    public class HomKit
    {
        public bool homeKitLimit { get; set; }
        public string sn { get; set; }
    }

    public class Info
    {
        public string powerstation_id { get; set; }
        public string time { get; set; }
        public string date_format { get; set; }
        public string date_format_ym { get; set; }
        public string stationname { get; set; }
        public string address { get; set; }
        public object owner_name { get; set; }
        public object owner_phone { get; set; }
        public string owner_email { get; set; }
        public double battery_capacity { get; set; }
        public string turnon_time { get; set; }
        public string create_time { get; set; }
        public double capacity { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string powerstation_type { get; set; }
        public int status { get; set; }
        public bool is_stored { get; set; }
        public bool is_powerflow { get; set; }
        public int charts_type { get; set; }
        public bool has_pv { get; set; }
        public bool has_statistics_charts { get; set; }
        public bool only_bps { get; set; }
        public bool only_bpu { get; set; }
        public double time_span { get; set; }
        public string pr_value { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
    }

    public class Inverter
    {
        public string sn { get; set; }
        public Dict dict { get; set; }
        public bool is_stored { get; set; }
        public string name { get; set; }
        public double in_pac { get; set; }
        public double out_pac { get; set; }
        public double eday { get; set; }
        public double emonth { get; set; }
        public double etotal { get; set; }
        public int status { get; set; }
        public string turnon_time { get; set; }
        public string releation_id { get; set; }
        public string type { get; set; }
        public double capacity { get; set; }
        public D d { get; set; }
        public bool it_change_flag { get; set; }
        public double tempperature { get; set; }
        public string check_code { get; set; }
        public object next { get; set; }
        public object prev { get; set; }
        public NextDevice next_device { get; set; }
        public PrevDevice prev_device { get; set; }
        public InvertFull invert_full { get; set; }
        public string time { get; set; }
        public string battery { get; set; }
        public double firmware_version { get; set; }
        public string warning_bms { get; set; }
        public string soh { get; set; }
        public string discharge_current_limit_bms { get; set; }
        public string charge_current_limit_bms { get; set; }
        public string soc { get; set; }
        public string pv_input_2 { get; set; }
        public string pv_input_1 { get; set; }
        public string back_up_output { get; set; }
        public string output_voltage { get; set; }
        public string backup_voltage { get; set; }
        public string output_current { get; set; }
        public string output_power { get; set; }
        public string total_generation { get; set; }
        public string daily_generation { get; set; }
        public string battery_charging { get; set; }
        public string last_refresh_time { get; set; }
        public string bms_status { get; set; }
        public string pw_id { get; set; }
        public string fault_message { get; set; }
        public object warning_code { get; set; }
        public double battery_power { get; set; }
        public string point_index { get; set; }
        public List<Point> points { get; set; }
        public double backup_pload_s { get; set; }
        public double backup_vload_s { get; set; }
        public double backup_iload_s { get; set; }
        public double backup_pload_t { get; set; }
        public double backup_vload_t { get; set; }
        public double backup_iload_t { get; set; }
        public object etotal_buy { get; set; }
        public double eday_buy { get; set; }
        public object ebattery_charge { get; set; }
        public double echarge_day { get; set; }
        public object ebattery_discharge { get; set; }
        public double edischarge_day { get; set; }
        public double batt_strings { get; set; }
        public object meter_connect_status { get; set; }
        public double mtactivepower_r { get; set; }
        public double mtactivepower_s { get; set; }
        public double mtactivepower_t { get; set; }
        public bool has_tigo { get; set; }
        public bool canStartIV { get; set; }
    }

    public class InvertFull
    {
        public int ct_solution_type { get; set; }
        public object cts { get; set; }
        public string sn { get; set; }
        public string check_code { get; set; }
        public string powerstation_id { get; set; }
        public string name { get; set; }
        public string model_type { get; set; }
        public int change_type { get; set; }
        public int change_time { get; set; }
        public double capacity { get; set; }
        public double eday { get; set; }
        public double iday { get; set; }
        public double etotal { get; set; }
        public double itotal { get; set; }
        public double hour_total { get; set; }
        public int status { get; set; }
        public long turnon_time { get; set; }
        public double pac { get; set; }
        public double tempperature { get; set; }
        public double vpv1 { get; set; }
        public double vpv2 { get; set; }
        public double vpv3 { get; set; }
        public double vpv4 { get; set; }
        public double ipv1 { get; set; }
        public double ipv2 { get; set; }
        public double ipv3 { get; set; }
        public double ipv4 { get; set; }
        public double vac1 { get; set; }
        public double vac2 { get; set; }
        public double vac3 { get; set; }
        public double iac1 { get; set; }
        public double iac2 { get; set; }
        public double iac3 { get; set; }
        public double fac1 { get; set; }
        public double fac2 { get; set; }
        public double fac3 { get; set; }
        public double istr1 { get; set; }
        public double istr2 { get; set; }
        public double istr3 { get; set; }
        public double istr4 { get; set; }
        public double istr5 { get; set; }
        public double istr6 { get; set; }
        public double istr7 { get; set; }
        public double istr8 { get; set; }
        public double istr9 { get; set; }
        public double istr10 { get; set; }
        public double istr11 { get; set; }
        public double istr12 { get; set; }
        public double istr13 { get; set; }
        public double istr14 { get; set; }
        public double istr15 { get; set; }
        public double istr16 { get; set; }
        public long last_time { get; set; }
        public double vbattery1 { get; set; }
        public double ibattery1 { get; set; }
        public double pmeter { get; set; }
        public double soc { get; set; }
        public double soh { get; set; }
        public object bms_discharge_i_max { get; set; }
        public double bms_charge_i_max { get; set; }
        public int bms_warning { get; set; }
        public int bms_alarm { get; set; }
        public int battary_work_mode { get; set; }
        public int workmode { get; set; }
        public double vload { get; set; }
        public double iload { get; set; }
        public double firmwareversion { get; set; }
        public object bmssoftwareversion { get; set; }
        public double pbackup { get; set; }
        public double seller { get; set; }
        public double buy { get; set; }
        public double yesterdaybuytotal { get; set; }
        public double yesterdaysellertotal { get; set; }
        public object yesterdayct2sellertotal { get; set; }
        public object yesterdayetotal { get; set; }
        public double yesterdayetotalload { get; set; }
        public int yesterdaylastime { get; set; }
        public double thismonthetotle { get; set; }
        public double lastmonthetotle { get; set; }
        public double ram { get; set; }
        public double outputpower { get; set; }
        public int fault_messge { get; set; }
        public object battery1sn { get; set; }
        public object battery2sn { get; set; }
        public object battery3sn { get; set; }
        public object battery4sn { get; set; }
        public object battery5sn { get; set; }
        public object battery6sn { get; set; }
        public object battery7sn { get; set; }
        public object battery8sn { get; set; }
        public double pf { get; set; }
        public double pv_power { get; set; }
        public double reactive_power { get; set; }
        public object leakage_current { get; set; }
        public int isoLimit { get; set; }
        public bool isbuettey { get; set; }
        public bool isbuetteybps { get; set; }
        public bool isbuetteybpu { get; set; }
        public bool isESUOREMU { get; set; }
        public double backUpPload_S { get; set; }
        public double backUpVload_S { get; set; }
        public double backUpIload_S { get; set; }
        public double backUpPload_T { get; set; }
        public double backUpVload_T { get; set; }
        public double backUpIload_T { get; set; }
        public object eTotalBuy { get; set; }
        public double eDayBuy { get; set; }
        public object eBatteryCharge { get; set; }
        public double eChargeDay { get; set; }
        public object eBatteryDischarge { get; set; }
        public double eDischargeDay { get; set; }
        public double battStrings { get; set; }
        public object meterConnectStatus { get; set; }
        public double mtActivepowerR { get; set; }
        public double mtActivepowerS { get; set; }
        public double mtActivepowerT { get; set; }
        public object ezPro_connect_status { get; set; }
        public string dataloggersn { get; set; }
        public object equipment_name { get; set; }
        public bool hasmeter { get; set; }
        public object meter_type { get; set; }
        public double pre_hour_lasttotal { get; set; }
        public long pre_hour_time { get; set; }
        public double current_hour_pv { get; set; }
        public object extend_properties { get; set; }
        public object eP_connect_status_happen { get; set; }
        public object eP_connect_status_recover { get; set; }
        public double total_sell { get; set; }
        public double total_buy { get; set; }
        public List<object> errors { get; set; }
        public double safetyConutry { get; set; }
        public object deratingMode { get; set; }
        public object master { get; set; }
        public object parallel_code { get; set; }
    }

    public class Kpi
    {
        public double month_generation { get; set; }
        public double pac { get; set; }
        public double power { get; set; }
        public double total_power { get; set; }
        public double day_income { get; set; }
        public double total_income { get; set; }
        public double yield_rate { get; set; }
        public string currency { get; set; }
    }

    public class Left
    {
        public bool isHT { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public int isFaultMsg { get; set; }
        public int faultMsgCode { get; set; }
    }

    public class NextDevice
    {
        public object sn { get; set; }
        public bool isStored { get; set; }
    }

    public class Point
    {
        public int target_index { get; set; }
        public string target_name { get; set; }
        public string display { get; set; }
        public string unit { get; set; }
        public string target_key { get; set; }
        public string text_cn { get; set; }
        public object target_sn_six { get; set; }
        public object target_sn_seven { get; set; }
        public object target_type { get; set; }
        public object storage_name { get; set; }
    }

    public class Powerflow
    {
        public string pv { get; set; }
        public int pvStatus { get; set; }
        public string bettery { get; set; }
        public int betteryStatus { get; set; }
        public object betteryStatusStr { get; set; }
        public string load { get; set; }
        public int loadStatus { get; set; }
        public string grid { get; set; }
        public int soc { get; set; }
        public string socText { get; set; }
        public bool hasEquipment { get; set; }
        public int gridStatus { get; set; }
        public bool isHomKit { get; set; }
        public bool isBpuAndInverterNoBattery { get; set; }
        public bool isMoreBettery { get; set; }
    }

    public class PrevDevice
    {
        public object sn { get; set; }
        public bool isStored { get; set; }
    }

    public class Right
    {
        public bool isHT { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public int isFaultMsg { get; set; }
        public int faultMsgCode { get; set; }
    }

    public class Root
    {
        public Info info { get; set; }
        public Kpi kpi { get; set; }
        public int powercontrol_status { get; set; }
        public List<object> images { get; set; }
        public Weather weather { get; set; }
        public List<Inverter> inverter { get; set; }
        public Hjgx hjgx { get; set; }
        public HomKit homKit { get; set; }
        public bool isTigo { get; set; }
        public int tigoIntervalTimeMinute { get; set; }
        public SmuggleInfo smuggleInfo { get; set; }
        public bool hasPowerflow { get; set; }
        public Powerflow powerflow { get; set; }
        public bool hasGridLoad { get; set; }
        public bool isParallelInventers { get; set; }
        public bool isEvCharge { get; set; }
        public object evCharge { get; set; }
        public bool hasEnergeStatisticsCharts { get; set; }
        public EnergeStatisticsCharts energeStatisticsCharts { get; set; }
        public EnergeStatisticsTotals energeStatisticsTotals { get; set; }
        public Soc soc { get; set; }
        public List<object> environmental { get; set; }
        public List<Equipment> equipment { get; set; }
    }

    public class SmuggleInfo
    {
        public bool isAllSmuggle { get; set; }
        public bool isSmuggle { get; set; }
        public object descriptionText { get; set; }
        public object sns { get; set; }
    }

    public class Soc
    {
        public int power { get; set; }
        public int status { get; set; }
    }

    public class Weather
    {
        public List<HeWeather6> HeWeather6 { get; set; }
    }
}