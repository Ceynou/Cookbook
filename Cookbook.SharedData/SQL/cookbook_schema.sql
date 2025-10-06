-- TODO Take into account language format and time zone

DROP SCHEMA IF EXISTS public CASCADE;

CREATE SCHEMA IF NOT EXISTS public AUTHORIZATION postgres;

-- TODO
CREATE TABLE
  users (
    user_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(20) UNIQUE NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(72) NOT NULL,
    is_admin BOOL NOT NULL DEFAULT FALSE,
    --birth_date DATE NOT NULL,
    image_path VARCHAR(255),
    --CONSTRAINT cc_legal_age CHECK (birth_date < now () - INTERVAL '13 years'),
    CONSTRAINT cc_username_not_empty CHECK (username <> ''),
    CONSTRAINT cc_email_not_empty CHECK (email <> ''),
    CONSTRAINT cc_image_path_not_empty CHECK (image_path <> '')
  );

CREATE TABLE
  recipes (
    recipe_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    preparation_duration INTERVAL NOT NULL,
    cooking_duration INTERVAL NOT NULL,
    difficulty SMALLINT NOT NULL,
    image_path VARCHAR(255) NOT NULL,
    creator_id INTEGER,
    FOREIGN KEY (creator_id) REFERENCES users (user_id) ON DELETE CASCADE,
    CONSTRAINT cc_difficulty_range CHECK (difficulty BETWEEN 1 AND 10),
    CONSTRAINT cc_duration_positive CHECK (
      cooking_duration + preparation_duration > INTERVAL '00:00:00'
    ),
    CONSTRAINT cc_image_path_not_empty CHECK (image_path <> ''),
    CONSTRAINT cc_title_not_empty CHECK (title <> '')
  );

CREATE TABLE
  steps (
    recipe_id INTEGER NOT NULL,
    step_number SMALLINT NOT NULL,
    instruction VARCHAR(200) NOT NULL,
    duration INTERVAL NOT NULL,
    is_cooking BOOL NOT NULL,
    PRIMARY KEY (step_number, recipe_id),
    FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id) ON DELETE CASCADE,
    CONSTRAINT cc_instruction_not_empty CHECK (instruction <> ''),
    CONSTRAINT cc_duration_positive CHECK (duration > INTERVAL '00:00:00')
  );

CREATE TABLE
  ingredients (
    ingredient_id SMALLINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(30) UNIQUE NOT NULL,
    CONSTRAINT cc_name_not_empty CHECK (name > '')
  );

CREATE TABLE
  categories (
    category_id SMALLINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(30) UNIQUE NOT NULL,
    CONSTRAINT cc_name_not_empty CHECK (name <> '')
  );

-- TODOs Avoid, where possible, concatenating two table names together to create the name of a relationship table. 
-- Rather than cars_mechanics prefer services.
-- Placing the value in one column and the units in another column. 
-- The column should make the units self-evident to prevent the requirement to combine columns again later in the application. 
-- Use CHECK() to ensure valid data is inserted into the column.
CREATE TABLE
  recipes_ingredients (
    recipe_id INTEGER NOT NULL,
    ingredient_id SMALLINT NOT NULL,
    quantity NUMERIC(7,2) NOT NULL,
    unit VARCHAR(30),
    PRIMARY KEY (recipe_id, ingredient_id),
    FOREIGN key (ingredient_id) REFERENCES ingredients (ingredient_id) ON DELETE CASCADE,
    FOREIGN key (recipe_id) REFERENCES recipes (recipe_id) ON DELETE CASCADE,
    CONSTRAINT cc_quantity_positive CHECK (quantity > 0)
  );

CREATE TABLE
  recipes_categories (
    recipe_id INTEGER NOT NULL,
    category_id SMALLINT NOT NULL,
    PRIMARY KEY (recipe_id, category_id),
    FOREIGN KEY (category_id) REFERENCES categories (category_id) ON DELETE CASCADE,
    FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id) ON DELETE CASCADE
  );

-- TODO add entity_rating if needed for liking comments
CREATE TABLE
  reviews (
    recipe_id INTEGER NOT NULL,
    reviewer_id INTEGER NOT NULL,
    rating SMALLINT NOT NULL,
    impression VARCHAR(500),
    PRIMARY KEY (recipe_id, reviewer_id),
	FOREIGN KEY (recipe_id) REFERENCES recipes (recipe_id) ON DELETE CASCADE,
    FOREIGN KEY (reviewer_id) REFERENCES users (user_id) ON DELETE CASCADE,
    CONSTRAINT cc_rating_range CHECK (rating BETWEEN 1 AND 5)
  );

-- CREATE VIEW
--  recipes_overview  as
--    SELECT
--      r.*,
--      COUNT(DISTINCT s.*) AS steps_count,
--      COUNT(DISTINCT ri.*) AS ingredients_count,
--      COUNT(DISTINCT rw.*) AS reviews_count,
--      ROUND(COALESCE(AVG(rw.rating), 0), 2) AS review_rating,
--      u.username, u.image_
--    FROM recipes r
--    LEFT JOIN users u ON r.creator_id = u.user_id
--    LEFT JOIN reviews rw ON rw.recipe_id = r.recipe_id
--    LEFT JOIN recipes_ingredients ri ON ri.recipe_id = r.recipe_id
--    LEFT JOIN steps s ON s.recipe_id = r.recipe_id
--    GROUP BY r.id, u.username, u.image_;
-- CREATE VIEW
--  recipes_detail AS
--   SELECT
--    r.*,
--    u.username, u.is_admin, u.image_,
--    i.ingredient_id, ingredient_.name, ri.quantity, ri.unit,
--    c.category_id, category.name,
--    s.step_numberber, s.instruction, s.duration, s.is_cooking,
--    rw.user_id, rw.rating, rw.opinion, rwu.username AS reviewer_username, rwu.image_path AS reviewer_image_path
--   FROM recipes r
--   LEFT JOIN users u ON u.id = r.creator_id
--   LEFT JOIN recipes_ingredients ri ON ri.recipe_id = r.id
--   LEFT JOIN ingredients i ON i.id = ri.ingredient_id
--   LEFT JOIN recipes_categories rc ON rc.recipe_id = r.id
--   LEFT JOIN categories c ON c.id = rc.category_id
--   LEFT JOIN steps s ON s.recipe_id = r.id
--   LEFT JOIN reviews rw ON rw.recipe_id = r.id
--   LEFT JOIN users rwu ON rwu.id = rw.user_id;