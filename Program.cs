using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Course.Protobuf.Test;

Console.WriteLine("Welcome to Protobuf Test!");
var birthDate = new DateTime(1976, 7, 9);
var birthDateUtc = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);
var birthDateTimestamp = Timestamp.FromDateTime(birthDateUtc);
var employee = new Employee
{
    Id = 1,
    FirstName = "John",
    LastName = "Wick",
    IsRetired = false,
    BirthDate = birthDateTimestamp,
    Age = 45, // Use age instead of birth date - because oneof is used and we need to send only one of the two fields
    MaritalStatus = Employee.Types.MaritalStatus.Married,
    CurrentAddress = new Address
    {
        City = "Anytown",
        StreetName = "123 Main St",
        HouseNumber = 123,
        ZipCode = "12345",
    },
    PreviousEmployers = { "Google", "Microsoft", "Apple" },
};

// Serialize the employee object to a file
using (var output = File.Create("emp.dat"))
{
    employee.WriteTo(output);
}

// Deserialize the employee object from the file
Employee empFromFile;
using (var input = File.OpenRead("emp.dat"))
{
    empFromFile = Employee.Parser.ParseFrom(input);
}

Console.WriteLine(empFromFile.ToString());

Console.WriteLine("Finished!");
