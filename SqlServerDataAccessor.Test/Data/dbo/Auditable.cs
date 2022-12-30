namespace SqlServerDataAccessor.Test.Data.dbo
{
    internal abstract class Auditable
    {
        public DateTime Created { get;  set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedBy { get; set;}
        public DateTime LastUpdatedBy { get; set;}
    }
}