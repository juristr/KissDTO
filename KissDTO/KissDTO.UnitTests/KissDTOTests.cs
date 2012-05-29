using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KissDTO.UnitTests.Utils;

namespace KissDTO.UnitTests
{
    class SimplePersonDTO
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
    }

    class PersonWithCompleteNestedsDTO
    {
        public Location Residence { get; set; }
    }

    [TestClass]
    public class KissDTOTests
    {

        [TestMethod]
        public void ShouldCorrectlyMapASimple1stLevelObjects()
        {
            //arrange
            var somePerson = new Person{
                 Firstname = "Juri",
                 Surname = "Strumpflohner",
                 Age = 27
            };

            //act
            var dtoMapped = somePerson.CopyValues<SimplePersonDTO>();

            //assert
            Assert.AreEqual(somePerson.Firstname, dtoMapped.Firstname);
            Assert.AreEqual(somePerson.Surname, dtoMapped.Surname);
        }

        [TestMethod]
        public void ShouldMapNestedObjectsWithoutDTOs()
        {
            //arrange
            var somePerson = new Person
            {
                Residence = new Location
                {
                    Name = "Bolzano",
                    Nation = new Nation
                    {
                         Name = "Italy"
                    }
                }
            };

            //act
            var dtoMapped = somePerson.CopyValues<PersonWithCompleteNestedsDTO>();

            //assert
            Assert.IsNotNull(dtoMapped.Residence.Nation);
        }

    }
}
