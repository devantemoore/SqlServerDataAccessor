using Task = SqlServerDataAccessor.Test.Data.dbo.Task;

var conn = @"Data Source=DMOORE-202108\SQLEXPRESS;Initial Catalog=tevms;Integrated Security=True;MultipleActiveResultSets=True;encrypt=false"; // encrypt defaults to true for good reasons
var dataAccessor = new SqlServerDataAccessor.SqlServerDataAccessor(conn);

dataAccessor.Get<Task>();
//var task = dataAccessor.Get<Task>(guid);
//var addedTask = dataAccessor.Insert<Task>(task);
//var updatedTask = dataAccessor.Update<Task>(task);
//var deletedTask = dataAccessor.Remove<Task>(guid);

