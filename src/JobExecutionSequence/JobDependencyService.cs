using System.IO;
using System.Linq;
using System.Text;

namespace JobExecutionSequence
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Manage job dependency
    /// </summary>
    public class JobDependencyService
    {
        /// <summary>
        /// Hold data structure of job and it dependencies
        /// </summary>
        private Dictionary<string, string> jobs;

        /// <summary>
        /// The final job sequence after dependency evaluation
        /// </summary>
        private StringBuilder jobSequence;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rawString">The raw input</param>
        public JobDependencyService(string rawString)
        {
            this.RawDataParser(rawString);
        }

        public string EvaluateJobSequence()
        {
            this.jobSequence = new StringBuilder();

            while (this.jobs.Count > 0)
            {
                var nextJob = this.jobs.First();
                var evaluatedResult = this.CheckDependency(nextJob.Key, nextJob.Value);
                jobSequence.Append(evaluatedResult);
            }

            return jobSequence.ToString();
        }

        private string CheckDependency(string jobId, string dependency, List<string> circularDependency = null)
        {
            string result;

            if (dependency == null || this.jobSequence.ToString().Contains(dependency))
                result = jobId;
            else if (jobId == dependency)
                throw new InvalidDataException("Jobs can’t depend on themselves");
            else
            {
                if (circularDependency == null)
                    circularDependency = new List<string>();

                if (circularDependency.Contains(jobId))
                    throw new InvalidDataException("Circular dependency found");

                if (!this.jobs.ContainsKey(dependency))
                    throw new InvalidDataException("Dependent job doesn't found");

                circularDependency.Add(jobId);

                result = this.CheckDependency(dependency, this.jobs[dependency], circularDependency) + jobId;
            }

            this.jobs.Remove(jobId);
            return result;
        }

        private void RawDataParser(string rawString)
        {
            var rawJobs = rawString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            this.jobs = new Dictionary<string, string>();

            foreach (var job in rawJobs)
            {
                var splitData = job.Split("=>", StringSplitOptions.RemoveEmptyEntries);
                var jobId = splitData[0].Trim();
                var dependency = splitData.Length > 1 ? splitData[1].Trim() : null;

                if (this.jobs.ContainsKey(jobId))
                    throw new InvalidDataException("Duplicate job found");

                this.jobs.Add(jobId, dependency);
            }
        }
    }
}
