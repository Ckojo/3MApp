using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimAplikacija_V2._0.DataAccess;

namespace TrimAplikacija_V2._0.Helpers
{
    public class CompanyHelper
    {
        public static List<Company> GetCompanies()
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Company>();
                    
                    if (companies != null)
                    {
                        return companies.ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static Company GetCompany(int id)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Company>();

                    if(companies != null)
                    {
                        return db.Companies.Find(id);
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static List<Company> GetCompanyByName(string name)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Company>();

                    if (companies != null)
                    {
                        return companies.Where(c => c.Name == name).ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static bool InsertCompany(Dictionary<string, string> companyData)
        {
            try
            {
                using (DataContext data = new DataContext())
                {
                    var companies = data.Set<Company>();

                    if (companies != null)
                    {
                        companies.Add(new Company
                        {
                            Name = companyData["companyName"],
                            City = companyData["companyCity"],
                            Email = companyData["companyEmail"],
                            TIN = companyData["companyTIN"],
                            TotalDebt = double.Parse(companyData["companyTotalDebt"])
                        });
                    }

                    if (data.SaveChanges() == 1)
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

        public static bool UpdateCompany(int id, Dictionary<string, string> data)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Company>();

                    if (companies != null)
                    {
                        Company company = companies.Find(id);

                        company.Name= data["companyName"];
                        company.City = data["companyCity"];
                        company.Email = data["companyEmail"];
                        company.TIN = data["companyTIN"];

                        if (db.SaveChanges() == 1)
                            return true;
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static bool DeleteCompany(int id)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var companies = db.Set<Company>();
                    
                    if(companies != null)
                    {
                        var companyToDelete = companies.Find(id);

                        if(companyToDelete != null)
                        {
                            companies.Remove(companyToDelete);
                        }

                        if (db.SaveChanges() == 1)
                            return true;
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }
    }
}