using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Linq;
using BLL;
using System.Data;
using System.Data.SqlClient;


[Serializable]
public class EmployeeEntry
{
    public EmployeeEntry()
    {
    }
    public EmployeeEntry(string id, string vsimpleName, string vdescription, string employerId)
    {
        ID = id;
        VsimpleName = vsimpleName;
        Vdescription = vdescription;
        EmployerID = employerId;
    }
    public string ID { get; private set; }
    public string EmployerID { get; private set; }
    public string VsimpleName { get; private set; }
    public string Vdescription { get; private set; }

    public void Assign(EmployeeEntry source)
    {
        VsimpleName = source.VsimpleName;
        Vdescription = source.Vdescription;
        EmployerID = source.EmployerID;
    }
}

public static class EmployeeSessionProvider
{
    const string Key = "DxEmployeeSessionProvider";

    static List<EmployeeEntry> CreateData()
    {
        List<EmployeeEntry> result = new List<EmployeeEntry>();
        #region 测试数据
        //result.Add(new EmployeeEntry(GenerateNewID(), "Nancy", "Davolio", new DateTime(1992, 5, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "echo", "aaa", new DateTime(1992, 5, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "echo", "bbb", new DateTime(1992, 5, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "echo", "ccc", new DateTime(1992, 5, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "echo", "ddd", new DateTime(1992, 5, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Andrew", "Fuller", new DateTime(1992, 8, 14), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Janet", "Leverling", new DateTime(1992, 4, 1), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Margaret", "Peacock", new DateTime(1993, 5, 3), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Steven", "Buchanan", new DateTime(1993, 10, 17), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Michael", "Suyama", new DateTime(1993, 10, 17), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Robert", "King", new DateTime(1994, 1, 2), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Laura", "Callahan", new DateTime(1994, 3, 5), GetEmployerId(result)));
        //result.Add(new EmployeeEntry(GenerateNewID(), "Anne", "Dodsworth", new DateTime(1994, 11, 15), GetEmployerId(result)));
        //result.Add(new EmployeeEntry("1", "Nancy", "Davolio", "1"));
        //result.Add(new EmployeeEntry("2", "echo", "aaa", "1"));
        //result.Add(new EmployeeEntry("3", "echo", "bbb", "1"));
        //result.Add(new EmployeeEntry("4", "echo", "ccc", "1"));
        //result.Add(new EmployeeEntry("5", "echo", "ddd", "1"));
        //result.Add(new EmployeeEntry("6", "Andrew", "Fuller", "2"));
        //result.Add(new EmployeeEntry("7", "Janet", "Leverling", "2"));
        //result.Add(new EmployeeEntry("8", "Margaret", "Peacock", "2"));
        //result.Add(new EmployeeEntry("9", "Steven", "Buchanan", "3"));
        //result.Add(new EmployeeEntry("10", "Michael", "Suyama", "3"));
        //result.Add(new EmployeeEntry("11", "Robert", "King", "4"));
        //result.Add(new EmployeeEntry("12", "Laura", "Callahan", "4"));
        //result.Add(new EmployeeEntry("13", "Anne", "Dodsworth", "5"));
        #endregion

        #region 添加树结构数据
        DataTable dt = new SearchManager().DL_HR_CT007BySel();
        if (dt.Rows.Count>0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(new EmployeeEntry(dt.Rows[i]["ccodeID"].ToString(), dt.Rows[i]["vsimpleName"].ToString(), dt.Rows[i]["vdescription"].ToString(), dt.Rows[i]["cpCodeID"].ToString()));
            }
        }
        #endregion

        return result;
    }
    static string GetEmployerId(List<EmployeeEntry> existingEmployees)
    {
        if (!existingEmployees.Any())
            return "";
        return existingEmployees[(int)(existingEmployees.Count / 3)].ID;
    }
    static string GenerateNewID()
    {
        return Guid.NewGuid().ToString();
    }

    public static IEnumerable<EmployeeEntry> Select()
    {
        HttpSessionState session = HttpContext.Current.Session;
        if (session[Key] == null)
            session[Key] = CreateData();
        return (List<EmployeeEntry>)session[Key];
    }
}