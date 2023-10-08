using BTurk.Automation.Core.Credentials;
using FluentAssertions;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace BTurk.Automation.Core.UnitTests;

public class UserCredentialsTests
{
    [Fact]
    public void DecryptCredentials_CredentialsEncryptedWithStrongPassword_SuccessfullyDecryptsCredentials()
    {
        // Arrange
        var encryptionKey = "@myverystringpassword812";
        var userCredentials = new UserCredentials();
        userCredentials.EncryptCredentials("boris", "my_password", encryptionKey);

        // Act
        var credentials = userCredentials.DecryptCredentials(encryptionKey);

        // Assert
        credentials.Username.Should().Be("boris");
        credentials.Password.Should().Be("my_password");
    }
}