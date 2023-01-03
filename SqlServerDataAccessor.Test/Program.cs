using Task = SqlServerDataAccessor.Test.Data.dbo.Task;

var conn = @"Data Source=DMOORE-202108\SQLEXPRESS;Initial Catalog=tevms;Integrated Security=True;MultipleActiveResultSets=True;encrypt=false"; // encrypt defaults to true for good reasons
var dataAccessor = new SqlServerDataAccessor.SqlServerDataAccessor(conn);



var tasks = dataAccessor.Get<Task>();

var guid = Guid.Parse("{68929ed8-6b37-481a-8641-254f5cf9fda2}"); 
//var task = dataAccessor.Get<Task>(guid);
//var addedTask = dataAccessor.Insert<Task>(new Task()
//{
//    Id = Guid.NewGuid(),
//    Name = "Another New Task to Insert",
//    Number = 11,
//    Description = "testing inserting another new task using the crud library",
//    Created = DateTime.Now,
//    CreatedBy = "devantemoore@infinity.io",
//    LastUpdated = DateTime.Now,
//    LastUpdatedBy = "devantemoore@infinity.io"
//});
//Console.WriteLine(addedTask);
//var updatedTask = dataAccessor.Update<Task>(task);
//var deletedTask = dataAccessor.Remove<Task>(guid);

