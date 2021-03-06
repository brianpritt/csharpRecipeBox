using Xunit;
using System;
using System.Collections.Generic;
using RecipeBox.Objects;
using System.Data;
using System.Data.SqlClient;

namespace  RecipeBox
{
  public class RecipeTest : IDisposable
  {
    public RecipeTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Save_SavesRecipeToDB_true()
    {
      //Arrange
      Recipe newRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      Recipe expectedRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      //Act
      newRecipe.Save();
      List<Recipe> allRecipe = Recipe.GetAll();
      newRecipe = allRecipe[0];
      //Assert
      Assert.Equal(expectedRecipe, newRecipe);
    }
    [Fact]
    public void SaveIngredients_SavesIngredientsToDB_true()
    {
      Recipe newRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      Dictionary<string, string> ingredients = new Dictionary<string, string>(){{"chicken", "1lb"}, {"rice", "1lb"}, {"carrots", "1/2c"}, {"peas","1/4c"}, {"onions", "1/4c"}};
      newRecipe.SetIngredients(ingredients);
      newRecipe.SaveIngredients();
      Dictionary<string, string> actualIngredients = newRecipe.GetIngredientsFromDB();
      Assert.Equal(5, actualIngredients.Count);
    }
    [Fact]
    public void Find_FindsRecipeInRecipes_true()
    {
      //Arrange
      Recipe newRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      newRecipe.Save();
      Dictionary<string, string> ingredients = new Dictionary<string, string>(){{"chicken", "1lb"}, {"rice", "1lb"}, {"carrots", "1/2c"}, {"peas","1/4c"}, {"onions", "1/4c"}};
      newRecipe.SetIngredients(ingredients);
      newRecipe.SaveIngredients();
      //Act
      Recipe findRecipe = Recipe.Find(newRecipe.GetId());

      //Assert
      Assert.Equal(newRecipe, findRecipe);
    }
    [Fact]
    public void SaveInstructions_SavesInstructionsToDB()
    {
      //Arrange
      Recipe newRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      List<string> instructions = new List<string>(){"Put Chicken In", "Cook"};
      newRecipe.Save();
      newRecipe.SetInstructions(instructions);
      newRecipe.SaveInstructions();
      //Act
      List<string> actualInstructions = newRecipe.GetInstructionsFromDB();
      //Assert
      Assert.Equal(2,actualInstructions.Count);

    }
    [Fact]
    public void EditRecipe_EditsRecipeElements_true()
    {
      Recipe newRecipe = new Recipe("Chicken Soup", "Lina", "Soup");
      newRecipe.Save();
      newRecipe.EditRecipe("Tomato Soup", "Brian", "Soup");
      Recipe foundRecipe = Recipe.Find(newRecipe.Id);
      Assert.Equal("Tomato Soup", foundRecipe.Name);
    }

    public void Dispose()
    {
      Recipe.DeleteAll();
    }

  }
}
