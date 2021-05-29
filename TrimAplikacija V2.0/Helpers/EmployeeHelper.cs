using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimAplikacija_V2._0.DataAccess;

namespace TrimAplikacija_V2._0.Helpers
{
    public static class EmployeeHelper
    {
        public static List<Employee> GetEmployees()
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var employees = db.Set <Employee>();

                    if (employees != null)
                    {
                        return employees.ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static Employee GetEmployee(int id)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Employee>();

                    if (companies != null)
                    {
                        return db.Employees.Find(id);
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static List<Employee> GetEmployeesByCompany(Company company)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var employees = db.Set<Employee>();

                    if (employees != null)
                    {
                        return employees.Where(e => e.Company.CompanyId == company.CompanyId).ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static bool InsertEmployee(Dictionary<string, string> employeeData)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var employees = db.Set<Employee>();

                    if (employees != null)
                    {
                        employees.Add(new Employee
                        {
                            FirstName = employeeData["FirstName"],
                            LastName = employeeData["LastName"],
                            IdNumber = employeeData["IdNumber"],
                            UniqueNumber = employeeData["UniqueNumber"],
                            Company = db.Companies.Find(int.Parse(employeeData["CompanyId"]))
                        });
                    }

                    if (db.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            };
        }
    }
}
