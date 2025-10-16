using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cookbook.Infrastructure;

public class CookbookContext : DbContext
{
    private readonly IConfiguration? _configuration;

    private readonly Category[] _newCategories =
    [
        new() { Name = "Main Course" },
        new() { Name = "Side dish" },
        new() { Name = "Dessert" },
        new() { Name = "Drink" },
        new() { Name = "Appetizer" },
        new() { Name = "Pasta" },
        new() { Name = "Baking" },
        new() { Name = "Salad" },
        new() { Name = "Breakfast" }
    ];

    private readonly Ingredient[] _newIngredients =
    [
        new() { Name = "Flour" },
        new() { Name = "Sugar" },
        new() { Name = "Salt" },
        new() { Name = "Butter" },
        new() { Name = "Eggs" },
        new() { Name = "Milk" },
        new() { Name = "Yeast" },
        new() { Name = "Water" },
        new() { Name = "Olive Oil" },
        new() { Name = "Vinegar" },
        new() { Name = "Baking Powder" },
        new() { Name = "Baking Soda" },
        new() { Name = "Vanilla Extract" },
        new() { Name = "Cinnamon" },
        new() { Name = "Nutmeg" },
        new() { Name = "Pepper" },
        new() { Name = "Garlic" },
        new() { Name = "Onion" },
        new() { Name = "Tomato" },
        new() { Name = "Lemon" },
        new() { Name = "Lime" },
        new() { Name = "Orange" },
        new() { Name = "Apple" },
        new() { Name = "Banana" },
        new() { Name = "Strawberry" },
        new() { Name = "Blueberry" },
        new() { Name = "Raspberry" },
        new() { Name = "Chocolate" },
        new() { Name = "Cocoa Powder" },
        new() { Name = "Honey" },
        new() { Name = "Maple Syrup" },
        new() { Name = "Soy Sauce" },
        new() { Name = "Mustard" },
        new() { Name = "Ketchup" },
        new() { Name = "Mayonnaise" },
        new() { Name = "Basil" },
        new() { Name = "Oregano" },
        new() { Name = "Thyme" },
        new() { Name = "Rosemary" },
        new() { Name = "Parsley" },
        new() { Name = "Cilantro" },
        new() { Name = "Dill" },
        new() { Name = "Chives" },
        new() { Name = "Ginger" },
        new() { Name = "Turmeric" },
        new() { Name = "Paprika" },
        new() { Name = "Cumin" },
        new() { Name = "Cayenne Pepper" },
        new() { Name = "Chicken" },
        new() { Name = "Beef" },
        new() { Name = "Pork" },
        new() { Name = "Fish" },
        new() { Name = "Shrimp" },
        new() { Name = "Salmon" },
        new() { Name = "Tuna" },
        new() { Name = "Rice" },
        new() { Name = "Pasta" },
        new() { Name = "Bread" },
        new() { Name = "Potatoes" },
        new() { Name = "Carrots" },
        new() { Name = "Celery" },
        new() { Name = "Broccoli" },
        new() { Name = "Spinach" },
        new() { Name = "Lettuce" },
        new() { Name = "Cucumber" },
        new() { Name = "Bell Pepper" },
        new() { Name = "Corn" },
        new() { Name = "Peas" },
        new() { Name = "Beans" },
        new() { Name = "Lentils" },
        new() { Name = "Oats" },
        new() { Name = "Almonds" },
        new() { Name = "Walnuts" },
        new() { Name = "Peanuts" },
        new() { Name = "Cashews" },
        new() { Name = "Sunflower Seeds" },
        new() { Name = "Sesame Seeds" },
        new() { Name = "Chia Seeds" },
        new() { Name = "Coconut" },
        new() { Name = "Raisins" },
        new() { Name = "Dates" },
        new() { Name = "Figs" },
        new() { Name = "Cheese" },
        new() { Name = "Yogurt" },
        new() { Name = "Cream" },
        new() { Name = "Butter Milk" },
        new() { Name = "Heavy Cream" },
        new() { Name = "Gelatin" },
        new() { Name = "Cornstarch" },
        new() { Name = "Tapioca Starch" },
        new() { Name = "Pectin" },
        new() { Name = "Coffee" },
        new() { Name = "Tea" },
        new() { Name = "Juice" },
        new() { Name = "Broth" },
        new() { Name = "Wine" },
        new() { Name = "Beer" },
        new() { Name = "Rum" },
        new() { Name = "Vodka" },
        new() { Name = "Whiskey" },
        new() { Name = "Brandy" },
        new() { Name = "Mint" },
        new() { Name = "Lavender" },
        new() { Name = "Saffron" },
        new() { Name = "Cardamom" },
        new() { Name = "Star Anise" },
        new() { Name = "Mozzarella" },
        new() { Name = "Avocado" }
    ];

