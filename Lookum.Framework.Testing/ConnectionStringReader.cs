﻿using System;
using System.Xml;

namespace Lookum.Framework.Testing
{
    class ConnectionStringReader
    {
        public static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }


        public static string GetOleDb()
        {
            return Get("OleDb");
        }

        public static string GetAdomd()
        {
            return Get("Adomd");
        }

        public static string GetSqlClient()
        {
            return Get("SqlClient");
        }

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                return "ConnectionString.user.config";
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                return "ConnectionString.config";
            }
            return "";
        }

        internal static string GetAdomdTabular()
        {
            return Get("AdomdTabular");
        }


        internal static string GetLocalSqlClient()
        {
            return Get("LocalSqlClient");
        }
        internal static string GetReportServerDatabase()
        {
            return Get("ReportServerDatabase");
        }

        internal static string GetIntegrationServerDatabase()
        {
            return Get("IntegrationServerDatabase");
        }

    }
}
