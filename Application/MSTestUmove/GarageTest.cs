using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umove_AS.Models;
using Umove_AS.Services;
namespace MSTestUmove
{
    [TestClass]
    public class GarageTest
    {
        [TestMethod]
        public void AddBus_AddbusSucces()
        {
            // Arrange
            var garage = new GarageHandler();
            var shiftplan = new ShiftPlan("nat"); //DK tilføjet shiftplan object
            var garage = new Garage();
            var bus = new Bus("Bus1", 300, 2.5, shiftplan);

            // Act
            garage.AddBus(bus);

            // Assert
            var allBuses = garage.GetAllBusses();
            Assert.AreEqual(1, allBuses.Count);
            Assert.AreEqual("Bus1", allBuses[0].ID);
        }
        [TestMethod]
        public void AddBus_DublicateAlarm()
        {
            // Arrange
            var garage = new Garage();
            var shiftplan = new ShiftPlan("nat"); //DK tilføjet shiftplan object

            // Act
            garage.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));
            garage.AddBus(new Bus("Bus1", 300, 2.5, shiftplan)); //Skal sige fejl
        }

        [TestMethod]
        public void EditBus_UpdateData()
        {
            var garage = new GarageHandler();
            // Arrange
            var garage = new Garage();
            var shiftplan = new ShiftPlan("nat"); //DK tilføjet shiftplan object

            // Act
            garage.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));

            garage.EditBus("Bus1", 350, 2.5);

            var bus = garage.GetAllBusses().First();

            // Assert
            Assert.AreEqual(350, bus.BatteryCapacity);
            Assert.AreEqual(2.5, bus.KmPerKWh);
        }

        [TestMethod]
        public void RemoveBus_Delete()
        {
            // Arrange
            var garage = new Garage();
            var shiftplan = new ShiftPlan("nat"); //DK tilføjet shiftplan object

            // Act
            garage.AddBus(new Bus("Bus1", 300, 2.5,shiftplan));

            garage.RemoveBus("Bus1");

            var buses = garage.GetAllBusses();

            // Assert
            Assert.AreEqual(0, buses.Count);
        }

        [TestMethod]
        public void GetBatteryTimeLeft_Correct() //DK
        {
            // Arrange
            var garage = new Garage();

            //Act
            TimeSpan timeLeft = garage.GetBatteryTimeLeft(150, 25, 2.5);


            //Assert
            TimeSpan expectedTimeLeft = TimeSpan.FromHours(150 * 2.5 / 25);
            Assert.AreEqual(expectedTimeLeft, timeLeft);
        
        }

        
    }
}
