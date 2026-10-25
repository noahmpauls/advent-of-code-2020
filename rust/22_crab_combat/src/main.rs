use clap::{App, Arg};
use regex::Regex;
use std::fs;

use crab_combat::{Combat};

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
    let regex = Regex::new(r"Player\s+[0-9]+:\s+(?:(?:[0-9]+(?:\s+)?)+)").unwrap();
    let mut decks = regex.find_iter(&contents);
    let mut game = Combat::from_strings(decks.next().unwrap().as_str(), decks.next().unwrap().as_str());
    let winner = loop {
        if let Some(winner) = game.step() {
            break winner;
        }
    };
    let winning_deck = game.deck(winner).unwrap();
    let score: usize = winning_deck.iter().rev().enumerate().map(|(i, c)| (i+1) * c).sum();
    println!("{}", score);
}

fn part_two(file: &str) {
    let contents = fs::read_to_string(file).unwrap();
    let regex = Regex::new(r"Player\s+[0-9]+:\s+(?:(?:[0-9]+(?:\s+)?)+)").unwrap();
    let mut decks = regex.find_iter(&contents);
    let mut game = Combat::from_strings(decks.next().unwrap().as_str(), decks.next().unwrap().as_str());
    let winner = loop {
        if let Some(winner) = game.step_recursive() {
            break winner;
        }
    };
    let winning_deck = game.deck(winner).unwrap();
    let score: usize = winning_deck.iter().rev().enumerate().map(|(i, c)| (i+1) * c).sum();
    println!("{}", score);
}

