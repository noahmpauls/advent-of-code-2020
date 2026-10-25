

use std::collections::{HashSet, HashMap};
use regex::Regex;

#[derive(Debug, Clone)]
pub struct Food {
    pub id: usize,
    pub ingredients: HashSet<String>,
    pub allergies: HashSet<String>,
}

impl Food {
    pub fn from(id: usize, string: &str) -> Food {
        let regex = Regex::new(r"(?P<ingredients>(?:[a-z]+\s*)+)(?:\(contains\s+(?P<allergies>(?:[a-z]+(?:,\s+)?)+)\))?").unwrap();
        let captures = regex.captures(string).unwrap();

        let ingredients = captures.name("ingredients").unwrap().as_str()
            .split_whitespace()
            .map(|s| String::from(s))
            .collect();
        let allergies = if let Some(allergies) = captures.name("allergies") {
            allergies.as_str()
                .split(", ")
                .map(|s| String::from(s))
                .collect()
        } else {
            HashSet::new()
        };

        Food { id, ingredients, allergies }
    }

    pub fn contains_ingredient(&self, ingredient: &str) -> bool {
        self.ingredients.contains(ingredient)
    }
}

pub fn find_safe_ingredients(foods: &Vec<Food>) -> Vec<String> {
    let mut allergen_foods = HashMap::new();
    let mut ingredient_foods = HashMap::new();

    for food in foods {
        for ingredient in &food.ingredients {
            let group = ingredient_foods.entry(ingredient).or_insert(HashSet::new());
            group.insert(food.id);
        }

        for allergen in &food.allergies {
            let group = allergen_foods.entry(allergen).or_insert(HashSet::new());
            group.insert(food.id);
        }
    }

    // check for any ingredient groups that are not allergen group supersets, and
    // add those to the list.

    let mut result = Vec::new();

    for entry in ingredient_foods {
        let mut allergen_free = true;
        let ingredient = entry.0;
        let ingredient_group = entry.1;
        for entry in &allergen_foods {
            // let allergen = entry.0;
            let allergen_group = entry.1;
            if ingredient_group.is_superset(allergen_group) {
                allergen_free = false;
            }
        }
        if allergen_free {
            result.push(String::from(ingredient));
        }
    }

    result
}

pub fn ingredient_allergen_mappings(foods: &Vec<Food>) -> Vec<(String, String)> {
    let safe_ingredients: HashSet<String> = find_safe_ingredients(foods).into_iter().collect();

    let mut allergen_foods = HashMap::new();
    let mut ingredient_foods = HashMap::new();

    for food in foods {
        for ingredient in &food.ingredients {
            let group = ingredient_foods.entry(ingredient).or_insert(HashSet::new());
            group.insert(food.id);
        }

        for allergen in &food.allergies {
            let group = allergen_foods.entry(allergen).or_insert(HashSet::new());
            group.insert(food.id);
        }
    }

    let mut ingredients_super_allergens = HashMap::new();

    for entry in ingredient_foods {
        let ingredient = entry.0;
        let ingredient_group = entry.1;
        if safe_ingredients.contains(ingredient) {
            continue;
        }
        let allergen_set = ingredients_super_allergens.entry(ingredient).or_insert(HashSet::new());
        for entry in &allergen_foods {
            let allergen = entry.0;
            let allergen_group = entry.1;
            if ingredient_group.is_superset(&allergen_group) {
                allergen_set.insert(allergen);
            }
        }
    }

    let mut result = Vec::new();
    
    while ingredients_super_allergens.len() > 0 {
        let (ingredient, allergen) = ingredients_super_allergens.iter()
            .filter(|entry| {
                entry.1.len() == 1
            })
            .map(|entry| {
                (*entry.0, *entry.1.iter().nth(0).unwrap())
            }).nth(0).unwrap();
        result.push((String::from(ingredient), String::from(*allergen)));
        ingredients_super_allergens.remove(ingredient);
        for allergen_set in ingredients_super_allergens.values_mut() {
            allergen_set.remove(allergen);
        }
    }

    result
}