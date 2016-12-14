using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox.Objects
{
  public class Recipe
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public int Rating {get; set;}
    public string Author {get; set;}
    public string Description {get; set;}
    private Dictionary<string, string> _ingredients = new Dictionary<string, string>(){};
    private List<string> _instructions = new List<string>{};

    public Recipe(string name, string author, string description, int rating = 0, int id = 0)
    {
      this.Id = id;
      this.Name = name;
      this.Rating = rating;
      this.Author = author;
      this.Description = description;
    }
    public Dictionary<string, string> GetIngredients()
    {
      return _ingredients;
    }
    public List<string> GetInstructions()
    {
      return _instructions;
    }
    public void SetIngredients(Dictionary<string, string> ingredients)
    {
      _ingredients = ingredients;
    }
    public void SetInstructions(List<string> instructions)
    {
      _instructions = instructions;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if (!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool nameEquality = this.Name == newRecipe.Name;
        bool descriptionEquality = this.Description == newRecipe.Description;
        bool authorEquality = this.Author == newRecipe.Author;
        bool ratingEquality = this.Rating == newRecipe.Rating;

        return (nameEquality && descriptionEquality && authorEquality && ratingEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, description, rating, author) OUTPUT INSERTED.id VALUES (@Name, @Description, @Rating, @Author);", conn);

      cmd.Parameters.AddWithValue("@Name", this.Name);
      cmd.Parameters.AddWithValue("@Description", this.Description);
      cmd.Parameters.AddWithValue("@Rating", this.Rating);
      cmd.Parameters.AddWithValue("@Author", this.Author);

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

    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int RecipeId = rdr.GetInt32(0);
        string RecipeName = rdr.GetString(1);
        string RecipeDescription = rdr.GetString(2);
        int RecipeRating = rdr.GetInt32(3);
        string RecipeAuthor = rdr.GetString(4);
        Recipe newRecipe = new Recipe(RecipeName, RecipeAuthor, RecipeDescription,RecipeRating, RecipeId);
        allRecipes.Add(newRecipe);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRecipes;
    }
    public void SaveIngredients()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      foreach(var ingredient in this.GetIngredients())
      {
        SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (recipe_id, name, amount) VALUES (@RecipeId, @Name, @Amount);", conn);
        cmd.Parameters.AddWithValue("@RecipeId", this.Id);
        cmd.Parameters.AddWithValue("@Name", ingredient.Key);
        cmd.Parameters.AddWithValue("@Amount", ingredient.Value);
        cmd.ExecuteNonQuery();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public Dictionary<string,string> GetIngredientsFromDB()
    {
      Dictionary<string,string> allIngredients = new Dictionary<string,string>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE recipe_id = @RecipeId;", conn);
      cmd.Parameters.AddWithValue("@RecipeId", this.Id);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        allIngredients.Add(rdr.GetString(2), rdr.GetString(3));
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allIngredients;
    }
    public static void DeleteAll()
		{
		  SqlConnection conn = DB.Connection();
		  conn.Open();
		  SqlCommand cmd = new SqlCommand("DELETE FROM recipes; DELETE FROM ingredients", conn);
		  cmd.ExecuteNonQuery();
		  conn.Close();
		}
  }
}
