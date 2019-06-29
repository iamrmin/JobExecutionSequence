namespace JobExecutionSequence.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests of JobDependencyService
    /// </summary>
    public class JobDependencyServiceTests
    {
        static object[] positiveCases =
        {
            new object[] { "a =>", "a" },
            new object[] { $"a =>{Environment.NewLine}b =>{Environment.NewLine}c =>", "abc" },
            new object[] { $"a =>{Environment.NewLine}b => c{Environment.NewLine}c =>", "acb" },
            new object[] { $"a =>{Environment.NewLine}b => c{Environment.NewLine}c => f{Environment.NewLine}d => a{Environment.NewLine}e => b{Environment.NewLine}f =>", "afcbde" }
        };

        static object[] negativeCases =
        {
            new object[] { $"a =>{Environment.NewLine}a =>{Environment.NewLine}c =>", "Duplicate job found" },
            new object[] { $"a =>{Environment.NewLine}b =>{Environment.NewLine}c => c", "Jobs can’t depend on themselves" },
            new object[] { $"a => b{Environment.NewLine}b => c{Environment.NewLine}c => a", "Circular dependency found" },
            new object[] { $"a => b{Environment.NewLine}c =>", "Dependent job doesn't found" }
        };

        [TestCaseSource(nameof(positiveCases))]
        public void When_Initialize_JobDependencyService_With_CorrectData_Return_ExpectedResult(string rawInput, string expectedResult)
        {
            var jobDependencyService = new JobDependencyService(rawInput);
            var output = jobDependencyService.EvaluateJobSequence();

            Assert.AreEqual(expectedResult, output);
        }

        [TestCaseSource(nameof(negativeCases))]
        public void When_Initialize_JobDependencyService_With_WrongData_Return_Error(string rawInput, string expectedMessage)
        {
            Assert.That(() =>
            {
                var jobDependencyService = new JobDependencyService(rawInput);
                jobDependencyService.EvaluateJobSequence();
            }, Throws.TypeOf<InvalidDataException>().With.Message.EqualTo(expectedMessage));
        }
    }
}