use clap::{App, Arg};
use regex::Regex;
use std::fs;

use jurassic_jigsaw::{Tile, Tileset, TilesetBuilder, SpaceType, Image, SatelliteImage};

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
    let tile_strings = get_tile_strings(&contents);

    let mut tileset = TilesetBuilder::new();
    for t in tile_strings {
        let (id, tile) = parse_tile(&t);
        tileset.with_tile(id, String::from(tile));
    }
    let tileset = tileset.build();

    let mut product: u128 = 1;
    for c in tileset.tiles_by_type(SpaceType::Corner) {
        product *= c.id as u128;
    }
    println!("{}", product);
}

fn part_two(file: &str) {
    let contents = fs::read_to_string(file).unwrap();
    let tile_strings = get_tile_strings(&contents);

    let mut tileset = TilesetBuilder::new();
    for t in tile_strings {
        let (id, tile) = parse_tile(&t);
        tileset.with_tile(id, String::from(tile));
    }
    let tileset = tileset.build();

    let image = Image::from(tileset);
    let sattelite_image = SatelliteImage::from(image);
    println!("{}", sattelite_image.roughness());
}

fn get_tile_strings(tileset: &str) -> Vec<String> {
    let regex = Regex::new(r"(Tile [0-9]+:\s+(?:[.#]+(?:\s+)?)+)").unwrap();
    regex.find_iter(tileset).map(|m| {String::from(String::from(m.as_str()).trim_end())}).collect()
}

fn parse_tile(tile: &str) -> (u32, &str) {
    let regex = Regex::new(r"Tile (?P<id>[0-9]+):\s+(?P<tile>(?:[.#]+(?:\s+)?)+)").unwrap();
    let captures = regex.captures(tile).unwrap();

    let id = captures.name("id").unwrap().as_str().parse::<u32>().unwrap();
    let tile = captures.name("tile").unwrap().as_str();

    (id, tile)
}
