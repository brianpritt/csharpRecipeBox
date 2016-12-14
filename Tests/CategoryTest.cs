using Xunit;
using System;
using System.Collections.Generic;
using RecipeBox.Objects;
using System.Data;
using System.Data.SqlClient;

namespace  RecipeBox
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Save_SavesCategoryToDB_true()
    {
      //Arrange
      Category newCategory = new Category("Soup", "A liquid form of food");
      Category expectedCategory = new Category("Soup", "A liquid form of food");
      //Act
      newCategory.Save();
      List<Category> allCategories = Category.GetAll();
      //Assert
      Assert.Equal(expectedCategory, newCategory);
    }

    public void Dispose()
    {
      Category.DeleteAll();
    }
  }
}
