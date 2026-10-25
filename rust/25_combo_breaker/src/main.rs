use clap::{App, Arg};
use std::fs;

fn main() {
    let part_validate = |s| {
        if s == "1" || s == "2" {
            return Ok(());
        }
        return Err(String::from("part must be either 1 or 2"));
    };

    let matches = App::new("Combo Breaker")
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
        2 => part_one(file),
        _ => (),
    }
}

fn part_one(file: &str) {
    let contents = fs::read_to_string(file).unwrap();
    let mut lines = contents.lines();
    let card: u64 = lines.next().unwrap().parse().unwrap();
    let door: u64 = lines.next().unwrap().parse().unwrap();

    let card_iters = combo_breaker::find_loop_size(card);
    let door_iters = combo_breaker::find_loop_size(door);

    let mut encryption1 = 1;
    for _ in 0..card_iters {
        encryption1 = combo_breaker::transform(encryption1, door);
    }

    let mut encryption2 = 1;
    for _ in 0..door_iters {
        encryption2 = combo_breaker::transform(encryption2, card);
    }

    assert_eq!(encryption1, encryption2);

    println!("{}", encryption1);
}
