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
            SimplePersonDTO dtoMapped = somePerson.CopyValues<SimplePersonDTO>() as SimplePersonDTO;

            //assert
            Assert.AreEqual(somePerson.Firstname, dtoMapped.Firstname);
            Assert.AreEqual(somePerson.Surname, dtoMapped.Surname);
        }

    }
}
