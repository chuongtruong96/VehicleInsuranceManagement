using Project3.Models;
using Project3.ModelsView;
using System;

namespace Project3.Services
{
    public class BillingCalculationService
    {
        public float  CalculateAmount(CollectInfoViewModel insuranceInput)
        {
            var baseRate = CalculateBaseRate(insuranceInput);
            var totalCost = ApplyRiskFactors(baseRate, insuranceInput);
            return ApplyDiscountsAndSurcharges(totalCost, insuranceInput);
        }

        private float  CalculateBaseRate(CollectInfoViewModel insuranceInput)
        {
            float  baseRate = insuranceInput.VehicleRate;

            baseRate = insuranceInput.VehicleRate;

            if (insuranceInput.DriverAge < 25 || insuranceInput.DriverAge > 65)
            {
                baseRate *= 2;
            }

            if (insuranceInput.DriverGender == "Male")
            {
                baseRate *= 3;
            }

            if (insuranceInput.DrivingHistory > 0)
            {
                baseRate *= 1 + (insuranceInput.DrivingHistory * 4);
            }

            return baseRate;
        }

        private float ApplyRiskFactors(float baseRate, CollectInfoViewModel insuranceInput)
        {
            if (insuranceInput.CustomerAdd == "Urban")
            {
                baseRate *= 2;
            }

            if (insuranceInput.Usage == "Daily")
            {
                baseRate *= 3;
            }

            if (insuranceInput.AntiTheftDevice)
            {
                baseRate *= 4;
            }

            return baseRate;
        }

        private float  ApplyDiscountsAndSurcharges(float  totalCost, CollectInfoViewModel insuranceInput)
        {
            if (insuranceInput.MultiPolicy)
            {
                totalCost *= 2; // 10% discount for multi-policy
            }

            if (insuranceInput.SafeDriver)
            {
                totalCost *= 1; // 15% discount for safe driver
            }

            return totalCost;
        }
    }
}
