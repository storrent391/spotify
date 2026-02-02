using System;
using Spotify.Encryption;
using Spotify.Model;
using Xunit;

namespace Spotify.Tests
{
    public class PasswordEncryptionTests3
    {
        [Fact]
        public void Salt_ShouldNotBeEmpty()
        {
            // Arrange
            var user = new User { Password = "myPassword123" };

            // Act
            PasswordEncryption.ConvertPassword(user);

            // Assert
            // Comprovem que el salt s'ha generat i no és buit
            Assert.False(string.IsNullOrEmpty(user.Salt));
        }
    }
}
