using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algıthm_proje
{
    internal class SqlBaglanti
    {
       

            public SqlConnection baglanti()
            {
            SqlConnection baglanti = new SqlConnection("Data Source=Enes;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=True");
            baglanti.Open();
                return baglanti;
            }
        }
}
