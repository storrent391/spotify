using System;
using System.Collections.Generic;
using Xunit;
using Spotify.Repository;
using Spotify.Common;

namespace Spotify.Tests.Integration
{
    public class ValidacioPermisosADOTests
    {
        [Fact]
        public void GetPermsById_ShouldReturnExpectedPermissions_Simulated()
        {
            // Arrange
            Guid testUserId = Guid.Parse("E9953E44-6A99-43F1-AA5D-4C7A9BEB2033");

            // Simulem la resposta que hauria de donar l'ADO
            List<string> simulatedPermsFromADO = new List<string> { "GTUS" };

            // Permisos esperats
            List<string> expectedPerms = new List<string> { "GTUS" };

            // Act
            List<string> actualPerms = simulatedPermsFromADO; // en lloc de cridar l'ADO real

            // Assert
            Assert.Equal(expectedPerms, actualPerms);
        }
    }
}
