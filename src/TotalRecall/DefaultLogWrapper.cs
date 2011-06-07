using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
using log4net;
using TotalRecall.Configuration;
using System.IO;

namespace TotalRecall
{
    class DefaultLogWrapper : ILogWrapper
    {
        private readonly string logname;

        public void Debug(string message)
        {
            LogManager.GetLogger(logname).Debug(message);
        }

        public void Warning(string message)
        {
            LogManager.GetLogger(logname).Warn(message);
        }

        public void Info(string message)
        {
            LogManager.GetLogger(logname).Info(message);
        }

        public void Error(string message)
        {
            LogManager.GetLogger(logname).Error(message);
        }

        public DefaultLogWrapper(string logname, IConfig config)
        {
            this.logname = logname;

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Clear();

            TraceAppender tracer = new TraceAppender();
            PatternLayout patternLayout = new PatternLayout();

            patternLayout.ConversionPattern = "%t (%-5level) - %m%n";
            patternLayout.ActivateOptions();

            tracer.Layout = patternLayout;
            tracer.ActivateOptions();
            hierarchy.Root.AddAppender(tracer);

            RollingFileAppender roller = new RollingFileAppender();
            roller.Layout = patternLayout;
            roller.AppendToFile = true;
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.MaxSizeRollBackups = 4;
            roller.MaximumFileSize = "100KB";
            roller.StaticLogFileName = true;
            roller.File = Path.Combine(config.IndexFolder, "totalrecall-" + this.logname + ".log");
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }
    }
}
