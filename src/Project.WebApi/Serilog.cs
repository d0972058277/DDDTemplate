using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi
{
    public class Serilog
    {
        public IEnumerable<SerilogWriteTo> WriteTo { get; set; }
        public class SerilogWriteTo
        {
            public string Name { get; set; }
            public Dictionary<string, object> Args { get; set; }
        }

        public bool TryGetSeqServerUrl(out string serverUrl)
        {
            serverUrl = null;
            var writeToSeq = WriteTo.Where(w => w.Name == "Seq").SingleOrDefault();
            if (writeToSeq is null)
            {
                return false;
            }
            else
            {
                var argServerUrl = writeToSeq.Args.Where(a => a.Key == "ServerUrl").Select(a => a.Value.ToString()).SingleOrDefault();
                if (string.IsNullOrWhiteSpace(argServerUrl))
                {
                    return false;
                }
                else
                {
                    serverUrl = argServerUrl;
                    return true;
                }
            }
        }
    }
}