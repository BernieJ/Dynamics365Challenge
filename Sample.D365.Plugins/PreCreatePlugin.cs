using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.D365.Plugins
{
    public class PreCreatePlugin : PluginBase
    {
        public PreCreatePlugin() : base("Contact")
        {

        }
        protected override void ExecuteCrmPlugin(LocalPluginContext localcontext)
        {
            if(localcontext.PluginExecutionContext.InputParameters.Contains("Contact"))
            {
                try
                {
                    var newContact = (Entity)localcontext.PluginExecutionContext.InputParameters["Contact"];

                    //Update Age
                    var dateOfBirth = Convert.ToDateTime(newContact["DateOfBirth"].ToString());

                    newContact["Age"] = CalculateAge(dateOfBirth);

                    //Update investment Maturity date
                    var joinDate = Convert.ToDateTime(newContact["JoiningDate"].ToString());
                    var investmentPeriod = Convert.ToInt32(newContact["InvestmentPeriod(Months)"].ToString());

                    newContact["MaturityDate"] = joinDate.AddMonths(investmentPeriod);


                    //Update estimated return
                    decimal interestRate = decimal.Parse(newContact["InterestRate(%)"].ToString()) / 100;
                    decimal initialInvestment = decimal.Parse(newContact["InitialInvestment"].ToString());

                    newContact["EstimatedReturn"] = CalculateReturnOnInvestment(initialInvestment, investmentPeriod, interestRate);

                    newContact["StatusReason"] = "In-Force";
                }
                catch (Exception ex)
                {
                    localcontext.TracingService.Trace(ex.ToString());
                    throw;
                }
            }
        }

        private decimal CalculateReturnOnInvestment(decimal amount, int investmentPeriod, decimal intrest)
        {
            intrest /= 12;

            var newAmount = amount;

            for (int i = 1; i <= investmentPeriod; i++)
            {
                newAmount = newAmount * intrest;
            }

            return newAmount;
        }

        private  int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;

            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}
