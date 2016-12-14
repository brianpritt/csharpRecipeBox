using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox.Objects
{
  public class Category
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}

    public Category(string name, string description, int id = 0)
    {
      this.Id = id;
      this.Name = name;
      this.Description = description;
    }
    public int GetId()
    {
      return Id;
    }
    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool nameEquality = this.Name == newCategory.Name;
        bool descriptionEquality = this.Description == newCategory.Description;

        return (nameEquality && descriptionEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name, description) OUTPUT INSERTED.id VALUES (@Name, @Description);", conn);

      cmd.Parameters.AddWithValue("@Description", this.Description);
      cmd.Parameters.AddWithValue("@Name", this.Name);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static List<Category> GetAll()
    {
      List<Category> allCategorys = new List<Category>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        string CategoryDescription = rdr.GetString(2);
        Category newCategory = new Category(CategoryName, CategoryDescription, CategoryId);
        allCategorys.Add(newCategory);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCategorys;
    }
    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT name, description FROM categories WHERE id = @id;", conn);
      cmd.Parameters.AddWithValue("@id", id);
      SqlDataReader rdr = cmd.ExecuteReader();

      string findName = null;  string findDescription = null;
      while(rdr.Read())
      {
         findName = rdr.GetString(0);
         findDescription = rdr.GetString(1);

       }
      Category foundCategory = new Category(findName, findDescription);
      //foundRecipe.GetIngredientsFromDB();

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return foundCategory;
    }
    public static void DeleteAll()
		{
		  SqlConnection conn = DB.Connection();
		  conn.Open();
		  SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
		  cmd.ExecuteNonQuery();
		  conn.Close();
		}
  }
}
