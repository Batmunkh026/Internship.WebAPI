
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Internship.Utils.DBConnection
{
    public class DBConnection
    {
        OracleConnection conn;
        private string TAG = "DBConnection";
        public DBConnection(string? host, string? schema)
        {
            string conStr = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={schema})));User Id=zspace;Password=zspace123;";
            conn = new OracleConnection(conStr);
        }
        public bool iOpen()
        {
            bool retVal = false;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    retVal = true;
                }
                else
                {
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                //LogWriter._error(TAG, ex.ToString());
            }
            return retVal;
        }
        public bool iClose()
        {
            bool retVal = false;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    retVal = true;
                }
                else
                {
                    conn.Close();
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                //LogWriter._error(TAG, ex.ToString());
            }
            return retVal;
        }
        public bool idbStatOK()
        {
            bool res = false;
            if (iOpen())
            {
                if (iClose())
                {
                    res = true;
                }
            }
            return res;
        }
        public bool idbCommand(string qry)
        {
            bool res;
            try
            {
                iOpen();
                OracleCommand cmd = new OracleCommand(qry, conn);
                int stt = cmd.ExecuteNonQuery();
                iClose();
                res = true;
            }
            catch (Exception ex)
            {
                //LogWriter._error(TAG, string.Format(@"SQL: [{0}], Exception: [{1}]", qry, ex.ToString()));
                res = false;
            }
            return res;
        }
        public bool idbCommandResult(string qry, out int cmres)
        {
            bool res;
            cmres = 0;
            try
            {
                iOpen();
                OracleCommand cmd = new OracleCommand(qry, conn);
                cmres = cmd.ExecuteNonQuery();
                iClose();
                res = true;
            }
            catch (Exception ex)
            {
                //LogWriter._error(TAG, string.Format(@"SQL: [{0}], Exception: [{1}]", qry, ex.ToString()));
                res = false;
            }
            return res;
        }
        public bool iDBCommandRetID(string qry, out int retID)
        {
            bool retVal = false;
            retID = 0;
            try
            {
                iOpen();
                OracleCommand cmd = new OracleCommand(qry, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.Add("ID", OracleDbType.Int32, 10).Direction = ParameterDirection.Output;
                int stt = cmd.ExecuteNonQuery();
                retID = int.Parse(cmd.Parameters["ID"].Value.ToString());
                iClose();
                retVal = true;
            }
            catch (Exception ex)
            {
                //LogWriter._error(TAG, string.Format(@"SQL: [{0}], Exception: [{1}]", qry, ex.ToString()));
                retVal = false;
            }
            return retVal;
        }

        public DataTable getTable(string qry)
        {
            DataTable dt = new DataTable();
            try
            {
                OracleDataAdapter da = new OracleDataAdapter(qry, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //LogWriter._error(TAG, string.Format(@"SQL: {0}, Exception: {1}", qry, ex.ToString()));
                dt.Clear();
            }
            return dt;
        }
        
    }
}
