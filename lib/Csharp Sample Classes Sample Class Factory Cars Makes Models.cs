using System;
using System.Collections.Generic;

namespace Sample
{
    public class Car
    {
        public int ID { get; set; }

        public CarMake Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public CarCondition Condition { get; set; }
    }

    public enum CarCondition
    {
        Excellent,
        Good,
        Fair,
        Poor
    }

    public enum CarMake
    {
        Acura,
        AstonMartin,
        Audi,
        Bentley,
        BMW,
        Buick,
        Cadillac,
        Chevrolet,
        Chrysler,
        Dodge,
        Ferrari,
        FIAT,
        Fisker,
        Ford,
        GMC,
        Honda,
        Hyundai,
        Infiniti,
        Jaguar,
        Jeep,
        Kia,
        Lamborghini,
        LandRover,
        Lexus,
        Lincoln,
        Lotus,
        Maserati,
        Maybach,
        Mazda,
        McLaren,
        MercedesBenz,
        MINI,
        Mitsubishi,
        Nissan,
        Porsche,
        Ram,
        RollsRoyce,
        Scion,
        smart,
        Subaru,
        Suzuki,
        Toyota,
        Volkswagen,
        Volvo
    }

    public class CarFactory
    {
        public static List<Car> GetCars(int count)
        {
            var returnCars = new List<Car>();

            int uBoundCondition = Enum.GetValues(typeof(CarCondition)).GetUpperBound(0);
            int uBoundMake = Enum.GetValues(typeof(CarMake)).GetUpperBound(0);


            for (int carIndex = 0; carIndex < count; carIndex++)
            {
                var newCar = new Car();
                newCar.ID = carIndex;
                newCar.Make = (CarMake)GenerateData.RandomInt(0, uBoundMake);
                newCar.Model = GenerateData.RandomString(5, 20);
                newCar.Year = GenerateData.RandomInt(DateTime.Now.Year - 25, DateTime.Now.Year);
                int yearsOld = DateTime.Now.Year - newCar.Year;
                newCar.Mileage = GenerateData.RandomInt(yearsOld * 5000, yearsOld * 50000);
                newCar.Condition = (CarCondition)GenerateData.RandomInt(0, uBoundCondition);

                returnCars.Add(newCar);
            }

            return returnCars;
        }
    }
}

