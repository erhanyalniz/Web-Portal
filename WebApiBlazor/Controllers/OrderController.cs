using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using WCFJob;

namespace WebApiBlazor.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {


        [HttpGet(Name = "GetOrder")]
        public IEnumerable<WCFJob.Order1> GetOrder()
        {
            DataTable OrderRolesTable = new DataTable();

            string queryRoles = $"SELECT * FROM tbl_Orders";

            using (var connection = new SqlConnection("Data Source=ERN-EMOBITS-RPL\\COMMUNITY;User ID=sa;Password=e-Lips2009;Initial Catalog=ErnieTMS"))
            {
                using (var command = new SqlCommand(queryRoles, connection))
                {
                    connection.Open();
                    //command.Parameters.AddWithValue("@RolID", RolID);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        OrderRolesTable.Load(reader);


                    }
                }
            }

            var ListOfOrders = new List<WCFJob.Order1>();

            for (int i = 0; i < OrderRolesTable.Rows.Count; i++)
            {
                var order = new WCFJob.Order1();
                order.OrderNo = OrderRolesTable.Rows[i]["OrderNr"].ToString();
                if (!string.IsNullOrEmpty(order.OrderNo))
                {
                    order.Units = GetUnits(order.OrderNo).ToArray();
                }
                ListOfOrders.Add(order);
            }

            return ListOfOrders;
        }

        private List<Unit> GetUnits(string orderNr)
        {
            DataTable UnitTable = new DataTable();
            string query = $"SELECT * FROM tbl_OrdersRegels WHERE OrderLink = @OrderNr";

            using (var connection = new SqlConnection("Data Source=ERN-EMOBITS-RPL\\COMMUNITY;User ID=sa;Password=e-Lips2009;Initial Catalog=ErnieTMS")) {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderNr", orderNr);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        UnitTable.Load(reader);
                    }

                }
            }
            var listOfUnits = new List<Unit>();
            for (int i = 0; i < UnitTable.Rows.Count; i++)
            {
                var unit = new Unit();
                unit.UnitNummer = UnitTable.Rows[i]["Nummer"].ToString();
                unit.OrderRegelState = UnitTable.Rows[i]["Status"].ToString();
                unit.TypeUnit = UnitTable.Rows[i]["Type"].ToString();
                unit.Rederij = UnitTable.Rows[i]["Rederij"].ToString();
                listOfUnits.Add(unit);
            }
            return listOfUnits;
        }
    }
}
