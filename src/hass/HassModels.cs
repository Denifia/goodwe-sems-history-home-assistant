namespace Hass.Db;

// Manually generated based on Home Assistant schema

public class statistics
{
    public int id { get; set; }
    public DateTime created { get; set; }
    public DateTime start { get; set; }
    public float? mean { get; set; }
    public float? min { get; set; }
    public float? max { get; set; }
    public DateTime? last_reset { get; set; }
    public decimal state { get; set; } // decimal for less precision 
    public float sum { get; set; }
    public int metadata_id { get; set; }
}

public class statistics_short_term
{
    public int id { get; set; }
    public DateTime created { get; set; }
    public DateTime start { get; set; }
    public float? mean { get; set; }
    public float? min { get; set; }
    public float? max { get; set; }
    public DateTime? last_reset { get; set; }
    public decimal state { get; set; } // decimal for less precision
    public float sum { get; set; }
    public int metadata_id { get; set; }
}

public class metadata
{
    public int id { get; set; }
    public string statistic_id { get; set; }
}