using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intuit.Ipp.Core.Configuration;


[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace RESTFULAPI_Automation_2._1
{
    [TestClass]
    public class UnitTest1
    {

    }
}