using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CSScriptLibrary;

namespace PluginCore.PluginCore.Helpers
{
    public class ScriptHost : MarshalByRefObject
    {
        /// <summary>
        /// Executes the script in a seperate appdomain and then unloads it
        /// NOTE: This is more suitable for one pass processes
        /// </summary>
        public void ExecuteScriptExternal(String script)
        {
            if (!File.Exists(script)) throw new FileNotFoundException();
            using (AsmHelper helper = new AsmHelper(CSScript.Compile(script, null, true), null, true))
            {
                helper.Invoke("*.Execute");
            }
        }

        /// <summary>
        /// Executes the script and adds it to the current app domain
        /// NOTE: This locks the assembly script file
        /// </summary>
        public void ExecuteScriptInternal(String script, Boolean random)
        {
            if (!File.Exists(script)) throw new FileNotFoundException();
            String file = random ? Path.GetTempFileName() : null;
            AsmHelper helper = new AsmHelper(CSScript.Load(script, file, false, null));
            helper.Invoke("*.Execute");
        }

    }
}
