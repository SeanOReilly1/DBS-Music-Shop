using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;

namespace MusicShop.Models
{
    public class DAO
    {
        SqlConnection con;
        public string message = "";

        public DAO()
        {
            con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        }

        public int InsertGuitar(Guitar guitar)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspInsertGuitar", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@GuitarId", guitar.ID);
            cmd.Parameters.AddWithValue("@Name", guitar.Name);
            cmd.Parameters.AddWithValue("@Image", guitar.Image);
            cmd.Parameters.AddWithValue("@Stock", guitar.Stock);
            cmd.Parameters.AddWithValue("@Price", guitar.Price);
            cmd.Parameters.AddWithValue("@ItemType", guitar.ItemType);
            cmd.Parameters.AddWithValue("@Brand", guitar.Brand);
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public List<Guitar> ShowAllGuitars()
        {
            SqlDataReader reader;
            List<Guitar> list = new List<Guitar>();

            SqlCommand cmd = new SqlCommand("uspShowAllGuitars", con);
            cmd.CommandType = CommandType.StoredProcedure;


            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Guitar guitar = new Guitar();
                    guitar.ID = int.Parse(reader["GuitarId"].ToString());
                    guitar.Name = reader["GuitarName"].ToString();
                    guitar.Image = reader["GuitarImage"].ToString();
                    guitar.Price = decimal.Parse(reader["Price"].ToString());
                    guitar.Stock = int.Parse(reader["Stock"].ToString());
                    guitar.ItemType = reader["ItemType"].ToString();
                    guitar.Brand = reader["Brand"].ToString();
                    list.Add(guitar);
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public Guitar ShowOneGuitar(int id)
        {
            SqlDataReader reader;
            Guitar guitar = null;

            SqlCommand cmd = new SqlCommand("uspShowOneGuitar", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuitarId", id);


            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    guitar = new Guitar();
                    guitar.ID = int.Parse(reader["GuitarId"].ToString());
                    guitar.Name = reader["GuitarName"].ToString();
                    guitar.Image = reader["GuitarImage"].ToString();
                    guitar.Price = decimal.Parse(reader["Price"].ToString());
                    guitar.Stock = int.Parse(reader["Stock"].ToString());
                    guitar.ItemType = reader["ItemType"].ToString();
                    guitar.Brand = reader["Brand"].ToString();
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return guitar;
        }

        public void DeleteGuitar(int ID)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("uspDeleteGuitar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@GuitarId";
                paramId.Value = ID;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }

        }

        public int UpdateGuitar(Guitar guitar)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspEditGuitar", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GuitarId", guitar.ID);
            cmd.Parameters.AddWithValue("@Name", guitar.Name);
            cmd.Parameters.AddWithValue("@Image", guitar.Image);
            cmd.Parameters.AddWithValue("@Stock", guitar.Stock);
            cmd.Parameters.AddWithValue("@Price", guitar.Price);
            cmd.Parameters.AddWithValue("@ItemType", guitar.ItemType);
            cmd.Parameters.AddWithValue("@Brand", guitar.Brand);

            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public Admin SelectOneAdmin(string email)
        {
            SqlDataReader reader;
            Admin admin = null;
            SqlCommand cmd = new SqlCommand("uspSelectOneStaff", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    admin = new Admin();
                    admin.Email = reader["StaffEmail"].ToString();
                    admin.Password = reader["Pass"].ToString();
                    
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return admin;
        }

        public int UpdateAdmin(Admin admin)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspUpdateStaff", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@pass", admin.Password);
            

            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public int InsertCustomer(Customer customer)
        {
            int count = 0;
            string password;
            SqlCommand cmd = new SqlCommand("uspInserttblCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custName", customer.Name);
            cmd.Parameters.AddWithValue("@custEmail", customer.Email);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            password = Crypto.HashPassword(customer.Password);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@addressLine1", customer.AddressLine1);
            cmd.Parameters.AddWithValue("@addressLine2", customer.AddressLine2);
            cmd.Parameters.AddWithValue("@city", customer.City);
            cmd.Parameters.AddWithValue("@country", customer.Country);
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public Customer ShowOneCustomer(string email)
        {
            Customer customer = null;
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspShowOneCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custEmail", email);
            

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer = new Customer();
                    customer.Email = reader["CustomerEmail"].ToString();
                    customer.Name = reader["CustomerName"].ToString();
                    customer.Password = reader["Pass"].ToString();
                    customer.Phone = reader["Phone"].ToString();
                    customer.DateRegistered = (DateTime)reader["DateRegistered"];
                    customer.AddressLine1 = reader["AddressLine1"].ToString();
                    customer.AddressLine2 = reader["AddressLine2"].ToString();
                    customer.City = reader["City"].ToString();
                    customer.Country = reader["Country"].ToString();
                }

            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex1)
            {
                message = ex1.Message;
            }
            finally
            {
                con.Close();
            }

