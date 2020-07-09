using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.PersonalizationManager;
using Moq;
using System.Security;
using FluentAssertions;
using System.Runtime.InteropServices;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        PersonalizationManager _target;
        Mock<IPersonalizationManagerUtilityFactory> _factoryMock;
        Mock<IConsoleWrapper> _mockConsole;
        Mock<IWindowsIdentityWrapper> _mockIdentity;
        [TestInitialize]
        public void TestSetup()
        {
            _mockConsole = new Mock<IConsoleWrapper>();
            _mockIdentity = new Mock<IWindowsIdentityWrapper>();

            _factoryMock = new Mock<IPersonalizationManagerUtilityFactory>();
            _factoryMock.Setup(x => x.CreateIdentity).Returns(_mockIdentity.Object);
            _factoryMock.Setup(x => x.CreateConsole).Returns(_mockConsole.Object);

            _target = new PersonalizationManager(_factoryMock.Object);
        }

        [TestMethod]
        public void TestSetCredentialsSimple()
        {
            //
            // SETUP
            //
            Mock<ICredentialWrapper> mockCred = new Mock<ICredentialWrapper>();

            _factoryMock.Setup(x => x.CreateCredential()).Returns(mockCred.Object);

            ConsoleKeyInfo a_key = new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false);
            ConsoleKeyInfo enter = new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);

            _mockConsole.SetupSequence(x => x.ReadKey(true))
                .Returns(a_key)
                .Returns(enter);

            _mockIdentity.Setup(x => x.Name).Returns("user");
            mockCred.SetupProperty(x => x.Username);
            mockCred.SetupProperty(x => x.SecurePassword);

            //
            // TEST
            //
            bool status = _target.SetCredentials("target");

            //
            // VERIFY
            //
            SecureString expected = new SecureString();
            expected.AppendChar('a');

            mockCred.Object.Username.Should().Be("user");
            CompareSecureStrings(mockCred.Object.SecurePassword, expected).Should().Be(true);

        }

        [TestMethod]
        public void TestSetCredentialsBackspace()
        {
            //
            // SETUP
            //
            Mock<ICredentialWrapper> mockCred = new Mock<ICredentialWrapper>();

            _factoryMock.Setup(x => x.CreateCredential()).Returns(mockCred.Object);

            ConsoleKeyInfo a_key = new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false);
            ConsoleKeyInfo b_key = new ConsoleKeyInfo('b', ConsoleKey.B, false, false, false);
            ConsoleKeyInfo backspace_key = new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false);
            ConsoleKeyInfo enter = new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);

            _mockConsole.SetupSequence(x => x.ReadKey(true))
                .Returns(a_key)
                .Returns(b_key)
                .Returns(backspace_key)
                .Returns(enter);

            _mockIdentity.Setup(x => x.Name).Returns("user");
            mockCred.SetupProperty(x => x.Username);
            mockCred.SetupProperty(x => x.SecurePassword);

            //
            // TEST
            //
            bool status = _target.SetCredentials("target");

            //
            // VERIFY
            //
            SecureString expected = new SecureString();
            expected.AppendChar('a');

            mockCred.Object.Username.Should().Be("user");
            CompareSecureStrings(mockCred.Object.SecurePassword, expected).Should().Be(true);

        }

        [TestMethod]
        public void TestGetCredentialsFailLoad()
        {
            //
            // SETUP
            //
            Mock<ICredentialWrapper> mockCred = new Mock<ICredentialWrapper>();

            _factoryMock.Setup(x => x.CreateCredential()).Returns(mockCred.Object);
            mockCred.SetupProperty(x => x.Target);
            mockCred.Setup(x => x.Load()).Returns(false);
            //
            // TEST
            //
            (var user, var password) = _target.GetCredentials("target");

            //
            // VERIFY
            //
            user.Should().Be(null);
            password.Should().Be(null);

        }

        [TestMethod]
        public void TestGetCredentialsSuccessLoad()
        {
            //
            // SETUP
            //
            SecureString expected = new SecureString();
            expected.AppendChar('a');

            Mock<ICredentialWrapper> mockCred = new Mock<ICredentialWrapper>();

            _factoryMock.Setup(x => x.CreateCredential()).Returns(mockCred.Object);
            mockCred.SetupProperty(x => x.Target);
            mockCred.Setup(x => x.Username).Returns("user");
            mockCred.Setup(x => x.SecurePassword).Returns(expected);

            mockCred.Setup(x => x.Load()).Returns(true);
            //
            // TEST
            //
            (var user, var password) = _target.GetCredentials("target");

            //
            // VERIFY
            //
            user.Should().Be("user");
            password.Should().Be(expected);

        }

        /// <summary>
        /// Utility for comparing the secure strings
        /// </summary>
        /// <param name="target">test value</param>
        /// <param name="expected">expected value</param>
        /// <returns>true if the strings are the same.  false if different</returns>
        private bool CompareSecureStrings(SecureString target, SecureString expected)
        {
            var targetBstr = Marshal.SecureStringToBSTR(target);
            var expectedBstr = Marshal.SecureStringToBSTR(expected);
            bool status = false;

            try
            {
                var targetString = Marshal.PtrToStringBSTR(targetBstr);
                var expectedString = Marshal.PtrToStringBSTR(expectedBstr);

                status = targetString == expectedString ? true : false;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(targetBstr);
                Marshal.ZeroFreeBSTR(expectedBstr);
            }

            return status;
        }
    }
}
