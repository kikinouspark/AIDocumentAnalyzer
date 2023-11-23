using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace DocumentRecognizerTest.Service
{
    public class IDSourceCheck
    {

        private IWebHostEnvironment _env;

        public IDSourceCheck(IWebHostEnvironment env)
        {
            _env = env;
        }

        public static string CheckSourceList(string ID)
        {
            List<string> DB_EIN_LIST = new List<string> {
                "13-2653281",
                "13-2730828",
                "13-3347003",
                "13-4941247",
                "13-6065491",
                "35-2420193",
                "52-0945480"
            };

            foreach (string ids in DB_EIN_LIST)
            {
                if (ID == ids)
                {
                    return "DB";
                }
            }
            return null;
        }
    }
}
