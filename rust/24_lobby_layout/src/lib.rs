use std::collections::HashSet;

#[derive(Debug)]
pub enum Compass {
    West,
    East,
    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast,
}

impl Compass {
    pub fn from(s: &str) -> Result<Compass, String> {
        match s.to_lowercase().as_str() {
            "w" => Ok(Compass::West),
            "e" => Ok(Compass::East),
            "nw" => Ok(Compass::NorthWest),
            "ne" => Ok(Compass::NorthEast),
            "sw" => Ok(Compass::SouthWest),
            "se" => Ok(Compass::SouthEast),
            _ => Err(String::from("unable to parse {} as compass direction")),
        }
    }

    pub fn delta(&self) -> Coord {
        match self {
            Compass::West => Coord::new(-2, 0),
            Compass::East => Coord::new(2, 0),
            Compass::NorthWest => Coord::new(-1, 1),
            Compass::NorthEast => Coord::new(1, 1),
            Compass::SouthWest => Coord::new(-1, -1),
            Compass::SouthEast => Coord::new(1, -1),
        }
    }

    pub fn values() -> Vec<Compass> {
        vec![
            Compass::West,
            Compass::East,
            Compass::NorthWest,
            Compass::NorthEast,
            Compass::SouthWest,
            Compass::SouthEast,
        ]
    }
}

#[derive(Debug, PartialEq, Eq, Hash, Copy, Clone)]
pub struct Coord {
    x: i32,
    y: i32,
}

impl Coord {
    pub fn new(x: i32, y: i32) -> Coord {
        Coord { x, y }
    }
}

impl std::ops::Add for Coord {
    type Output = Coord;

    fn add(self, _rhs: Coord) -> Coord {
        Coord {
            x: self.x + _rhs.x,
            y: self.y + _rhs.y,
        }
    }
}

pub struct FlipTiles {
    black: HashSet<Coord>,
}

impl FlipTiles {
    pub fn new() -> FlipTiles {
        FlipTiles {
            black: HashSet::new(),
        }
    }

    pub fn black_count(&self) -> usize {
        self.black.len()
    }

    pub fn flip(&mut self, coord: Coord) {
        if !self.black.remove(&coord) {
            self.black.insert(coord);
        }
    }

    pub fn conway(&mut self) {
        let mut new_black = HashSet::new();
        let mut memo = HashSet::new();

        for black in &self.black {
            let count = self.adjacent_black_count(*black);
            if !(count == 0 || count > 2) {
                new_black.insert(*black);
            }

            for neighbor in Compass::values().iter().map(|c| *black + c.delta()) {
                if !memo.contains(&neighbor)
                    && !self.black.contains(&neighbor) 
                    && self.adjacent_black_count(neighbor) == 2 
                {
                    new_black.insert(neighbor);
                    memo.insert(neighbor);
                }
            }
        }

        self.black = new_black;
    }

    fn adjacent_black_count(&self, coord: Coord) -> usize {
        Compass::values().iter()
        .map(|c| if self.black.contains(&(coord + c.delta())) { 1 } else { 0 })
        .sum()
    }
}

