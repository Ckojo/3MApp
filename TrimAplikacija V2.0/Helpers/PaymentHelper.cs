using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimAplikacija_V2._0.DataAccess;

namespace TrimAplikacija_V2._0.Helpers
{
    public class PaymentHelper
    {
        public static List<Payment> GetPayments()
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var payments = db.Set<Payment>();

                    if (payments != null)
                    {
                        return payments.ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static List<Payment> GetPaymentsByEmployee(int id)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var payments = db.Set<Payment>();

                    if (payments != null)
                    {
                        return payments.Where(e => e.Employee.EmployeeId == id).ToList();
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }

        public static Payment GetPayment(int id)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var payments = db.Set<Payment>();

                    if (payments != null)
                    {
                        return db.Payments.Find(id);
                    }
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new NotImplementedException(exception.Message);
            }
        }
        
        public static bool InsertPayment(Dictionary<string, string> paymentData)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var payments = db.Set<Payment>();

                    if (payments != null)
                    {
                        payments.Add(new Payment
                        {
                            PurchaseDate = DateTime.Parse(paymentData["purchaseDate"]),
                            Installements = int.Parse(paymentData["installements"]),
                            PurchaseAmount = double.Parse(paymentData["purchaseAmount"]),
                            InstallementAmount = double.Parse(paymentData["installementAmount"]),
                            TotalDebt = double.Parse(paymentData["totalDebt"]),
                            Paid = double.Parse(paymentData["paid"]),
                            DebtLeft = double.Parse(paymentData["debtLeft"]),
                            Employee = db.Employees.Find(int.Parse(paymentData["employeeId"]))
                        });
                    }

                    if (db.SaveChanges() > 1)
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

        public static bool InsertInstallenmentDetails(int paymentId, List<(DateTime InstallenmentDate, double InstallenmentAmount)> paymentDetails)
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var paymentDate = db.Set<PaymentDate>();
                    
                    if (paymentDate != null)
                    {
                        var payments = paymentDate.Where(p => p.Payment.Id == paymentId).ToList();

                        if(payments.Count == 0)
                        {
                            foreach (var item in paymentDetails)
                            {
                                paymentDate.Add(new PaymentDate()
                                {
                                    Payment = db.Payments.Find(paymentId),
                                    InstallenmentAmount = db.InstallenmentAmounts.Add(new InstallenmentAmount()
                                    {
                                        Paid = item.InstallenmentAmount
                                    }),
                                    InstallenmentDate = db.InstallenmentDates.Add(new InstallenmentDate()
                                    {
                                        DatePaid = item.InstallenmentDate
                                    })
                                });
                            }
                        }
                    }

                    if (db.SaveChanges() > 1)
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