    private readonly Recipe[] _newRecipes =
    [
        new()
        {
            Title = "The Salad", PreparationDuration = TimeSpan.Parse("00:20:00"),
            CookingDuration = TimeSpan.Parse("00:00:00"), Difficulty = 5, ImagePath = "the_salad.jpg", CreatorId = 1
        },
        new()
        {
            Title = "The Salad 2", PreparationDuration = TimeSpan.Parse("00:20:00"),
            CookingDuration = TimeSpan.Parse("00:00:00"), Difficulty = 3, ImagePath = "the_salad2.jpg", CreatorId = 1
        },
        new()
        {
            Title = "Simple Pasta Bake", PreparationDuration = TimeSpan.Parse("00:15:00"),
            CookingDuration = TimeSpan.Parse("00:30:00"), Difficulty = 1, ImagePath = "1.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Quick Omelette", PreparationDuration = TimeSpan.Parse("00:05:00"),
            CookingDuration = TimeSpan.Parse("00:10:00"), Difficulty = 1, ImagePath = "2.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Chocolate Chip Cookies", PreparationDuration = TimeSpan.Parse("00:20:00"),
            CookingDuration = TimeSpan.Parse("00:12:00"), Difficulty = 2, ImagePath = "3.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Classic Beef Burger", PreparationDuration = TimeSpan.Parse("00:15:00"),
            CookingDuration = TimeSpan.Parse("00:10:00"), Difficulty = 1, ImagePath = "4.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Vegetable Stir-Fry", PreparationDuration = TimeSpan.Parse("00:10:00"),
            CookingDuration = TimeSpan.Parse("00:15:00"), Difficulty = 1, ImagePath = "5.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Lemonade", PreparationDuration = TimeSpan.Parse("00:05:00"),
            CookingDuration = TimeSpan.Parse("00:00:00"), Difficulty = 5, ImagePath = "6.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Chicken Noodle Soup", PreparationDuration = TimeSpan.Parse("00:20:00"),
            CookingDuration = TimeSpan.Parse("00:40:00"), Difficulty = 1, ImagePath = "7.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Guacamole", PreparationDuration = TimeSpan.Parse("00:10:00"),
            CookingDuration = TimeSpan.Parse("00:00:00"), Difficulty = 4, ImagePath = "8.jpg", CreatorId = 2
        },
        new()
        {
            Title = "Pancakes", PreparationDuration = TimeSpan.Parse("00:10:00"),
            CookingDuration = TimeSpan.Parse("00:15:00"), Difficulty = 1, ImagePath = "9.jpg", CreatorId = 1
        }
    ];

    private readonly RecipesCategory[] _newRecipesCategories =
    [
        new() { RecipeId = 1, CategoryId = 8 },
        new() { RecipeId = 2, CategoryId = 8 },
        new() { RecipeId = 3, CategoryId = 1 },
        new() { RecipeId = 3, CategoryId = 6 },
        new() { RecipeId = 4, CategoryId = 9 },
        new() { RecipeId = 5, CategoryId = 3 },
        new() { RecipeId = 5, CategoryId = 7 },
        new() { RecipeId = 6, CategoryId = 1 },
        new() { RecipeId = 7, CategoryId = 1 },
        new() { RecipeId = 7, CategoryId = 2 },
        new() { RecipeId = 8, CategoryId = 4 },
        new() { RecipeId = 9, CategoryId = 1 },
        new() { RecipeId = 10, CategoryId = 5 },
        new() { RecipeId = 11, CategoryId = 9 }
    ];

