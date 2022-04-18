//========================================================//
//     Copyright 2012, Analytical Graphics, Inc.          //
//========================================================//
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using AGI.VectorGeometryTool.Plugin;
using AGI.STK.Plugin;
using AGI.STKObjects;
using AGI.Attr;
using AGI.Plugin;

namespace Agi.VGT.Vector.Plugin.Examples.CSharp
{
    /// <summary>
    /// Example VGT Vector Plugin
    /// </summary>
    // NOTE: Generate your own Guid using Microsoft's GuidGen.exe
	[Guid("99B79624-9B2F-4b28-B9A7-FF14960B3E2E")]
    // NOTE: Create your own ProgId to match your plugin's namespace and name
    [ProgId("Agi.VGT.Vector.Plugin.Examples.CSharp.Example1")]
    // NOTE: Specify the ClassInterfaceType.None enumeration, so the custom COM Interface 
    // you created is used instead of an autogenerated COM Interface.
    [ClassInterface(ClassInterfaceType.None)]
    public class Example1 :
        IExample1,
        IAgCrdnVectorPlugin,
        IAgUtPluginConfig
    {
        private string m_DisplayName = "CSharp.VectorExample";
        private AgStkPluginSite m_Site;
        private AgStkObjectRoot m_StkRootObject;

        private object m_AgAttrScope;

        private AgCrdnPluginCalcProvider m_CalcToolProvider;
        private AgCrdnPluginProvider m_VectorToolProvider;

        private AgCrdnConfiguredVector m_moonConfiguredVector;
        private AgCrdnConfiguredVector m_sunConfiguredVector;

        public double MyDouble { get; set; }
        public string MyString { get; set; }

        public bool Init(IAgUtPluginSite Site)
        {
            Debug.WriteLine(m_DisplayName + ".Init()", "Entered:");

            m_Site = (AgStkPluginSite)Site;

            if (m_Site != null)
            {
                // Get a pointer to the STK Object Model root object
                m_StkRootObject = (AgStkObjectRoot)m_Site.StkRootObject;
            }

            Debug.WriteLine(m_DisplayName + ".Init()", "Exited:");

            return true;
        }

        public void Register(AgCrdnVectorPluginResultReg Result)
        {
            Debug.WriteLine(m_DisplayName + ".Register()", "Entered:");

            string objPath = "[" + Result.ObjectPath + "]";

            Result.ShortDescription = "Test short Desc: Component created using " + m_DisplayName + " " + objPath;
            Result.LongDescription = "Test long Desc: Component created using " + m_DisplayName + " " + objPath;

            string oPath = Result.ObjectPath;
            string parentPath = Result.ParentPath;
            string grandParentPath = Result.GrandParentPath;

            Debug.WriteLine(objPath + " Register() [objPath=" + oPath + "] [parentPath=" + parentPath + "] [grandParentPath=" + grandParentPath + "]");

            Debug.WriteLine(m_DisplayName + ".Register()", "Exited:");
        }

        public bool Evaluate(AgCrdnVectorPluginResultEval Result)
        {
            if (m_moonConfiguredVector != null && m_sunConfiguredVector != null)
            {
                double x1 = 0;
                double y1 = 0;
                double z1 = 0;
                m_moonConfiguredVector.CurrentValue(Result, ref x1, ref y1, ref z1);

                double x2 = 0;
                double y2 = 0;
                double z2 = 0;
                m_sunConfiguredVector.CurrentValue(Result, ref x2, ref y2, ref z2);

                // For this example, the vector is the average vector of the sun and the moon.
                Result.SetVectorComponents((x2 + x1) / 2, (y2 + y1) / 2, (z2 + z1) / 2);
            }

            return true;
        }

        public void Free()
        {
            Debug.WriteLine(m_DisplayName + ".Free()", "Entered:");

            m_CalcToolProvider = null;
            m_moonConfiguredVector = null;
            m_sunConfiguredVector = null;

            Debug.WriteLine(m_DisplayName + ".Free()", "Exited:");
        }

        public bool Reset(AgCrdnVectorPluginResultReset Result)
        {
            string objPath = "[" + Result.ObjectPath + "]";

            Debug.WriteLine(m_DisplayName + ".Reset()", "Entered:");

            m_CalcToolProvider = Result.CalcToolProvider;
            m_VectorToolProvider = Result.VectorToolProvider;

            m_moonConfiguredVector = m_VectorToolProvider.ConfigureVector("Moon", "<MyObject>", "ICRF", "<MyObject>");
            m_sunConfiguredVector = m_VectorToolProvider.ConfigureVector("Sun", "<MyObject>", "ICRF", "<MyObject>");

            Debug.WriteLine(m_DisplayName + ".Reset()", "Exited:");

            return true;
        }

        public object GetPluginConfig(AGI.Attr.AgAttrBuilder pAttrBuilder)
        {
            Debug.WriteLine(m_DisplayName + ".GetPluginConfig()", "Entered:");

            if (m_AgAttrScope == null)
            {
                m_AgAttrScope = pAttrBuilder.NewScope();

                pAttrBuilder.AddStringDispatchProperty(m_AgAttrScope, "MyString", "A string", "MyString", (int)AgEAttrAddFlags.eAddFlagReadOnly);
                pAttrBuilder.AddDoubleDispatchProperty(m_AgAttrScope, "MyDouble", "A double", "MyDouble", (int)AgEAttrAddFlags.eAddFlagNone);
            }

            Debug.WriteLine(m_DisplayName + ".GetPluginConfig()", "Exited:");

            return m_AgAttrScope;
        }

        public void VerifyPluginConfig(AgUtPluginConfigVerifyResult pPluginCfgResult)
        {
            Debug.WriteLine(m_DisplayName + ".VerifyPluginConfig()", "Entered:");

            pPluginCfgResult.Result = true;
            pPluginCfgResult.Message = "Ok";

            Debug.WriteLine(m_DisplayName + ".VerifyPluginConfig()", "Exited:");
        }
    }
}
//========================================================//
//     Copyright 2012, Analytical Graphics, Inc.          //
//========================================================//