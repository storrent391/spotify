using System;
using Spotify.Encryption;
using Spotify.Model;
using Xunit;

namespace Spotify.Tests
{
    public class PasswordEncryptionTests2
    {
        [Fact]
        public void ConvertPassword_SamePassword_ShouldProduceDifferentHashes()
        {
            // Arrange
            var user1 = new User { Password = "myPassword123" };
            var user2 = new User { Password = "myPassword123" };

            // Act
            PasswordEncryption.ConvertPassword(user1);
            PasswordEncryption.ConvertPassword(user2);

            // Assert
            Assert.NotEqual(user1.Password, user2.Password);
        }
    }
}
