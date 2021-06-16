/*
** 
**  electrifier
** 
**  Copyright 2017-19 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/

using NLog;
using NLog.Config;
using NLog.Targets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace electrifier.Core
{
    public static class LogContext
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Initialize()
        {
            LoggingConfiguration logConfig = new LoggingConfiguration();

            FileTarget fileTarget = new FileTarget("logfile")
            {
                FileName = "logfile.txt"
            };

            logConfig.AddRuleForAllLevels(fileTarget);

            NLog.LogManager.Configuration = logConfig;

            Logger.Info("Logging enabled");
        }

        //[Conditional("DEBUG")]
        internal static void Trace(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            //            string fullMessage = "@ '" + shorten(filePath) + "' in " + memberName;

            Logger.Trace($" {memberName} in {shorten(filePath)}");
        }

        //[Conditional("DEBUG")]
        internal static void Debug(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {

            Logger.Debug($" {memberName}: '{message}' in {shorten(filePath)}");
        }

        //[Conditional("DEBUG")]
        internal static void Warn(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            Logger.Warn($" {memberName}: '{message}' in {shorten(filePath)}");
        }

        //[Conditional("DEBUG")]
        internal static void Error(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Logger.Error($" {memberName}: '{message}' in {shorten(filePath)} #{lineNumber}");
        }

        private static string shorten(string fullFileName) => fullFileName.Substring(fullFileName.IndexOf(@"\src\") + 5);
    }
}
