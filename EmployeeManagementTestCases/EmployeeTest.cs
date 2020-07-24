namespace EmployeeManagementTestCases
{
    using EmployeeBuisenessLayer.Interface;
    using EmployeeBuisenessLayer.Services;
    using EmployeeCommonLayer;
    using EmployeeManagement.Controllers;
    using EmployeeRepositoryLayer.Interface;
    using EmployeeRepositoryLayer.Services;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class EmployeeTest
    {
        EmployeeController employeeController;
        IEmployeeBL employeeBuiseness;
        IEmployeeRL employeeRepository;

        public EmployeeTest()
        {
            employeeRepository = new EmployeeRL();
            employeeBuiseness = new EmployeeBL(employeeRepository);
            employeeController = new EmployeeController(employeeBuiseness);
        }

        [Fact]
        public void GivenAddEmployee_WhenCalled_ShouldReturnOkResult()
        {
            var testData = new EmployeeResponseModel
            { FirstName = "Akshay", LastName = "Kumar", Gender = "Male", EmailId = "akshay123@gmail.com", PhoneNumber = "9874516300", City = "Pune" };

            //var data = employeeController.AddEmployee(testData);
            //Assert.IsType<OkObjectResult>(data);
        }
    }
}
