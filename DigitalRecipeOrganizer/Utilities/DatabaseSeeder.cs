using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalRecipeOrganizer.Utilities
{
    public static class DatabaseSeeder
    {
        public static async Task SeedSampleRecipes(RecipeDbContext context)
        {
            // Check if database already has recipes
            if (await context.Recipes.AnyAsync())
            {
                return; // Database already seeded
            }

            // Get current user or create a demo user if none exists
            User currentUser;
            if (UserSession.IsLoggedIn)
            {
                currentUser = UserSession.CurrentUser!;
            }
            else
            {
                // This shouldn't happen, but just in case
                return;
            }

            var sampleRecipes = new List<Recipe>
            {
                new Recipe
                {
                    Title = "Classic Chocolate Chip Cookies",
                    Category = "Dessert",
                    Ingredients = @"2 1/4 cups all-purpose flour
1 tsp baking soda
1 tsp salt
1 cup butter, softened
3/4 cup granulated sugar
3/4 cup packed brown sugar
2 large eggs
2 tsp vanilla extract
2 cups chocolate chips",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 Preheat oven to 375°F.\par Mix flour, baking soda and salt in bowl.\par Beat butter and sugars until creamy.\par Add eggs and vanilla.\par Gradually blend in flour mixture.\par Stir in chocolate chips.\par Bake 9-11 minutes.}",
                    PrepTimeMinutes = 15,
                    CookTimeMinutes = 10,
                    Servings = 48,
                    Notes = "Makes about 4 dozen cookies. Can freeze dough for later use.",
                    CreatedDate = DateTime.Now.AddDays(-30),
                    UserId = currentUser.UserId
                },
                new Recipe
                {
                    Title = "Spaghetti Carbonara",
                    Category = "MainCourse",
                    Ingredients = @"400g spaghetti
200g pancetta or bacon, diced
4 large eggs
100g Parmesan cheese, grated
2 cloves garlic, minced
Salt and black pepper to taste
Fresh parsley for garnish",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 Cook spaghetti according to package directions.\par While pasta cooks, fry pancetta until crispy.\par Beat eggs with Parmesan cheese.\par Drain pasta, reserving 1 cup pasta water.\par Toss hot pasta with pancetta and garlic.\par Remove from heat and quickly stir in egg mixture.\par Add pasta water as needed for creamy consistency.\par Season with salt and pepper, garnish with parsley.}",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 15,
                    Servings = 4,
                    Notes = "Use freshly grated Parmesan for best results. Work quickly when adding eggs to prevent scrambling.",
                    CreatedDate = DateTime.Now.AddDays(-25),
                    UserId = currentUser.UserId
                },
                new Recipe
                {
                    Title = "Fresh Fruit Smoothie",
                    Category = "Beverage",
                    Ingredients = @"1 banana
1 cup frozen mixed berries
1 cup Greek yogurt
1/2 cup orange juice
1 tbsp honey
1/2 cup ice cubes
Fresh mint leaves (optional)",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 Add all ingredients to blender.\par Blend on high until smooth.\par Taste and adjust sweetness with honey if needed.\par Pour into glasses and serve immediately.\par Garnish with fresh mint if desired.}",
                    PrepTimeMinutes = 5,
                    CookTimeMinutes = 0,
                    Servings = 2,
                    Notes = "Can substitute almond milk for a dairy-free version. Add protein powder for post-workout nutrition.",
                    CreatedDate = DateTime.Now.AddDays(-20),
                    ScheduledDate = DateTime.Now.AddDays(2),
                    UserId = currentUser.UserId
                },
                new Recipe
                {
                    Title = "Caesar Salad",
                    Category = "Salad",
                    Ingredients = @"1 large head romaine lettuce
1 cup croutons
1/2 cup Parmesan cheese, shaved
Caesar dressing:
3 cloves garlic, minced
2 anchovy fillets
2 tbsp lemon juice
1 tsp Dijon mustard
1 egg yolk
3/4 cup olive oil
Salt and pepper to taste",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 \b Make dressing:\b0  Blend garlic, anchovies, lemon juice, mustard, and egg yolk.\par Slowly drizzle in olive oil while blending.\par Season with salt and pepper.\par\par \b Prepare salad:\b0  Wash and chop romaine lettuce.\par Toss lettuce with dressing.\par Top with croutons and Parmesan shavings.\par Serve immediately.}",
                    PrepTimeMinutes = 20,
                    CookTimeMinutes = 0,
                    Servings = 4,
                    Notes = "For food safety, use pasteurized eggs. Can add grilled chicken for a main course.",
                    CreatedDate = DateTime.Now.AddDays(-15),
                    UserId = currentUser.UserId
                },
                new Recipe
                {
                    Title = "Tomato Basil Soup",
                    Category = "Soup",
                    Ingredients = @"2 tbsp olive oil
1 onion, diced
3 cloves garlic, minced
2 cans (28 oz each) whole tomatoes
2 cups vegetable broth
1 cup fresh basil leaves
1/2 cup heavy cream
2 tbsp sugar
Salt and pepper to taste",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 Heat olive oil in large pot.\par Sauté onion until soft, about 5 minutes.\par Add garlic and cook 1 minute.\par Add tomatoes and broth, bring to boil.\par Simmer 20 minutes.\par Add basil and blend until smooth.\par Stir in cream and sugar.\par Season with salt and pepper.}",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 30,
                    Servings = 6,
                    Notes = "Pairs perfectly with grilled cheese sandwiches. Can freeze for up to 3 months.",
                    CreatedDate = DateTime.Now.AddDays(-10),
                    ScheduledDate = DateTime.Now.AddDays(5),
                    UserId = currentUser.UserId
                },
                new Recipe
                {
                    Title = "Fluffy Pancakes",
                    Category = "Breakfast",
                    Ingredients = @"2 cups all-purpose flour
2 tbsp sugar
2 tsp baking powder
1/2 tsp baking soda
1/2 tsp salt
2 cups buttermilk
2 large eggs
1/4 cup melted butter
1 tsp vanilla extract
Butter for cooking",
                    Instructions = @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}\f0\fs20 Whisk together dry ingredients in large bowl.\par In separate bowl, beat eggs, buttermilk, melted butter, and vanilla.\par Pour wet ingredients into dry ingredients.\par Stir until just combined (batter should be lumpy).\par Heat griddle over medium heat.\par Pour 1/4 cup batter for each pancake.\par Cook until bubbles form, then flip.\par Cook until golden brown on both sides.}",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 15,
                    Servings = 8,
                    Notes = "Serve with maple syrup, fresh berries, or whipped cream. Don't overmix batter for fluffiest pancakes.",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    ScheduledDate = DateTime.Now.AddDays(1),
                    UserId = currentUser.UserId
                }
            };

            await context.Recipes.AddRangeAsync(sampleRecipes);
            await context.SaveChangesAsync();
        }
    }
}
