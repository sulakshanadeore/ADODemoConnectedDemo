using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ADODemo1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            //ConfigurationManager.ConnectionStrings["projectcnString"].ConnectionString
            SqlConnection cn = new SqlConnection("server=Sulakshana\\sqlexpress;Initial Catalog=northwind;Integrated Security=True");
            
            try
            {
                
                SqlCommand cmd = new SqlCommand("select * from [dbo].ShowDeptData()", cn);
                //Command to execute requires a open connection
                cn.Open();
                //select query in the command use executeReader to execute the command
                SqlDataReader dr = cmd.ExecuteReader();
                //dr.Read();//one row
                //dr.Read();//second row
                //HasRows is a  property whihc return bool to tell whether the tatble has rows or the table is empty
                List<Dept> Deptlist = new List<Dept>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Dept d = new Dept();
                        d.Deptno = Convert.ToInt32(dr["deptno"]);
                        d.Deptname = dr["dname"].ToString();
                        d.DeptLoc = dr["loc"].ToString();
                        Deptlist.Add(d);
                    }
                }

                dgvDeptData.DataSource = Deptlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

            
            
            




        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            //System.Configuration(Right Click References=----Add Reference----Assembly...)
            SqlConnection cn = new SqlConnection("server=Sulakshana\\sqlexpress;Initial Catalog=northwind;Integrated Security=True");

            SqlCommand cmd = new SqlCommand("insert into department values(@deptname,@location)", cn);
            cmd.Parameters.AddWithValue("@deptname", txtdeptname.Text);
            cmd.Parameters.AddWithValue("@location", txtdeptloc.Text);
            cn.Open();
            //Insert/Update/Delete --cmd .ExecuteNonQuery()
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
            MessageBox.Show("Successfully Completed");
            btnShowAll_Click(sender, e);




        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection("server=Sulakshana\\sqlexpress;Initial Catalog=northwind;Integrated Security=True");

            SqlCommand cmd = new SqlCommand("UpdateDept", cn);
            cmd.CommandType = CommandType.StoredProcedure;
 
            cmd.Parameters.AddWithValue("@p_deptno", Convert.ToInt32(txtDeptno.Text));
            cmd.Parameters.AddWithValue("@p_dname", txtdeptname.Text.Trim());
            cmd.Parameters.AddWithValue("@p_loc", txtdeptloc.Text.Trim());
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
            MessageBox.Show("Updated successfully");
            btnShowAll_Click(sender, e);

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection("server=Sulakshana\\sqlexpress;Initial Catalog=northwind;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from department where deptno=" + Convert.ToInt32(txtDeptno.Text),cn);
            cn.Open();
            SqlDataReader dr=cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Dept d = new Dept();
                d.Deptno = Convert.ToInt32(dr["deptno"]);
                d.Deptname = dr["dname"].ToString();
                d.DeptLoc = dr["loc"].ToString();

                txtdeptname.Text = d.Deptname;
                txtdeptloc.Text = d.DeptLoc;

                //txtdeptname.Text= dr["dname"].ToString();
                //txtdeptloc.Text = dr["loc"].ToString();



                MessageBox.Show("Record found successfully");
            }


            cn.Close();
            cn.Dispose();
        }
    }
}
