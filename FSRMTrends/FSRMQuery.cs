using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Management.Infrastructure;


namespace FSRMTrends
{
    class FSRMQuery
    {
        public ArrayList getFSRMData() {
            //Set WMI Namespace and the query to run to acquire quotas
            string Namespace = @"Root\Microsoft\Windows\FSRM";
            string Query = "SELECT * FROM MSFT_FSRMQuota";

            //Open session and run query
            CimSession FSRMSession = CimSession.Create(SQLDataSource.Default.FSRMServerName);
            
            IEnumerable<CimInstance> queryInstance = FSRMSession.QueryInstances(Namespace, "WQL", Query);

            //convert each CimInstance returned from WMI auery into a dictionary of key/value pairs
            ArrayList queryProperties = new ArrayList();

            foreach (CimInstance queryProperty in queryInstance)
            {
                queryProperties.Add(queryProperty.CimInstanceProperties.ToDictionary(k => k.Name, v => v.Value));
            }

            //return requested data as an array of dictionaries
            return queryProperties;
        }
    }
}
