namespace SqlServerDataAccessor.Test.Data.dbo
{
    internal abstract class Auditable
    {
        public DateTime Created { get;  set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; }
        public string CreatedBy { get; set;}
        public string LastUpdatedBy { get; set;}
    }
}