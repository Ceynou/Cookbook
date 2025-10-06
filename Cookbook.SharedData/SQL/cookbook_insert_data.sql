-- Insert data into the users table
INSERT INTO users (username, email, password_hash, is_admin, image_path) VALUES
('admin', 'admin@admin.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', TRUE, '3.jpg'),
('user0', 'user0@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, '29.jpg'),
('user1', 'user1@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, '34.jpg'),
('user2', 'user2@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, '42.jpg'),
('user3', 'user3@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, '81.jpg'),
('user4', 'user4@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, '85.jpg'),
('user5', 'user5@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, NULL),
('user6', 'user6@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, NULL),
('user7', 'user7@user.fr', '$2a$11$wEmM.8Xz88.ew0uzwKje5eFWNbPKpzoaYFHOVRuTQAEHAzxX1gWam', FALSE, NULL);


-- Insert data into the recipes table
INSERT INTO recipes (title, preparation_duration, cooking_duration, difficulty, image_path, creator_id) VALUES
('The Salad', '00:20:00', '00:00:00', 5, 'the_salad.jpg', 1),
('The Salad 2', '00:20:00', '00:00:00', 3, 'the_salad2.jpg', 1),
('Simple Pasta Bake', '00:15:00', '00:30:00', 1, '1.jpg', 2),
('Quick Omelette', '00:05:00', '00:10:00', 1, '2.jpg', 3),
('Chocolate Chip Cookies', '00:20:00', '00:12:00', 2, '3.jpg', 4),
('Classic Beef Burger', '00:15:00', '00:10:00', 1, '4.jpg', 5),
('Vegetable Stir-Fry', '00:10:00', '00:15:00', 1, '5.jpg', 6),
('Lemonade', '00:05:00', '00:00:00', 5, '6.jpg', 7),
('Chicken Noodle Soup', '00:20:00', '00:40:00', 1, '7.jpg', 8),
('Guacamole', '00:10:00', '00:00:00', 4, '8.jpg', 9),
('Pancakes', '00:10:00', '00:15:00', 1, '9.jpg', 1);


-- Insert data into the ingredients table
INSERT INTO ingredients (name) VALUES
('Flour'),
('Sugar'),
('Salt'),
('Butter'),
('Eggs'),
('Milk'),
('Yeast'),
('Water'),
('Olive Oil'),
('Vinegar'),
('Baking Powder'),
('Baking Soda'),
('Vanilla Extract'),
('Cinnamon'),
('Nutmeg'),
('Pepper'),
('Garlic'),
('Onion'),
('Tomato'),
('Lemon'),
('Lime'),
('Orange'),
('Apple'),
('Banana'),
('Strawberry'),
('Blueberry'),
('Raspberry'),
('Chocolate'),
('Cocoa Powder'),
('Honey'),
('Maple Syrup'),
('Soy Sauce'),
('Mustard'),
('Ketchup'),
('Mayonnaise'),
('Basil'),
('Oregano'),
('Thyme'),
('Rosemary'),
('Parsley'),
('Cilantro'),
('Dill'),
('Chives'),
('Ginger'),
('Turmeric'),
('Paprika'),
('Cumin'),
('Cayenne Pepper'),
('Chicken'),
('Beef'),
('Pork'),
('Fish'),
('Shrimp'),
('Salmon'),
('Tuna'),
('Rice'),
('Pasta'),
('Bread'),
('Potatoes'),
('Carrots'),
('Celery'),
('Broccoli'),
('Spinach'),
('Lettuce'),
('Cucumber'),
('Bell Pepper'),
('Corn'),
('Peas'),
('Beans'),
('Lentils'),
('Oats'),
('Almonds'),
('Walnuts'),
('Peanuts'),
('Cashews'),
('Sunflower Seeds'),
('Sesame Seeds'),
('Chia Seeds'),
('Coconut'),
('Raisins'),
('Dates'),
('Figs'),
('Cheese'),
('Yogurt'),
('Cream'),
('Butter Milk'),
('Heavy Cream'),
('Gelatin'),
('Cornstarch'),
('Tapioca Starch'),
('Pectin'),
('Coffee'),
('Tea'),
('Juice'),
('Broth'),
('Wine'),
('Beer'),
('Rum'),
('Vodka'),
('Whiskey'),
('Brandy'),
('Mint'),
('Lavender'),
('Saffron'),
('Cardamom'),
('Star Anise'),
('Mozzarella'),
('Avocado');

-- Insert data into the ingredients_recipes table
INSERT INTO recipes_ingredients (recipe_id, ingredient_id, quantity, unit) VALUES
(1, 3, 1, 'tbs'),
(1, 5, 4, NULL),
(1, 9, 2, 'tbs'),
(1, 10, 1, 'tbs'),
(1, 19, 5, NULL),
(1, 55, 200, 'g'),
(1, 65, 2, NULL),
(1, 107, 250, 'g'),
(1, 108, 2, NULL),
(2, 3, 1, 'tbs'),
(2, 5, 4, NULL),
(2, 9, 2, 'tbs'),
(2, 10, 1, 'tbs'),
(2, 19, 5, NULL),
(2, 55, 200, 'g'),
(2, 65, 2, NULL),
(2, 107, 250, 'g'),
(2, 108, 2, NULL),
(3, 58, 500, 'g'), -- Pasta
(3, 19, 400, 'g'), -- Tomato (assuming canned tomatoes or sauce)
(3, 18, 1, NULL),  -- Onion
(3, 17, 2, 'cloves'), -- Garlic
(3, 9, 2, 'tbs'),   -- Olive Oil
(3, 84, 100, 'g'),  -- Cheese (generic)
(3, 3, 1, 'tsp'),   -- Salt
(3, 16, 0.5, 'tsp'), -- Pepper
(4, 5, 3, NULL),   -- Eggs
(4, 6, 2, 'tbs'),   -- Milk
(4, 4, 1, 'tbs'),   -- Butter
(4, 84, 50, 'g'),   -- Cheese
(4, 3, 1, 'pinch'), -- Salt
(4, 16, 1, 'pinch'), -- Pepper
(5, 1, 250, 'g'),  -- Flour
(5, 4, 150, 'g'),  -- Butter
(5, 2, 100, 'g'),  -- Sugar (can specify brown/white if needed)
(5, 5, 1, NULL),   -- Eggs
(5, 13, 1, 'tsp'), -- Vanilla Extract
(5, 12, 0.5, 'tsp'),-- Baking Soda
(5, 3, 0.25, 'tsp'),-- Salt
(5, 28, 200, 'g'), -- Chocolate (chips)
(6, 51, 500, 'g'), -- Beef (ground)
(6, 59, 4, NULL),  -- Bread (burger buns)
(6, 18, 0.5, NULL), -- Onion
(6, 64, 4, 'slices'), -- Lettuce
(6, 19, 1, NULL),  -- Tomato
(6, 84, 4, 'slices'), -- Cheese
(6, 34, 2, 'tbs'), -- Ketchup
(6, 35, 2, 'tbs'), -- Mayonnaise
(6, 3, 1, 'tsp'),   -- Salt
(6, 16, 0.5, 'tsp'), -- Pepper
(7, 62, 1, NULL),  -- Broccoli
(7, 61, 2, NULL),  -- Carrots
(7, 66, 1, NULL),  -- Bell Pepper
(7, 18, 1, NULL),  -- Onion
(7, 17, 2, 'cloves'), -- Garlic
(7, 44, 1, 'tbs'),  -- Ginger (assuming fresh)
(7, 9, 2, 'tbs'),   -- Olive Oil (or Sesame oil)
(7, 32, 3, 'tbs'), -- Soy Sauce
(7, 57, 200, 'g'), -- Rice (for serving, optional)
(8, 20, 4, NULL),   -- Lemon
(8, 8, 1, 'liter'), -- Water
(8, 2, 150, 'g'),   -- Sugar (adjust to taste)
(9, 50, 500, 'g'), -- Chicken (e.g., breast or thighs)
(9, 61, 3, NULL),  -- Carrots
(9, 62, 3, 'stalks'),-- Celery
(9, 18, 1, NULL),  -- Onion
(9, 58, 200, 'g'), -- Pasta (e.g., egg noodles)
(9, 97, 1500, 'ml'),-- Broth (chicken)
(9, 40, 2, 'tbs'), -- Parsley (chopped)
(9, 3, 1, 'tsp'),   -- Salt (or to taste)
(9, 16, 0.5, 'tsp'), -- Pepper (or to taste)
(10, 108, 3, NULL), -- Avocado (ripe)
(10, 21, 1, NULL),  -- Lime (juiced)
(10, 18, 0.25, NULL),-- Onion (finely chopped)
(10, 19, 1, NULL),  -- Tomato (roma, seeded and diced)
(10, 41, 2, 'tbs'), -- Cilantro (chopped)
(10, 17, 1, 'clove'),-- Garlic (minced)
(10, 3, 0.5, 'tsp'), -- Salt
(11, 1, 150, 'g'),  -- Flour
(11, 11, 2, 'tsp'), -- Baking Powder
(11, 3, 1, 'pinch'), -- Salt
(11, 2, 1, 'tbs'),  -- Sugar
(11, 6, 300, 'ml'), -- Milk
(11, 5, 1, NULL),   -- Eggs
(11, 4, 2, 'tbs');  -- Butter (melted, plus more for cooking)

-- Insert data into the categories table
INSERT INTO categories (name) VALUES
('Main Course'), ('Side dish'), ('Dessert'), ('Drink'), ('Appetizer'), ('Pasta'), ('Baking'), ('Salad'), ('Breakfast');

-- Insert data into the categories_recipes table
INSERT INTO recipes_categories (recipe_id, category_id) VALUES
(1, 8),
(2, 8),
(3, 1), -- Simple Pasta Bake -> Main Course
(3, 6), -- Simple Pasta Bake -> Pasta
(4, 9), -- Quick Omelette -> Breakfast
(5, 3), -- Chocolate Chip Cookies -> Dessert
(5, 7), -- Chocolate Chip Cookies -> Baking
(6, 1), -- Classic Beef Burger -> Main Course
(7, 1), -- Vegetable Stir-Fry -> Main Course
(7, 2), -- Vegetable Stir-Fry -> Side dish
(8, 4), -- Lemonade -> Drink
(9, 1), -- Chicken Noodle Soup -> Main Course
(10, 5), -- Guacamole -> Appetizer
(11, 9); -- Pancakes -> Breakfast

-- Insert data into the steps table
INSERT INTO steps (recipe_id, step_number, instruction, duration, is_cooking) VALUES
(1, 1, 'Cut the tomatoes in quarters and add to the bowl.', '00:05:00', TRUE),
(1, 2, 'Cut the cucumber in small slices and add to the bowl.', '00:05:00', TRUE),
(1, 3, 'Cut the mozzarella in cube and add it to the bowl', '00:05:00', TRUE),
(1, 4, 'Add the tuna to the bowl.', '00:01:00', TRUE),
(1, 5, 'Add the olive oil.', '00:01:00', TRUE),
(1, 6, 'Add the vinegar.', '00:01:00', TRUE),
(1, 7, 'Add the salt.', '00:01:00', TRUE),
(1, 8, 'Mix everything.', '00:01:00', TRUE),
(2, 1, 'Cut the tomatoes in quarters and add to the bowl.', '00:05:00', TRUE),
(2, 2, 'Cut the cucumber in small slices and add to the bowl.', '00:05:00', TRUE),
(2, 3, 'Cut the mozzarella in cube and add it to the bowl', '00:05:00', TRUE),
(2, 4, 'Add the tuna to the bowl.', '00:01:00', TRUE),
(2, 5, 'Add the olive oil.', '00:01:00', TRUE),
(2, 6, 'Add the vinegar.', '00:01:00', TRUE),
(2, 7, 'Add the salt.', '00:01:00', TRUE),
(2, 8, 'Mix everything.', '00:01:00', TRUE),
(3, 1, 'Cook pasta according to package directions. Drain.', '00:10:00', FALSE),
(3, 2, 'While pasta cooks, chop onion and garlic.', '00:05:00', TRUE),
(3, 3, 'Heat olive oil in a pan, sauté onion and garlic until softened.', '00:05:00', FALSE),
(3, 4, 'Add tomato sauce, salt, and pepper. Simmer for 5 minutes.', '00:05:00', FALSE),
(3, 5, 'Combine cooked pasta and sauce in an ovenproof dish.', '00:02:00', TRUE),
(3, 6, 'Top with cheese.', '00:01:00', TRUE),
(3, 7, 'Bake at 180°C (350°F) for 15-20 minutes, until bubbly and golden.', '00:20:00', FALSE),
(4, 1, 'Whisk eggs, milk, salt, and pepper in a bowl.', '00:02:00', TRUE),
(4, 2, 'Melt butter in a non-stick skillet over medium heat.', '00:01:00', FALSE),
(4, 3, 'Pour egg mixture into the skillet.', '00:01:00', FALSE),
(4, 4, 'As eggs set, gently lift edges and tilt skillet to allow uncooked egg to flow underneath.', '00:03:00', FALSE),
(4, 5, 'Sprinkle cheese over one half of the omelette.', '00:01:00', TRUE),
(4, 6, 'Fold the other half over the cheese.', '00:01:00', FALSE),
(4, 7, 'Cook for another minute until cheese is melted. Slide onto a plate.', '00:01:00', FALSE),
(5, 1, 'Preheat oven to 190°C (375°F). Cream together softened butter and sugar until light and fluffy.', '00:05:00', TRUE),
(5, 2, 'Beat in the egg and vanilla extract.', '00:02:00', TRUE),
(5, 3, 'In a separate bowl, whisk together flour, baking soda, and salt.', '00:03:00', TRUE),
(5, 4, 'Gradually add the dry ingredients to the wet ingredients, mixing until just combined.', '00:04:00', TRUE),
(5, 5, 'Stir in the chocolate chips.', '00:01:00', TRUE),
(5, 6, 'Drop rounded tablespoons of dough onto ungreased baking sheets.', '00:05:00', TRUE),
(5, 7, 'Bake for 10-12 minutes, or until edges are golden brown.', '00:12:00', FALSE),
(5, 8, 'Let cool on baking sheets for a few minutes before transferring to wire racks to cool completely.', '00:05:00', FALSE),
(6, 1, 'Finely chop half an onion. Mix ground beef, chopped onion, salt, and pepper.', '00:05:00', TRUE),
(6, 2, 'Form the mixture into 4 equal patties.', '00:03:00', TRUE),
(6, 3, 'Heat a grill or pan over medium-high heat. Cook patties for 4-5 minutes per side for medium-rare.', '00:10:00', FALSE),
(6, 4, 'During the last minute of cooking, place a slice of cheese on each patty to melt.', '00:01:00', FALSE),
(6, 5, 'Slice tomato and remaining onion. Prepare lettuce leaves.', '00:03:00', TRUE),
(6, 6, 'Lightly toast the burger buns if desired.', '00:02:00', FALSE),
(6, 7, 'Assemble burgers: bun bottom, lettuce, tomato, onion, patty with cheese, ketchup/mayo, bun top.', '00:02:00', TRUE),
(7, 1, 'Chop broccoli, carrots, bell pepper, and onion into bite-sized pieces. Mince garlic and ginger.', '00:10:00', TRUE),
(7, 2, 'Heat oil in a wok or large skillet over high heat.', '00:01:00', FALSE),
(7, 3, 'Add carrots and onion, stir-fry for 3-4 minutes.', '00:04:00', FALSE),
(7, 4, 'Add broccoli and bell pepper, stir-fry for another 4-5 minutes until tender-crisp.', '00:05:00', FALSE),
(7, 5, 'Add garlic and ginger, stir-fry for 1 minute until fragrant.', '00:01:00', FALSE),
(7, 6, 'Pour soy sauce over vegetables and toss to coat.', '00:01:00', FALSE),
(7, 7, 'Serve immediately, optionally over cooked rice.', '00:01:00', FALSE),
(8, 1, 'Juice the lemons into a pitcher.', '00:03:00', TRUE),
(8, 2, 'Add water and sugar to the pitcher.', '00:01:00', TRUE),
(8, 3, 'Stir well until the sugar is completely dissolved.', '00:01:00', TRUE),
(8, 4, 'Chill in the refrigerator before serving. Serve over ice if desired.', '01:00:00', FALSE),
(9, 1, 'Chop carrots, celery, and onion.', '00:10:00', TRUE),
(9, 2, 'In a large pot, combine chicken, chopped vegetables, and broth. Bring to a boil.', '00:05:00', FALSE),
(9, 3, 'Reduce heat, cover, and simmer until chicken is cooked through, about 20-25 minutes.', '00:25:00', FALSE),
(9, 4, 'Remove chicken from the pot. Let cool slightly, then shred or dice.', '00:05:00', TRUE),
(9, 5, 'Return chicken to the pot. Add noodles and cook according to package directions, usually 7-10 minutes.', '00:10:00', FALSE),
(9, 6, 'Stir in chopped parsley. Season with salt and pepper to taste.', '00:02:00', TRUE),
(9, 7, 'Serve hot.', '00:01:00', FALSE),
(10, 1, 'Cut avocados in half, remove pit, and scoop flesh into a bowl.', '00:02:00', TRUE),
(10, 2, 'Mash avocado flesh with a fork, leaving some chunks if desired.', '00:02:00', TRUE),
(10, 3, 'Add lime juice, finely chopped onion, diced tomato, chopped cilantro, minced garlic, and salt.', '00:04:00', TRUE),
(10, 4, 'Stir everything together gently until well combined.', '00:01:00', TRUE),
(10, 5, 'Taste and adjust seasoning if necessary. Serve immediately.', '00:01:00', TRUE),
(11, 1, 'In a large bowl, whisk together flour, baking powder, salt, and sugar.', '00:02:00', TRUE),
(11, 2, 'In a separate bowl, whisk together milk, egg, and melted butter.', '00:02:00', TRUE),
(11, 3, 'Pour the wet ingredients into the dry ingredients and whisk until just combined (do not overmix; lumps are okay).', '00:02:00', TRUE),
(11, 4, 'Heat a lightly oiled griddle or frying pan over medium-high heat. Add a knob of butter.', '00:02:00', FALSE),
(11, 5, 'Pour or scoop about 1/4 cup of batter onto the griddle for each pancake.', '00:01:00', TRUE),
(11, 6, 'Cook for about 2-3 minutes per side, until golden brown. Flip when bubbles appear on the surface.', '00:05:00', FALSE),
(11, 7, 'Repeat with remaining batter, adding more butter to the pan as needed.', '00:08:00', FALSE),
(11, 8, 'Serve warm with your favorite toppings (e.g., maple syrup, fruit).', '00:01:00', FALSE);


-- Insert data into the reviews table
INSERT INTO reviews (recipe_id, reviewer_id, rating, impression) VALUES
(1, 2, 5, 'Absolutely delicious!'),
(1, 3, 4, 'Absolutely delicious!'),
(1, 4, 1, 'Absolutely delicious LOL!'),
(1, 5, 2, 'Absolutely disgrace!'),
(1, 6, 1, 'Absolutely delicious!'),
(1, 7, 5, 'Absolutely delicious!'),
(1, 8, 4, 'YUck'),
(2, 3, 5, 'Absolutely delicious!'),
(2, 4, 5, 'Absolutely delicious LOL!'),
(2, 5, 5, 'Absolutely disgrace!'),
(2, 6, 5, 'Absolutely delicious!'),
(2, 7, 5, 'Absolutely delicious!'),
(2, 8, 5, 'YUck'),
(3, 3, 1, 'Absolutely delicious!'),
(3, 4, 1, 'Absolutely delicious LOL!'),
(3, 5, 1, 'Absolutely disgrace!'),
(3, 6, 1, 'Absolutely delicious!'),
(3, 7, 1, 'Absolutely delicious!'),
(3, 8, 1, 'YUck');