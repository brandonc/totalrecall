using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;

namespace TotalRecall.MSBuild
{
    class MSBuildLogWrapper : ILogWrapper
    {
        private readonly TaskLoggingHelper helper;

        public void Debug(string message)
        {
            this.helper.LogMessage(message);
        }

        public void Warning(string message)
        {
            this.helper.LogWarning(message);
        }

        public void Info(string message)
        {
            this.helper.LogMessage(message);
        }

        public void Error(string message)
        {
            this.helper.LogError(message);
        }

        public MSBuildLogWrapper(TaskLoggingHelper logHelper)
        {
            this.helper = logHelper;
        }
    }
}
