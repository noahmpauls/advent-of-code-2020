use clap::{App, Arg};
use std::fs;

use allergen_assessment::{Food};

fn main() {
    let part_validate = |s| {
        if s == "1" || s == "2" {
            return Ok(());
        }
        return Err(String::from("part must be either 1 or 2"));
    };

    let matches = App::new("Jurassic Jigsaw")
        .arg(Arg::with_name("PART")
            .short("p")
            .long("part")
            .takes_value(true)
            .required(true)
            .validator(part_validate)
            .help("part of this day to run"))
        .arg(Arg::with_name("FILE")
            .short("f")
            .long("file")
            .takes_value(true)
            .required(true)
            .help("input file to take"))
        .get_matches();

    let part = matches.value_of("PART").unwrap().parse::<u8>().unwrap();
    let file = matches.value_of("FILE").unwrap();

    run(part, file);
}

fn run(part: u8, file: &str) {
    match part {
        1 => part_one(file),
        2 => part_two(file),
        _ => (),
    }
}

fn part_one(file: &str) {
    // part 1: map allergens to foods and ingredients to foods.
    // if an ingredient food group is a superset of an allergen food
    // group, that ingredient could contain that allergen and is therefore not
    // safe.
    let contents = fs::read_to_string(file).unwrap();
    let foods: Vec<Food> = contents.lines().enumerate()
        .map(|(id, line)| Food::from(id, line))
        .collect();
    let safe_ingredients = allergen_assessment::find_safe_ingredients(&foods);
    let mut occurences = 0;
    for ingredient in safe_ingredients {
        for food in &foods {
            if food.contains_ingredient(&ingredient) {
                occurences += 1;
            }
        }
    }
    println!("{}", occurences);
}

fn part_two(file: &str) {
    // part 2: remaining unsafe ingredients will be a superset of at least one
    // food group. In order to have complete info, there must be at least one
    // ingredient that is only a superset of one allergen food group. Start by
    // eliminating that one, then remove it from contention and repeat.
    let contents = fs::read_to_string(file).unwrap();
    let foods: Vec<Food> = contents.lines().enumerate()
        .map(|(id, line)| Food::from(id, line))
        .collect();
    let mut mappings = allergen_assessment::ingredient_allergen_mappings(&foods);
    mappings.sort_by(|a, b| a.1.partial_cmp(&b.1).unwrap());
    let ingredients: Vec<String> = mappings.iter().map(|e| e.0.clone()).collect();
    let cardinal = ingredients.join(",");

    println!("{}", cardinal);
}

