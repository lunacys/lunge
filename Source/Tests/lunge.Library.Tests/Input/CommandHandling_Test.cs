using System;
using lunge.Library.Entities;
using lunge.Library.Input;
using NUnit.Framework;

namespace lunge.Library.Tests.Input
{
    class TestEntity : Entity { }

    class JumpCommand : IInputCommand<TestEntity>
    {
        public void Execute(TestEntity entity)
        {
            Console.WriteLine("Jumping entity with id " + entity.Id);
        }
    }

    [TestFixture]
    public class CommandHandling_Test
    {
        
    }
}