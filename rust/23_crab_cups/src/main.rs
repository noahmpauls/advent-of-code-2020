use clap::{App, Arg};
use std::fs;

use crab_cups::{Cups};

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
    let contents = fs::read_to_string(file).unwrap();
    let nums: Vec<usize> = contents.lines().next().unwrap()
        .chars()
        .map(|c| String::from(c).parse::<usize>().unwrap() - 1)
        .collect();
    let mut cups = Cups::new(&nums);
    for _ in 0..100 {
        cups.step();
    }
    let result = cups.cups_from(0, nums.len())[1..].iter().map(|n| format!("{}", n+1)).collect::<Vec<String>>().join("");
    println!("{}", result);
}

fn part_two(file: &str) {
    // this takes forever. ultimately, the key is to use a linked list.
    let contents = fs::read_to_string(file).unwrap();
    let mut nums: Vec<usize> = contents.lines().next().unwrap()
        .chars()
        .map(|c| String::from(c).parse::<usize>().unwrap() - 1)
        .collect();
    for num in nums.len()..1_000_000 {
        nums.push(num);
    }
    let mut cups = Cups::new(&nums);
    for _i in 0..10_000_000 {
        cups.step();
    }
    let result = cups.cups_from(0, 3);
    let (c1, c2): (u128, u128) = (result[1] as u128 + 1, result[2] as u128 + 1);
    let product = c1 * c2;
    println!("{}", product);
}

