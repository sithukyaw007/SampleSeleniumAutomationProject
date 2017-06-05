using AutomationFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutomationFramework.Base.Browser;

namespace UniSuperTestScenarios.AQATestHook
{
    public class HookInitialize : TestInitializeHooks
    {
        /*public HookInitialize() : base(BrowserType.Electron)
        {
            InitSettings();
        }*/

        public HookInitialize() 
        {
            InitSettings();
        }



    }
}
