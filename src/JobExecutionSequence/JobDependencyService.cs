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
        /// Constructor
        /// </summary>
        /// <param name="rawString">The raw input</param>
        public JobDependencyService(string rawString)
        {
            this.RawDataParser(rawString);
        }

        private void RawDataParser(string rawString)
        {
            var rawJobs = rawString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            this.jobs = new Dictionary<string, string>();

            foreach (var job in rawJobs)
            {
                var splitData = job.Split("=>", StringSplitOptions.RemoveEmptyEntries);
                this.jobs.Add(splitData[0], splitData.Length > 1 ? splitData[1] : null);
            }
        }
    }
}