    private readonly RecipesIngredient[] _newRecipesIngredients =
    [
        new() { RecipeId = 1, IngredientId = 3, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 1, IngredientId = 5, Quantity = 4, Unit = null },
        new() { RecipeId = 1, IngredientId = 9, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 1, IngredientId = 10, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 1, IngredientId = 19, Quantity = 5, Unit = null },
        new() { RecipeId = 1, IngredientId = 55, Quantity = 200, Unit = "g" },
        new() { RecipeId = 1, IngredientId = 65, Quantity = 2, Unit = null },
        new() { RecipeId = 1, IngredientId = 107, Quantity = 250, Unit = "g" },
        new() { RecipeId = 1, IngredientId = 108, Quantity = 2, Unit = null },
        new() { RecipeId = 2, IngredientId = 3, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 2, IngredientId = 5, Quantity = 4, Unit = null },
        new() { RecipeId = 2, IngredientId = 9, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 2, IngredientId = 10, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 2, IngredientId = 19, Quantity = 5, Unit = null },
        new() { RecipeId = 2, IngredientId = 55, Quantity = 200, Unit = "g" },
        new() { RecipeId = 2, IngredientId = 65, Quantity = 2, Unit = null },
        new() { RecipeId = 2, IngredientId = 107, Quantity = 250, Unit = "g" },
        new() { RecipeId = 2, IngredientId = 108, Quantity = 2, Unit = null },
        new() { RecipeId = 3, IngredientId = 58, Quantity = 500, Unit = "g" },
        new() { RecipeId = 3, IngredientId = 19, Quantity = 400, Unit = "g" },
        new() { RecipeId = 3, IngredientId = 18, Quantity = 1, Unit = null },
        new() { RecipeId = 3, IngredientId = 17, Quantity = 2, Unit = "cloves" },
        new() { RecipeId = 3, IngredientId = 9, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 3, IngredientId = 84, Quantity = 100, Unit = "g" },
        new() { RecipeId = 3, IngredientId = 3, Quantity = 1, Unit = "tsp" },
        new() { RecipeId = 3, IngredientId = 16, Quantity = 0.5m, Unit = "tsp" },
        new() { RecipeId = 4, IngredientId = 5, Quantity = 3, Unit = null },
        new() { RecipeId = 4, IngredientId = 6, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 4, IngredientId = 4, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 4, IngredientId = 84, Quantity = 50, Unit = "g" },
        new() { RecipeId = 4, IngredientId = 3, Quantity = 1, Unit = "pinch" },
        new() { RecipeId = 4, IngredientId = 16, Quantity = 1, Unit = "pinch" },
        new() { RecipeId = 5, IngredientId = 1, Quantity = 250, Unit = "g" },
        new() { RecipeId = 5, IngredientId = 4, Quantity = 150, Unit = "g" },
        new() { RecipeId = 5, IngredientId = 2, Quantity = 100, Unit = "g" },
        new() { RecipeId = 5, IngredientId = 5, Quantity = 1, Unit = null },
        new() { RecipeId = 5, IngredientId = 13, Quantity = 1, Unit = "tsp" },
        new() { RecipeId = 5, IngredientId = 12, Quantity = 0.5m, Unit = "tsp" },
        new() { RecipeId = 5, IngredientId = 3, Quantity = 0.25m, Unit = "tsp" },
        new() { RecipeId = 5, IngredientId = 28, Quantity = 200, Unit = "g" },
        new() { RecipeId = 6, IngredientId = 51, Quantity = 500, Unit = "g" },
        new() { RecipeId = 6, IngredientId = 59, Quantity = 4, Unit = null },
        new() { RecipeId = 6, IngredientId = 18, Quantity = 0.5m, Unit = null },
        new() { RecipeId = 6, IngredientId = 64, Quantity = 4, Unit = "slices" },
        new() { RecipeId = 6, IngredientId = 19, Quantity = 1, Unit = null },
        new() { RecipeId = 6, IngredientId = 84, Quantity = 4, Unit = "slices" },
        new() { RecipeId = 6, IngredientId = 34, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 6, IngredientId = 35, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 6, IngredientId = 3, Quantity = 1, Unit = "tsp" },
        new() { RecipeId = 6, IngredientId = 16, Quantity = 0.5m, Unit = "tsp" },
        new() { RecipeId = 7, IngredientId = 62, Quantity = 1, Unit = null },
        new() { RecipeId = 7, IngredientId = 61, Quantity = 2, Unit = null },
        new() { RecipeId = 7, IngredientId = 66, Quantity = 1, Unit = null },
        new() { RecipeId = 7, IngredientId = 18, Quantity = 1, Unit = null },
        new() { RecipeId = 7, IngredientId = 17, Quantity = 2, Unit = "cloves" },
        new() { RecipeId = 7, IngredientId = 44, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 7, IngredientId = 9, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 7, IngredientId = 32, Quantity = 3, Unit = "tbs" },
        new() { RecipeId = 7, IngredientId = 57, Quantity = 200, Unit = "g" },
        new() { RecipeId = 8, IngredientId = 20, Quantity = 4, Unit = null },
        new() { RecipeId = 8, IngredientId = 8, Quantity = 1, Unit = "liter" },
        new() { RecipeId = 8, IngredientId = 2, Quantity = 150, Unit = "g" },
        new() { RecipeId = 9, IngredientId = 50, Quantity = 500, Unit = "g" },
        new() { RecipeId = 9, IngredientId = 61, Quantity = 3, Unit = null },
        new() { RecipeId = 9, IngredientId = 62, Quantity = 3, Unit = "stalks" },
        new() { RecipeId = 9, IngredientId = 18, Quantity = 1, Unit = null },
        new() { RecipeId = 9, IngredientId = 58, Quantity = 200, Unit = "g" },
        new() { RecipeId = 9, IngredientId = 97, Quantity = 1500, Unit = "ml" },
        new() { RecipeId = 9, IngredientId = 40, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 9, IngredientId = 3, Quantity = 1, Unit = "tsp" },
        new() { RecipeId = 9, IngredientId = 16, Quantity = 0.5m, Unit = "tsp" },
        new() { RecipeId = 10, IngredientId = 108, Quantity = 3, Unit = null },
        new() { RecipeId = 10, IngredientId = 21, Quantity = 1, Unit = null },
        new() { RecipeId = 10, IngredientId = 18, Quantity = 0.25m, Unit = null },
        new() { RecipeId = 10, IngredientId = 19, Quantity = 1, Unit = null },
        new() { RecipeId = 10, IngredientId = 41, Quantity = 2, Unit = "tbs" },
        new() { RecipeId = 10, IngredientId = 17, Quantity = 1, Unit = "clove" },
        new() { RecipeId = 10, IngredientId = 3, Quantity = 0.5m, Unit = "tsp" },
        new() { RecipeId = 11, IngredientId = 1, Quantity = 150, Unit = "g" },
        new() { RecipeId = 11, IngredientId = 11, Quantity = 2, Unit = "tsp" },
        new() { RecipeId = 11, IngredientId = 3, Quantity = 1, Unit = "pinch" },
        new() { RecipeId = 11, IngredientId = 2, Quantity = 1, Unit = "tbs" },
        new() { RecipeId = 11, IngredientId = 6, Quantity = 300, Unit = "ml" },
        new() { RecipeId = 11, IngredientId = 5, Quantity = 1, Unit = null },
        new() { RecipeId = 11, IngredientId = 4, Quantity = 2, Unit = "tbs" }
    ];