            return customer;
        }

        public int DeleteCustomer(string email)
        {
            int counter = 0;
            SqlCommand cmd = new SqlCommand("uspDeleteCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerEmail", email);

            try
            {
                con.Open();
                counter = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return counter;
        }

        public int UpdateCustomer(Customer customer)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspUpdateCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custName", customer.Name);
            cmd.Parameters.AddWithValue("@custEmail", customer.Email);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@pass", customer.Password);
            cmd.Parameters.AddWithValue("@addressLine1", customer.AddressLine1);
            cmd.Parameters.AddWithValue("@addressLine2", customer.AddressLine2);
            cmd.Parameters.AddWithValue("@city", customer.City);
            cmd.Parameters.AddWithValue("@country", customer.Country);
            cmd.Parameters.AddWithValue("@DateRegistered", customer.DateRegistered);
            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public List<Guitar> ShowFilterList(string search)
        {
            string sqlQuery = "select * from [dbo].[Guitars] where Brand like'" + search + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            List<Guitar> list = new List<Guitar>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Guitar
                {
                    ID = Convert.ToInt32(dr["GuitarId"]),
                    Name = Convert.ToString(dr["GuitarName"]),
                    Image = Convert.ToString(dr["GuitarImage"]),
                    Stock = Convert.ToInt32(dr["Stock"]),
                    Price = Convert.ToDecimal(dr["Price"]),
                    ItemType = Convert.ToString(dr["ItemType"]),
                });
            }

            con.Close();
            return list;
        }

        public int InsertOrder(string email)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspInsertOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            

            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }

            finally
            {
                con.Close();
            }
            return count;
        }

        public int GetCurrentOrderID()
        {
            SqlDataReader reader;
            int orderID = -1;
            SqlCommand cmd = new SqlCommand("uspCurrentOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    orderID = int.Parse(reader["ID"].ToString());
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return orderID;
        }

        public int InsertOrderItem(int orderID, OrderItem orderItem)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspInsertOrderItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@guitarID", orderItem.ItemOrdered.ID);
            cmd.Parameters.AddWithValue("@quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@price", orderItem.Price);

            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }

            finally
            {
                con.Close();
            }
            return count;
        }

        public int latestOrderCustomer(string email)
        {
            int orderID = -1;
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspGetMostRecentOrderIDForCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    orderID = int.Parse(reader["OrderID"].ToString());
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return orderID;
        }

        public Order GetOrder(int orderID)
        {
            Order retrievedOrder = new Order();
            retrievedOrder.OrderID = orderID;

            //Get Order Record for OrderID
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspSelectOneOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderID);

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    retrievedOrder.CustomerEmail = reader["CustomerEmail"].ToString();
                    retrievedOrder.OrderDate = (DateTime)reader["DateOfOrder"];
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }

            //Get OrderItem Records for OrderID
            cmd = new SqlCommand("uspGetOrderItems", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderID);
            retrievedOrder.OrderItems = new List<OrderItem>();

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OrderItem oi = new OrderItem();
                    oi.ItemOrdered = new Guitar();
                    oi.Price = decimal.Parse(reader["Price"].ToString());
                    oi.Quantity = int.Parse(reader["Quantity"].ToString());
                    oi.ItemOrdered.ID = int.Parse(reader["GuitarId"].ToString());
                    retrievedOrder.OrderItems.Add(oi);
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return retrievedOrder;
        }

        public List<Admin> SelectAllAdmin()
        {
            SqlDataReader reader;
            List<Admin> list = new List<Admin>();

            SqlCommand cmd = new SqlCommand("uspSelectAllStaff", con);
            cmd.CommandType = CommandType.StoredProcedure;


            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Admin admin = new Admin();
                    admin.Email = reader["StaffEmail"].ToString();
                    admin.Password = reader["Pass"].ToString();
                    
                    list.Add(admin);
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public int InsertAdmin(Admin admin)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand("uspInserttblStaff", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@pass", admin.Password);
            

            try
            {
                con.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }

            finally
            {
                con.Close();
            }
            return count;
        }

        public int DeleteAdminMember(string email)
        {
            int counter = 0;
            SqlCommand cmd = new SqlCommand("uspDeleteOneStaff", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();
                counter = cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return counter;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> list = null;

            //Get Order Record for OrderID
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspGetAllOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    list = new List<Order>();
                }
                while (reader.Read())
                {
                    Order ord = new Order();
                    ord.CustomerEmail = reader["CustomerEmail"].ToString();
                    ord.OrderID = int.Parse(reader["OrderId"].ToString());
                    ord.OrderDate = (DateTime)reader["DateOfOrder"];
                    list.Add(ord);
                }
            }
            catch (SystemException ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public List<Customer> SelectAllCustomers()
        {
            List<Customer> custList = null;
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspShowAllCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    custList = new List<Customer>();
                }

                while (reader.Read())
                {
                    Customer customer = new Customer();
                    customer.Email = reader["CustomerEmail"].ToString();
                    customer.Name = reader["CustomerName"].ToString();
                    customer.Password = reader["Pass"].ToString();
                    customer.Phone = reader["Phone"].ToString();
                    customer.DateRegistered = (DateTime)reader["DateRegistered"];
                    customer.AddressLine1 = reader["AddressLine1"].ToString();
                    customer.AddressLine2 = reader["AddressLine2"].ToString();
                    customer.City = reader["City"].ToString();
                    customer.Country = reader["Country"].ToString();
                    custList.Add(customer);
                }

            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex1)
            {
                message = ex1.Message;
            }
            finally
            {
                con.Close();
            }

            return custList;
        }
    }
}