using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;

namespace TinyURL.Commands.Tests
{
    public class TinyURLCommandRegistryTests
    {
        private readonly Mock<ICommand> _createCommandMock;
        private readonly Mock<ICommand> _deleteCommandMock;
        private readonly Mock<ICommand> _getCommandMock;
        private readonly Mock<ICommand> _statsCommandMock;
        private readonly TinyURLCommandRegistry _commandRegistry;

        public TinyURLCommandRegistryTests()
        {
            _createCommandMock = new Mock<ICommand>();
            _deleteCommandMock = new Mock<ICommand>();
            _getCommandMock = new Mock<ICommand>();
            _statsCommandMock = new Mock<ICommand>();

            _createCommandMock.Setup(c => c.Name).Returns("create");
            _deleteCommandMock.Setup(c => c.Name).Returns("delete");
            _getCommandMock.Setup(c => c.Name).Returns("get");
            _statsCommandMock.Setup(c => c.Name).Returns("stats");

            var commands = new List<ICommand>
            {
                _createCommandMock.Object,
                _deleteCommandMock.Object,
                _getCommandMock.Object,
                _statsCommandMock.Object
            };

            _commandRegistry = new TinyURLCommandRegistry(commands);
        }

        [Fact]
        public void FindCommand_ShouldReturnCommand_WhenCommandExists()
        {
            // Act
            var command = _commandRegistry.FindCommand("create");

            // Assert
            Assert.NotNull(command);
            Assert.Equal("create", command.Name);
        }

        [Fact]
        public void FindCommand_ShouldReturnNull_WhenCommandDoesNotExist()
        {
            // Act
            var command = _commandRegistry.FindCommand("nonexistent");

            // Assert
            Assert.Null(command);
        }

        [Fact]
        public void FindCommand_ShouldBeCaseInsensitive()
        {
            // Act
            var command = _commandRegistry.FindCommand("CREATE");

            // Assert
            Assert.NotNull(command);
            Assert.Equal("create", command.Name);
        }
    }
}