    private readonly Step[] _newSteps =
    [
        new()
        {
            RecipeId = 1, StepNumber = 1, Instruction = "Cut the tomatoes in quarters and add to the bowl.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 2, Instruction = "Cut the cucumber in small slices and add to the bowl.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 3, Instruction = "Cut the mozzarella in cube and add it to the bowl",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 4, Instruction = "Add the tuna to the bowl.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 5, Instruction = "Add the olive oil.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 6, Instruction = "Add the vinegar.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 7, Instruction = "Add the salt.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 1, StepNumber = 8, Instruction = "Mix everything.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 1, Instruction = "Cut the tomatoes in quarters and add to the bowl.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 2, Instruction = "Cut the cucumber in small slices and add to the bowl.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 3, Instruction = "Cut the mozzarella in cube and add it to the bowl",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 4, Instruction = "Add the tuna to the bowl.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 5, Instruction = "Add the olive oil.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 6, Instruction = "Add the vinegar.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 7, Instruction = "Add the salt.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 2, StepNumber = 8, Instruction = "Mix everything.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 3, StepNumber = 1, Instruction = "Cook pasta according to package directions. Drain.",
            Duration = TimeSpan.Parse("00:10:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 3, StepNumber = 2, Instruction = "While pasta cooks, chop onion and garlic.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 3, StepNumber = 3,
            Instruction = "Heat olive oil in a pan, sauté onion and garlic until softened.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 3, StepNumber = 4, Instruction = "Add tomato sauce, salt, and pepper. Simmer for 5 minutes.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 3, StepNumber = 5, Instruction = "Combine cooked pasta and sauce in an ovenproof dish.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 3, StepNumber = 6, Instruction = "Top with cheese.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = true
        },
        new()
        {
            RecipeId = 3, StepNumber = 7,
            Instruction = "Bake at 180°C (350°F) for 15-20 minutes, until bubbly and golden.",
            Duration = TimeSpan.Parse("00:20:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 4, StepNumber = 1, Instruction = "Whisk eggs, milk, salt, and pepper in a bowl.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 4, StepNumber = 2, Instruction = "Melt butter in a non-stick skillet over medium heat.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 4, StepNumber = 3, Instruction = "Pour egg mixture into the skillet.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 4, StepNumber = 4,
            Instruction = "As eggs set, gently lift edges and tilt skillet to allow uncooked egg to flow underneath.",
            Duration = TimeSpan.Parse("00:03:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 4, StepNumber = 5, Instruction = "Sprinkle cheese over one half of the omelette.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 4, StepNumber = 6, Instruction = "Fold the other half over the cheese.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 4, StepNumber = 7,
            Instruction = "Cook for another minute until cheese is melted. Slide onto a plate.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 5, StepNumber = 1,
            Instruction =
                "Preheat oven to 190°C (375°F). Cream together softened butter and sugar until light and fluffy.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 2, Instruction = "Beat in the egg and vanilla extract.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 3,
            Instruction = "In a separate bowl, whisk together flour, baking soda, and salt.",
            Duration = TimeSpan.Parse("00:03:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 4,
            Instruction = "Gradually add the dry ingredients to the wet ingredients, mixing until just combined.",
            Duration = TimeSpan.Parse("00:04:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 5, Instruction = "Stir in the chocolate chips.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 6,
            Instruction = "Drop rounded tablespoons of dough onto ungreased baking sheets.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 5, StepNumber = 7, Instruction = "Bake for 10-12 minutes, or until edges are golden brown.",
            Duration = TimeSpan.Parse("00:12:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 5, StepNumber = 8,
            Instruction =
                "Let cool on baking sheets for a few minutes before transferring to wire racks to cool completely.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 6, StepNumber = 1,
            Instruction = "Finely chop half an onion. Mix ground beef, chopped onion, salt, and pepper.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 6, StepNumber = 2, Instruction = "Form the mixture into 4 equal patties.",
            Duration = TimeSpan.Parse("00:03:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 6, StepNumber = 3,
            Instruction =
                "Heat a grill or pan over medium-high heat. Cook patties for 4-5 minutes per side for medium-rare.",
            Duration = TimeSpan.Parse("00:10:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 6, StepNumber = 4,
            Instruction = "During the last minute of cooking, place a slice of cheese on each patty to melt.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 6, StepNumber = 5, Instruction = "Slice tomato and remaining onion. Prepare lettuce leaves.",
            Duration = TimeSpan.Parse("00:03:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 6, StepNumber = 6, Instruction = "Lightly toast the burger buns if desired.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 6, StepNumber = 7,
            Instruction =
                "Assemble burgers: bun bottom, lettuce, tomato, onion, patty with cheese, ketchup/mayo, bun top.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 7, StepNumber = 1,
            Instruction =
                "Chop broccoli, carrots, bell pepper, and onion into bite-sized pieces. Mince garlic and ginger.",
            Duration = TimeSpan.Parse("00:10:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 7, StepNumber = 2, Instruction = "Heat oil in a wok or large skillet over high heat.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 7, StepNumber = 3, Instruction = "Add carrots and onion, stir-fry for 3-4 minutes.",
            Duration = TimeSpan.Parse("00:04:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 7, StepNumber = 4,
            Instruction = "Add broccoli and bell pepper, stir-fry for another 4-5 minutes until tender-crisp.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 7, StepNumber = 5, Instruction = "Add garlic and ginger, stir-fry for 1 minute until fragrant.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 7, StepNumber = 6, Instruction = "Pour soy sauce over vegetables and toss to coat.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 7, StepNumber = 7, Instruction = "Serve immediately, optionally over cooked rice.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 8, StepNumber = 1, Instruction = "Juice the lemons into a pitcher.",
            Duration = TimeSpan.Parse("00:03:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 8, StepNumber = 2, Instruction = "Add water and sugar to the pitcher.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 8, StepNumber = 3, Instruction = "Stir well until the sugar is completely dissolved.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 8, StepNumber = 4,
            Instruction = "Chill in the refrigerator before serving. Serve over ice if desired.",
            Duration = TimeSpan.Parse("01:00:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 9, StepNumber = 1, Instruction = "Chop carrots, celery, and onion.",
            Duration = TimeSpan.Parse("00:10:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 9, StepNumber = 2,
            Instruction = "In a large pot, combine chicken, chopped vegetables, and broth. Bring to a boil.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 9, StepNumber = 3,
            Instruction = "Reduce heat, cover, and simmer until chicken is cooked through, about 20-25 minutes.",
            Duration = TimeSpan.Parse("00:25:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 9, StepNumber = 4,
            Instruction = "Remove chicken from the pot. Let cool slightly, then shred or dice.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 9, StepNumber = 5,
            Instruction =
                "Return chicken to the pot. Add noodles and cook according to package directions, usually 7-10 minutes.",
            Duration = TimeSpan.Parse("00:10:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 9, StepNumber = 6,
            Instruction = "Stir in chopped parsley. Season with salt and pepper to taste.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 9, StepNumber = 7, Instruction = "Serve hot.", Duration = TimeSpan.Parse("00:01:00"),
            IsCooking = false
        },
        new()
        {
            RecipeId = 10, StepNumber = 1,
            Instruction = "Cut avocados in half, remove pit, and scoop flesh into a bowl.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 10, StepNumber = 2,
            Instruction = "Mash avocado flesh with a fork, leaving some chunks if desired.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 10, StepNumber = 3,
            Instruction =
                "Add lime juice, finely chopped onion, diced tomato, chopped cilantro, minced garlic, and salt.",
            Duration = TimeSpan.Parse("00:04:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 10, StepNumber = 4, Instruction = "Stir everything together gently until well combined.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 10, StepNumber = 5, Instruction = "Taste and adjust seasoning if necessary. Serve immediately.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 11, StepNumber = 1,
            Instruction = "In a large bowl, whisk together flour, baking powder, salt, and sugar.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 11, StepNumber = 2,
            Instruction = "In a separate bowl, whisk together milk, egg, and melted butter.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 11, StepNumber = 3,
            Instruction =
                "Pour the wet ingredients into the dry ingredients and whisk until just combined (do not overmix; lumps are okay).",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 11, StepNumber = 4,
            Instruction = "Heat a lightly oiled griddle or frying pan over medium-high heat. Add a knob of butter.",
            Duration = TimeSpan.Parse("00:02:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 11, StepNumber = 5,
            Instruction = "Pour or scoop about 1/4 cup of batter onto the griddle for each pancake.",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = true
        },
        new()
        {
            RecipeId = 11, StepNumber = 6,
            Instruction =
                "Cook for about 2-3 minutes per side, until golden brown. Flip when bubbles appear on the surface.",
            Duration = TimeSpan.Parse("00:05:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 11, StepNumber = 7,
            Instruction = "Repeat with remaining batter, adding more butter to the pan as needed.",
            Duration = TimeSpan.Parse("00:08:00"), IsCooking = false
        },
        new()
        {
            RecipeId = 11, StepNumber = 8,
            Instruction = "Serve warm with your favorite toppings (e.g., maple syrup, fruit).",
            Duration = TimeSpan.Parse("00:01:00"), IsCooking = false
        }
    ];


    private readonly User[] _newUsers =
    [
        new()
        {
            Username = "admin", Email = "admin@admin.com",
            PasswordHash = "9799894B364201483DB2C3E8A2376F72:D585D41B951B0757",
            IsAdmin = true, ImagePath = "3.jpg"
        },
        new()
        {
            Username = "user", Email = "user@user.com",
            PasswordHash = "2C3863D0E5F3FDC89B71EEA788FF883D:D631D676E107878F",
            IsAdmin = false, ImagePath = "29.jpg"
        }
    ];

    public CookbookContext()
    {
    }

    public CookbookContext(DbContextOptions<CookbookContext> options) : base(options)
    {
    }

    public CookbookContext(DbContextOptions<CookbookContext> options, IConfiguration config)
        : base(options)
    {
        _configuration = config;
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipesCategory> RecipesCategories { get; set; }
    public virtual DbSet<RecipesIngredient> RecipesIngredients { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _configuration != null) // ADD NULL CHECK
            optionsBuilder.UseNpgsql(_configuration.GetSection("DatabaseProviderString").Value);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();

            entity.Property(e => e.CategoryId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("ingredients_pkey");

            entity.ToTable("ingredients");

            entity.HasIndex(e => e.Name, "ingredients_name_key").IsUnique();

            entity.Property(e => e.IngredientId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ingredient_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("recipes_pkey");

            entity.ToTable("recipes");

            entity.Property(e => e.RecipeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("recipe_id");
            entity.Property(e => e.CookingDuration).HasColumnName("cooking_duration");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Difficulty).HasColumnName("difficulty");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .HasColumnName("image_path");
            entity.Property(e => e.PreparationDuration).HasColumnName("preparation_duration");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Creator).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("recipes_creator_id_fkey");
        });

        modelBuilder.Entity<RecipesCategory>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.CategoryId }).HasName("recipes_categories_pkey");

            entity.ToTable("recipes_categories");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(d => d.Category).WithMany(p => p.RecipesCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("recipes_categories_category_id_fkey");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipesCategories)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipes_categories_recipe_id_fkey");
        });

        modelBuilder.Entity<RecipesIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId }).HasName("recipes_ingredients_pkey");

            entity.ToTable("recipes_ingredients");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(7, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Unit)
                .HasMaxLength(30)
                .HasColumnName("unit");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipesIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("recipes_ingredients_ingredient_id_fkey");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipesIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipes_ingredients_recipe_id_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.ReviewerId }).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");
            entity.Property(e => e.Impression)
                .HasMaxLength(500)
                .HasColumnName("impression");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("reviews_recipe_id_fkey");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ReviewerId)
                .HasConstraintName("reviews_reviewer_id_fkey");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => new { e.StepNumber, e.RecipeId }).HasName("steps_pkey");

            entity.ToTable("steps");

            entity.Property(e => e.StepNumber).HasColumnName("step_number");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Instruction)
                .HasMaxLength(200)
                .HasColumnName("instruction");
            entity.Property(e => e.IsCooking).HasColumnName("is_cooking");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Steps)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("steps_recipe_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .HasColumnName("image_path");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(72)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
        });
    }

    public async Task SeedAsync(CookbookContext context, CancellationToken cancellationToken)
    {
        if (!await context.Users.AnyAsync(cancellationToken))
        {
            context.Users.AddRange(_newUsers);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Categories.AnyAsync(cancellationToken))
        {
            context.Categories.AddRange(_newCategories);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Ingredients.AnyAsync(cancellationToken))
        {
            context.Ingredients.AddRange(_newIngredients);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Recipes.AnyAsync(cancellationToken))
        {
            context.Recipes.AddRange(_newRecipes);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.RecipesCategories.AnyAsync(cancellationToken))
        {
            context.RecipesCategories.AddRange(_newRecipesCategories);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.RecipesIngredients.AnyAsync(cancellationToken))
        {
            context.RecipesIngredients.AddRange(_newRecipesIngredients);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Steps.AnyAsync(cancellationToken))
        {
            context.Steps.AddRange(_newSteps);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public void Seed(CookbookContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(_newUsers);
            context.SaveChanges();
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(_newCategories);
            context.SaveChanges();
        }

        if (!context.Ingredients.Any())
        {
            context.Ingredients.AddRange(_newIngredients);
            context.SaveChanges();
        }

        if (!context.Recipes.Any())
        {
            context.Recipes.AddRange(_newRecipes);
            context.SaveChanges();
        }

        if (!context.RecipesCategories.Any())
        {
            context.RecipesCategories.AddRange(_newRecipesCategories);
            context.SaveChanges();
        }

        if (!context.RecipesIngredients.Any())
        {
            context.RecipesIngredients.AddRange(_newRecipesIngredients);
            context.SaveChanges();
        }

        if (!context.Steps.Any())
        {
            context.Steps.AddRange(_newSteps);
            context.SaveChanges();
        }
    }
}