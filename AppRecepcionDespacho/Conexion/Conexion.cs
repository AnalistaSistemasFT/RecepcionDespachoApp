using System;
using System.Collections.Generic;
using System.Text;
using AppRecepcionDespacho.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace AppRecepcionDespacho.Conexion
{
    public class Conex
    {

        //  <add name = "SQLSERVER5" connectionString="Server=10.10.100.26;Database=FTODO;Integrated Security=No; Uid=sa;Password=Passw0rd"
        //providerName="System.Data.SqlClient" />
        static string cadenaConexion_1 = @"Data Source=10.10.100.26;Initial Catalog=FTODO;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
        //static string cadenaConexion = @"Data Source=10.10.100.26;Initial Catalog=LYBK;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
        static string cadenaConexion = @"Data Source=192.168.0.200;Initial Catalog=LYBK;Persist Security Info=True;user id=sa;password=PlantaV.;Connect Timeout=160";
        //static string cadenaConexion_1 = @"Data Source=1192.168.0.200;Initial Catalog=FTODO;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
        protected string CadenaConexion = string.Empty;
        protected SqlTransaction Transaccion;
        protected string sConnName = string.Empty;

        public DataTable ejecutarConsultaLogin(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public DataTable ejecutarConsulta(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public DataTable ejecutarConsultaftodo(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion_1))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public DataTable ejecutarConsulta1(string sentencia)
        {
            //List<Parametro> listaEmpleados = new List<Parametro>();
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlCommand comando = new SqlCommand(sentencia, con))
                {
                    using (SqlDataAdapter reader = new SqlDataAdapter(comando))
                    {
                        reader.Fill(data);
                    }
                }
                con.Close();
                return data;
            }
        }
        public bool ejecutarconsultaEnviar(string sentencia)
        {
            bool retornar = false;
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                try
                {
                    using (SqlCommand comando = new SqlCommand(sentencia, con))
                    {
                        comando.ExecuteNonQuery();
                        retornar = true;
                    }
                }
                catch (Exception ee)
                {
                    retornar = false;
                    Console.WriteLine("###################### = " + ee.ToString());
                }
                con.Close();
            }
            return retornar;

        }
        public bool GuardarDespProducto(DespProductos _despProductos)
        {
            string sql = "INSERT INTO tblDespProductos (DespachoId, ProductoId, ItemId, Fecha, Status, Cantidad, Peso, CeldaId, SucursalId, ItemFId, Piezas, Metros, Colada) " +
            "values (@DespachoId, @ProductoId, @ItemId, @Fecha, @Status, @Cantidad ,@Peso, @CeldaId, @SucursalId, @ItemFId, @Piezas, @Metros, @Colada )";
            bool response;

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();

                using (SqlCommand comando = new SqlCommand(sql, con))
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("es-ES");
                        comando.Parameters.Add("@DespachoId", SqlDbType.NVarChar).Value = _despProductos.DespachoId;
                        comando.Parameters.Add("@ProductoId", SqlDbType.NVarChar).Value = _despProductos.ProductoId;
                        comando.Parameters.Add("@ItemId", SqlDbType.NVarChar).Value = _despProductos.ItemId;
                        comando.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = _despProductos.Fecha;
                        comando.Parameters.Add("@Status", SqlDbType.NVarChar).Value = _despProductos.Status;
                        comando.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = _despProductos.Cantidad;
                        comando.Parameters.Add("@Peso", SqlDbType.Decimal).Value = _despProductos.Peso;
                        comando.Parameters.Add("@CeldaId", SqlDbType.NVarChar).Value = _despProductos.CeldaId;
                        comando.Parameters.Add("@SucursalId", SqlDbType.Int).Value = _despProductos.SucursalId;
                        comando.Parameters.Add("@ItemFId", SqlDbType.NVarChar).Value = _despProductos.ItemFId;
                        comando.Parameters.Add("@Piezas", SqlDbType.Int).Value = _despProductos.Piezas;
                        comando.Parameters.Add("@Metros", SqlDbType.Decimal).Value = _despProductos.Metros;
                        comando.Parameters.Add("@Colada", SqlDbType.NVarChar).Value = _despProductos.Colada;
                        comando.CommandType = CommandType.Text;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("####################################= " + err.Message);
                    }
                    if (comando.ExecuteNonQuery() == 1)
                        response = true;
                    else
                        response = false;
                }

                con.Close();
            }
            return response;
        }
        string ObtenerString()
        {
            return cadenaConexion;
        }
        protected System.Data.SqlClient.SqlConnection conectar()
        {
            //CadenaConexion = ObtenerString();
            CadenaConexion = string.Format(CadenaConexion);
            System.Data.SqlClient.SqlConnection obconexion = new System.Data.SqlClient.SqlConnection(CadenaConexion);
            obconexion.Open();
            return obconexion;
        }
        protected System.Data.SqlClient.SqlTransaction IniciarTransaccion()
        {
            System.Data.SqlClient.SqlConnection obconexion = conectar();
            return obconexion.BeginTransaction();
        }
        protected System.Data.SqlClient.SqlTransaction inicio_tr(string sql)
        {
            if (sql == "FTODO")
                CadenaConexion = cadenaConexion_1;//@"Data Source=10.10.100.26;Initial Catalog=FTODO;Persist Security Info=True;user id=sa;password=Passw0rd;Connect Timeout=160";
            else
                CadenaConexion = cadenaConexion;
            return IniciarTransaccion();
        }
        //protected System.Data.DataSet consultar(string csql)
        //{
        //    System.Data.Odbc.OdbcTransaction obtransaccion = IniciarTransaccion();
        //    obtransaccion.Commit();
        //    return consultar(csql, obtransaccion);
        //}
        protected System.Data.DataSet consultar(string csql)
        {
            System.Data.SqlClient.SqlTransaction obtransaccion = IniciarTransaccion();
            System.Data.DataSet ConjuntoDatos = consultar(csql, obtransaccion);
            //esto le agregue de nuevo cerrar conexiones
            obtransaccion.Commit();
            obtransaccion.Dispose();
            //return consultar(csql, obtransaccion);
            return ConjuntoDatos;
        }
        protected System.Data.DataSet consultar(string csql, System.Data.SqlClient.SqlTransaction obtransaccion)
        {
            System.Data.SqlClient.SqlDataAdapter adaptador = new System.Data.SqlClient.SqlDataAdapter(csql, obtransaccion.Connection);
            adaptador.SelectCommand.Transaction = obtransaccion;
            System.Data.DataSet ConjuntoDatos = new System.Data.DataSet();
            adaptador.Fill(ConjuntoDatos);
            return ConjuntoDatos;
        }
        protected int ejecutar(ref string er, string csql)
        {
            System.Data.SqlClient.SqlTransaction obtransaccion = IniciarTransaccion();
            int i = ejecutar(ref er, csql, obtransaccion);
            obtransaccion.Commit();
            return i;
        }
        protected int ejecutar(ref string er, string csql, System.Data.SqlClient.SqlTransaction obtransaccion)
        {
            try
            {
                System.Data.SqlClient.SqlCommand comando = new System.Data.SqlClient.SqlCommand(csql, obtransaccion.Connection);
                comando.Transaction = obtransaccion;
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                er = ex.Message;
                return 0;
            }
        }
        //--------------------------------------------------------------------------
        //consultar que retornan datatables
        protected System.Data.DataTable consultarD(string csql)
        {
            System.Data.SqlClient.SqlTransaction obtransaccion = IniciarTransaccion();
            return consultarD(csql, obtransaccion);
        }
        protected System.Data.DataTable consultarD(string csql, System.Data.SqlClient.SqlTransaction obtransaccion)
        {
            System.Data.SqlClient.SqlDataAdapter adaptador = new System.Data.SqlClient.SqlDataAdapter(csql, obtransaccion.Connection);
            adaptador.SelectCommand.Transaction = obtransaccion;
            System.Data.DataTable ConjuntoDatos = new System.Data.DataTable();
            adaptador.Fill(ConjuntoDatos);
            return ConjuntoDatos;
        }




        protected System.Collections.Hashtable ColComandos = new System.Collections.Hashtable();

        public int Ejecutar(string ProcedimientoAlmacenado, System.Object[] Args)
        {
            SqlCommand Com = Comando(ProcedimientoAlmacenado);
            CargarParametros(Com, Args);
            int Resp = Com.ExecuteNonQuery();
            for (int i = 0; i < Com.Parameters.Count; i++)
            {
                SqlParameter Par = (SqlParameter)Com.Parameters[i];
                if (Par.Direction == ParameterDirection.InputOutput || Par.Direction == ParameterDirection.Output)
                    //if (Par.Direction == ParameterDirection.InputOutput || Par.Direction == ParameterDirection.ReturnValue)
                    Resp = int.Parse(Par.Value.ToString());
            }
            return Resp;
        }

        public int Ejecutar(string ProcedimientoAlmacenado, System.Object[] Args, SqlTransaction tran)
        {
            SqlCommand Com = Comando(ProcedimientoAlmacenado, tran);
            CargarParametros(Com, Args);
            int Resp = Com.ExecuteNonQuery();
            for (int i = 0; i < Com.Parameters.Count; i++)
            {
                SqlParameter Par = (SqlParameter)Com.Parameters[i];
                if (Par.Direction == ParameterDirection.InputOutput || Par.Direction == ParameterDirection.Output)
                    Resp = int.Parse(Par.Value.ToString());
            }
            return Resp;
        }

        protected void CargarParametros(System.Data.SqlClient.SqlCommand Com, System.Object[] Args)
        {
            int Limite = Com.Parameters.Count;
            for (int i = 1; i < Com.Parameters.Count; i++)
            {
                System.Data.SqlClient.SqlParameter P = (System.Data.SqlClient.SqlParameter)Com.Parameters[i];
                if (i <= Args.Length)
                    P.Value = Args[i - 1];
                else
                    P.Value = null;
            }
        }

        protected SqlCommand Comando(string ProcedimientoAlmacenado, System.Data.SqlClient.SqlTransaction tran)
        {
            System.Data.SqlClient.SqlCommand Com;
            if (ColComandos.Contains(ProcedimientoAlmacenado))
                Com = (System.Data.SqlClient.SqlCommand)ColComandos[ProcedimientoAlmacenado];
            else
            {
                SqlConnection Con2 = new SqlConnection("Persist Security Info=False;User ID=sa;Password=Passw0rd;Initial Catalog=sisadm2;Server=192.168.1.20");
                Con2.Open();
                Com = new SqlCommand(ProcedimientoAlmacenado, Con2);
                Com.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(Com);
                Con2.Close();
                Con2.Dispose();
                ColComandos.Add(ProcedimientoAlmacenado, Com);

            }
            Com.Connection = tran.Connection;
            Com.Transaction = tran;
            return (SqlCommand)Com;
        }

        protected SqlCommand Comando(string NombreProcedimiento)
        {
            SqlCommand Com;
            if (ColComandos.Contains(NombreProcedimiento))
                Com = (SqlCommand)ColComandos[NombreProcedimiento];
            else
            {
                SqlConnection Con2 = new SqlConnection(CadenaConexion);
                Con2.Open();
                Com = new SqlCommand(NombreProcedimiento, Con2);
                Com.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(Com);
                Con2.Close();
                Con2.Dispose();
                ColComandos.Add(NombreProcedimiento, Com);
                Com.Connection = this.conectar();
                Com.Transaction = Transaccion;
            }
            return Com;
        }

    }
}
