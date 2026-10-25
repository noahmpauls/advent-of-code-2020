/*
The important observation about this problem is that the borders have a
completely unique match or no match at all, even after flipping.

We can thus find tiles and place them appropriately:
1. find corners.
2. find edges, and assemble edges and corners together.
3. add remaining pieces in

1. create a tile
2. get its borders and find all tiles that share the border (normal and flipped)
3. 
*/

use std::collections::{HashSet, HashMap};

#[derive(Debug, Clone)]
pub struct Tile {
    pub id: u32,
    pixels: usize,
    content: TileContent,
    adjacency: TileAdjacency,
}

#[derive(Debug, Clone, Copy)]
pub enum Rotation {
    None,
    Clock,
    Counter,
    About,
}

impl Rotation {
    pub fn values() -> Vec<Rotation> {
        vec![Rotation::None, Rotation::Clock, Rotation::Counter, Rotation::About,]
    }
}

#[derive(Debug)]
pub enum Mirror {
    None,
    Horizontal,
    Vertical,
}

impl Tile {
    pub fn rotate(&self, rotation: &Rotation) -> Tile {
        Tile {
            id: self.id,
            pixels: self.pixels,
            content: self.content.rotate(rotation),
            adjacency: self.adjacency.rotate(rotation),
        }
    }

    pub fn mirror(&self, mirror: &Mirror) -> Tile {
        Tile {
            id: self.id,
            pixels: self.pixels,
            content: self.content.mirror(mirror),
            adjacency: self.adjacency.mirror(mirror),
        }
    }

    pub fn space_type(&self) -> SpaceType {
        match self.adjacency.adjacent_count() {
            2 => SpaceType::Corner,
            3 => SpaceType::Edge,
            4 => SpaceType::Middle,
            other => panic!("invalid adjacent count {}", other)
        }
    }

    pub fn content_string(&self) -> String {
        let mut result = String::new();
        for line in &self.content.lines {
            result.push_str(&format!("{}\n", line));
        }
        result
    }

    pub fn content_string_borders(&self) -> String {
        let mut result = String::new();
        for (i, line) in self.content.lines.iter().enumerate() {
            if i == 0 || i == self.pixels - 1 {
                result.push_str(&format!("{}\n", line));
            } else {
                result.push_str(&format!("{}{}{}\n",
                    line.chars().nth(0).unwrap(),
                    " ".repeat(self.pixels - 2),
                    line.chars().nth(self.pixels - 1).unwrap()
                ));
            }
        }

        result

    }
}

#[derive(Debug, Clone)]
struct TileContent {
    lines: Vec<String>,
}

impl TileContent {
    pub fn from(tile: &str) -> TileContent {
        let mut lines = Vec::new();
        for line in tile.lines() {
            if !line.is_empty() {
                lines.push(String::from(line));
            }
        }
        TileContent { lines }
    }

    pub fn pixels(&self) -> usize {
        self.lines.len()
    }

    pub fn borders(&self) -> Borders {
        let pixels = self.lines.len();
        let north = self.lines.iter().nth(0).unwrap().clone();
        let south = self.lines.iter().nth(pixels - 1).unwrap().clone();
        let mut east = String::new();
        let mut west = String::new();
        for i in 0..pixels {
            west.push(self.lines[i].chars().nth(0).unwrap());
            east.push(self.lines[i].chars().nth(pixels - 1).unwrap());
        }
        Borders { north, south, east, west }
    }

    pub fn rotate(&self, rotation: &Rotation) -> TileContent {
        match rotation {
            Rotation::None => {
                self.clone()
            },
            Rotation::Clock => {
                let mut lines = Vec::new();
                for i in 0..self.lines.len() {
                    let mut new_line = String::new();
                    for line in self.lines.iter().rev() {
                        new_line.push(line.chars().nth(i).unwrap());
                    }
                    lines.push(new_line);
                }
                TileContent { lines }
            },
            Rotation::Counter => {
                let mut lines = Vec::new();
                for i in (0..self.lines.len()).rev() {
                    let mut new_line = String::new();
                    for line in self.lines.iter() {
                        new_line.push(line.chars().nth(i).unwrap());
                    }
                    lines.push(new_line);
                }
                TileContent { lines }
            },
            Rotation::About => {
                let lines: Vec<String> = self.lines.iter()
                    .rev()
                    .map(|s| {s.chars().rev().collect()})
                    .collect();
                TileContent { lines }
            },
        }
    }

    pub fn mirror(&self, mirror: &Mirror) -> TileContent {
        match mirror {
            Mirror::None => {
                self.clone()
            },
            Mirror::Horizontal => {
                let lines: Vec<String> = self.lines.iter()
                    .map(|s| {s.chars().rev().collect()})
                    .collect();
                TileContent { lines }
            },
            Mirror::Vertical => {
                let lines: Vec<String> = self.lines.iter()
                    .rev()
                    .map(|s| {s.clone()})
                    .collect();
                TileContent { lines }
            },
        }
    }

    pub fn trimmed(&self) -> Vec<String> {
        let mut result = Vec::new();
        for (i, line) in self.lines.iter().enumerate() {
            if i != 0 && i != self.lines.len() - 1 {
                result.push(String::from(&line[1..line.len()-1]));
            }
        }

        result
    }
}

#[derive(Debug, Clone)]
struct TileAdjacency {
    north: Option<u32>,
    south: Option<u32>,
    east: Option<u32>,
    west: Option<u32>,
}

impl TileAdjacency {
    pub fn get(&self, direction: Direction) -> Option<u32> {
        match direction {
            Direction::North => {
                self.north
            },
            Direction::South => {
                self.south
            },
            Direction::East => {
                self.east
            },
            Direction::West => {
                self.west
            },
            _ => panic!("only compass allowed")
        }
    }

    pub fn adjacent_count(&self) -> usize {
        [self.north, self.south, self.east, self.west]
            .iter()
            .map(|a| { if a.is_some() { 1 } else { 0 } })
            .sum()
    }

    pub fn rotate(&self, rotation: &Rotation) -> TileAdjacency {
        match rotation {
            Rotation::None => {
                self.clone()
            },
            Rotation::Clock => {
                TileAdjacency {
                    north: self.west,
                    south: self.east,
                    east: self.north,
                    west: self.south,
                }
            },
            Rotation::Counter => {
                TileAdjacency {
                    north: self.east,
                    south: self.west,
                    east: self.south,
                    west: self.north,
                }
            },
            Rotation::About => {
                TileAdjacency {
                    north: self.south,
                    south: self.north,
                    east: self.west,
                    west: self.east,
                }
            },
        }
    }

    pub fn mirror(&self, mirror: &Mirror) -> TileAdjacency {
        match mirror {
            Mirror::None => {
                self.clone()
            },
            Mirror::Horizontal => {
                TileAdjacency {
                    north: self.north,
                    south: self.south,
                    east: self.west,
                    west: self.east,
                }
            },
            Mirror::Vertical => {
                TileAdjacency {
                    north: self.south,
                    south: self.north,
                    east: self.east,
                    west: self.west,
                }
            },
        }
    }

    pub fn debug_lines(&self, id: u32) -> Vec<String> {
        let mut result = Vec::new();
        result.push(String::from("                "));
        result.push(
            format!("      {}      ", 
            if self.north.is_some() {self.north.unwrap().to_string()} else {String::from("    ")}
        ));
        result.push(
            format!(" {} {} {} ",
            if self.west.is_some() {self.west.unwrap().to_string()} else {String::from("    ")},
            id.to_string(),
            if self.east.is_some() {self.east.unwrap().to_string()} else {String::from("    ")},
        ));
        result.push(
            format!("      {}      ", 
            if self.south.is_some() {self.south.unwrap().to_string()} else {String::from("    ")}
        ));
        result.push(String::from("                "));

        result
    }

    pub fn debug_string(&self, id: u32) -> String {
        let mut result = String::new();
        for line in self.debug_lines(id) {
            result.push_str(&format!("{}\n", line));
        }
        result
    }
}

struct Borders {
    north: String,
    south: String,
    east: String,
    west: String,
}

#[derive(Copy, Clone, PartialEq, PartialOrd, Eq, Hash, Debug)]
enum Direction {
    All,
    North,
    South,
    East,
    West,
}

impl Direction {
    pub fn compass() -> Vec<Direction> {
        vec![
            Direction::North,
            Direction::South,
            Direction::East,
            Direction::West,    
        ]
    }
}

impl Borders {
    fn perms(&self, direction: Direction) -> Vec<String> {
        match direction {
            Direction::North => {
                vec![
                    self.north.clone(),
                    self.north.chars().rev().collect(),
                ]
            },
            Direction::South => {
                vec![
                    self.south.clone(),
                    self.south.chars().rev().collect(),
                ]
            },
            Direction::East => {
                vec![
                    self.east.clone(),
                    self.east.chars().rev().collect(),
                ]
            },
            Direction::West => {
                vec![
                    self.west.clone(),
                    self.west.chars().rev().collect(),
                ]
            },
            Direction::All => {
                vec![
                    self.north.clone(),
                    self.south.clone(),
                    self.east.clone(),
                    self.west.clone(),
                    self.north.chars().rev().collect(),
                    self.south.chars().rev().collect(),
                    self.east.chars().rev().collect(),
                    self.west.chars().rev().collect(),
                ]
            },
        }
    }
}

pub struct Tileset {
    tiles: HashMap<u32, Tile>,
}

impl Tileset {
    pub fn tiles_by_type(&self, space_type: SpaceType) -> Vec<Tile> {
        self.tiles.values().into_iter()
            .filter(|t| {
                t.space_type() == space_type
            })
            .map(|t| {t.clone()})
            .collect()
    }

    pub fn get(&self, id: u32) -> Tile {
        self.tiles[&id].clone()
    }
}

pub struct TilesetBuilder {
    tiles: HashMap<u32, String>
}

impl TilesetBuilder {
    pub fn new() -> TilesetBuilder {
        TilesetBuilder { tiles: HashMap::new() }
    }

    pub fn with_tile(&mut self, id: u32, tile: String) -> &mut TilesetBuilder {
        self.tiles.insert(id, tile);
        self
    }

    pub fn build(&self) -> Tileset {
        let mut content_map = HashMap::new();
        let mut border_map = HashMap::new();

        for entry in self.tiles.iter() {
            let id = entry.0;
            let content = TileContent::from(&entry.1);
            let borders = content.borders();
            for b in borders.perms(Direction::All) {
                let border_set = border_map.entry(b).or_insert(HashSet::new());
                border_set.insert(id);
            }
            content_map.insert(id, content);
        }

        let mut tiles = HashMap::new();

        for entry in content_map.into_iter() {
            let id = *entry.0;
            let content = entry.1;
            let borders = content.borders();

            let mut neighbors = HashMap::new();
            for dir in Direction::compass() {
                neighbors.insert(dir, HashSet::new());
            }

            for dir in Direction::compass() {
                let perms = borders.perms(dir);
                for p in perms {
                    neighbors.insert(dir, neighbors[&dir].union(&border_map[&p])
                                .filter(|i| { ***i != id })
                                .map(|i| {*i})
                                .collect());
                }
            }

            for n in neighbors.iter() {
                assert!(n.1.len() <= 1);
            }

            let get_adjacent = |n: HashSet<&u32>| {
                if n.len() == 1 {
                    Some(**n.iter().nth(0).unwrap())
                } else {
                    None
                }
            };

            let tile_adjacency = TileAdjacency {
                north: get_adjacent(neighbors[&Direction::North].clone()),
                south: get_adjacent(neighbors[&Direction::South].clone()),
                east: get_adjacent(neighbors[&Direction::East].clone()),
                west: get_adjacent(neighbors[&Direction::West].clone()),
            };

            let tile = Tile {
                id: id,
                pixels: content.pixels(),
                content: content,
                adjacency: tile_adjacency,
            };

            tiles.insert(tile.id, tile);
        }

        Tileset { tiles }
    }
}

pub struct Image {
    grid: Grid<Tile>,
    side_length: usize,
}

impl Image {
    pub fn from(tileset: Tileset) -> Image {
        let side_length = (tileset.tiles.len() as f64).sqrt();
        assert_eq!(side_length.fract(), 0.0);
        let side_length = side_length as usize;
        
        let mut grid = Grid::<Tile>::new(side_length, side_length);

        let mut current_coord = Some(Coord::from(0, 0));
        // let mut used_ids = HashSet::<u32>::new();

        while current_coord.is_some() {
            let coord = current_coord.unwrap();
            let mut expected_id = None;  // the id of the tile this space is expected to be
            let mut north_id = None;  // the id of the tile to the north of this one
            let mut west_id = None;  // the id of the tile to the west of this one

            // find the northern tile id and its expected southern neighbor
            if let Some(north_coord) = grid.neighbor_coord(&coord, Direction::North) {
                let north_tile = grid.get(north_coord).unwrap();
                north_id = Some(north_tile.id);
                expected_id = Some(north_tile.adjacency.south.unwrap());
            }

            // find the western tile id and its expected eastern neighbor
            if let Some(west_coord) = grid.neighbor_coord(&coord, Direction::West) {
                let west_tile = grid.get(west_coord).unwrap();
                west_id = Some(west_tile.id);
                // northern and western should have same expected neighbor
                let west_expected_id = Some(west_tile.adjacency.east.unwrap());
                if expected_id.is_some() {
                    assert_eq!(expected_id.unwrap(), west_expected_id.unwrap());
                }
                expected_id = Some(west_tile.adjacency.east.unwrap());
            }

            let tile = if let Some(expected_id) = expected_id {
                tileset.get(expected_id).clone()
            } else {
                tileset.tiles_by_type(SpaceType::Corner).iter().nth(0).unwrap().clone()
            };

            'transform: for mirror in [Mirror::None, Mirror::Vertical] {
                for rotation in Rotation::values() {
                    let transformed_tile = tile.rotate(&rotation).mirror(&mirror);
                    if Self::tile_fits_northwest(&grid, &coord, &transformed_tile) {
                        grid.insert(&coord, transformed_tile);
                        break 'transform;
                    }
                }
            }

            current_coord = grid.next_coord_lrud(coord);
        }

        Image { grid, side_length }
    }

    fn tile_fits_northwest(grid: &Grid<Tile>, coord: &Coord, tile: &Tile) -> bool {
        // check north border and west border
        Self::border_matches(grid, coord, tile, Direction::North) && Self::border_matches(grid, coord, tile, Direction::West)
    }

    fn border_matches(grid: &Grid<Tile>, coord: &Coord, tile: &Tile, direction: Direction) -> bool {
        let tile_border = tile.adjacency.get(direction);
        let grid_border = grid.neighbor_coord(coord, direction);

        // no coord means this is an edge, so no neighbor on this border
        if grid_border.is_none() {
            return tile_border.is_none();
        }

        let grid_border = grid.get(grid_border.unwrap()).unwrap();
        if let Some(tile_border) = tile_border {
            return tile_border == grid_border.id;
        } else {
            return false;
        }
    }

    pub fn id_grid(&self) {
        for row in &self.grid.grid {
            for tile in row {
                print!("{} ", tile.as_ref().unwrap().id)
            }
            println!("")
        }
    }

    pub fn stitch(&self) -> Vec<String> {
        let mut result = Vec::new();
        let rows = &self.grid.grid;

        'outer: for (r, row) in rows.iter().enumerate() {
            for tile in row {
                if tile.is_none() {break 'outer}
                let tile = tile.clone().unwrap();
                if result.len() < (r+1) * tile.content.trimmed().len() {
                    for _ in 0..tile.content.trimmed().len() {
                        result.push(String::new());
                    }
                }
                for (i, line) in tile.content.trimmed().iter().enumerate() {
                    let index = (r*(tile.content.trimmed().len())) + i;
                    result[index].push_str(line);
                }
            }
        }

        return result;
    }

    pub fn stitch_borders(&self) -> Vec<String> {
        let mut result = Vec::new();
        let rows = &self.grid.grid;

        'outer: for (r, row) in rows.iter().enumerate() {
            for tile in row {
                if tile.is_none() {break 'outer}
                let tile = tile.clone().unwrap();
                if result.len() < (r+1) * tile.pixels {
                    for _ in 0..tile.pixels {
                        result.push(String::new());
                    }
                }
                for (i, line) in tile.content.lines.iter().enumerate() {
                    let index = (r*tile.pixels) + i;
                    if i == 0 || i == tile.pixels - 1 {
                        result[index].push_str(line);
                    } else {
                        result[index].push_str(&format!("{}{}{}",
                            line.chars().nth(0).unwrap(),
                            " ".repeat(tile.pixels - 2),
                            line.chars().nth(tile.pixels - 1).unwrap()
                        ))
                    }

                }
            }
        }

        return result;
    }

    pub fn stitch_adjacency(&self) -> Vec<String> {
        let mut result = Vec::new();
        let rows = &self.grid.grid;

        'outer: for (r, row) in rows.iter().enumerate() {
            for tile in row {
                if tile.is_none() {break 'outer}
                let tile = tile.clone().unwrap();
                let adjacency_lines = tile.adjacency.debug_lines(tile.id);
                if result.len() < (r+1) * adjacency_lines.len() {
                    for _ in 0..adjacency_lines.len() {
                        result.push(String::new());
                    }
                }
                for (i, line) in adjacency_lines.iter().enumerate() {
                    let index = (r*adjacency_lines.len()) + i;
                    result[index].push_str(line);
                }
            }
        }

        return result;
    }

    pub fn test_tile_rotation(&self) {
        let tile = self.grid.get(Coord::from(0, 0)).unwrap();
        for line in &tile.content.lines {
            println!("{}", line);
        }
        println!("------------");

        for rotation in Rotation::values() {
            println!("{:?}", rotation);
            let rot_tile = tile.rotate(&rotation);
            for line in &rot_tile.content.lines {
                println!("{}", line);
            }
            println!("");
        }

        for mirror in [Mirror::None, Mirror::Horizontal] {
            println!("{:?}", mirror);
            let rot_tile = tile.mirror(&mirror);
            for line in &rot_tile.content.lines {
                println!("{}", line);
            }
            println!("");
        }

        let adjacency_debug = |a: &TileAdjacency| { vec![a.north, a.east, a.south, a.west ]};

        let tile = self.grid.get(Coord::from(1, 1)).unwrap();
        for line in &tile.content.lines {
            println!("{}", line);
        }
        println!("------------");

        for rotation in Rotation::values() {
            println!("{:?}", rotation);
            let rot_tile = tile.rotate(&rotation);
            for a in adjacency_debug(&rot_tile.adjacency) {
                println!("{:?}, ", a);
            }
            println!("");
        }

        for mirror in [Mirror::None, Mirror::Horizontal] {
            println!("{:?}", mirror);
            let rot_tile = tile.mirror(&mirror);
            for a in adjacency_debug(&rot_tile.adjacency) {
                println!("{:?}, ", a);
            }
            println!("");
        }
    }
}

struct Grid<T> {
    grid: Vec<Vec<Option<T>>>,
    rows: usize,
    cols: usize,
}

#[derive(PartialEq, Copy, Clone, Debug)]
pub enum SpaceType {
    Corner,
    Edge,
    Middle,
}

impl<T> Grid<T> {
    pub fn new(rows: usize, cols: usize) -> Grid<T>{
        let mut grid = Vec::new();
        for _ in 0..rows {
            let mut row = Vec::new();
            for _ in 0..cols {
                row.push(None);
            }
            grid.push(row);
        }

        Grid { grid, rows, cols }
    }

    pub fn insert(&mut self, c: &Coord, item: T) {
        self.grid[c.r][c.c] = Some(item);
    }

    pub fn clear(&mut self, c: Coord) {
        self.grid[c.r][c.c] = None;
    }

    pub fn get(&self, c: Coord) -> Option<&T> {
        self.grid[c.r][c.c].as_ref()
    }

    pub fn space_type(&self, coord: Coord) -> SpaceType {
        let mut row_edge = false;
        let mut col_edge = false;
        if coord.r == 0 || coord.r == self.rows - 1 {
            row_edge = true;
        }
        if coord.c == 0 || coord.c == self.cols - 1 {
            col_edge = true;
        }
        
        if row_edge && col_edge {
            SpaceType::Corner
        } else if row_edge || col_edge {
            SpaceType::Edge
        } else {
            SpaceType::Middle
        }
    }

    pub fn next_coord_lrud(&self, coord: Coord) -> Option<Coord> {
        if (coord.c + 1) < self.cols {
            Some(Coord::from(coord.r, coord.c + 1))
        } else if (coord.r + 1) < self.rows {
            Some(Coord::from(coord.r + 1, 0))
        } else {
            None
        }
    }

    pub fn neighbor_coord(&self, coord: &Coord, direction: Direction) -> Option<Coord> {
        match direction {
            Direction::North => {
                if coord.r > 0 {
                    Some(Coord::from(coord.r - 1, coord.c))
                } else { None }
            },
            Direction::South => {
                if (coord.r + 1) < self.rows {
                    Some(Coord::from(coord.r + 1, coord.c))
                } else { None }
            },
            Direction::East => {
                if (coord.c + 1) < self.cols {
                    Some(Coord::from(coord.r, coord.c + 1))
                } else { None }
            },
            Direction::West => {
                if coord.c > 0 {
                    Some(Coord::from(coord.r, coord.c - 1))
                } else { None }
            },
            Direction::All => panic!("no All allowed"),
        }
    }
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
struct Coord {
    r: usize,
    c: usize,
}

impl Coord {
    pub fn from(r: usize, c: usize) -> Coord {
        Coord { r, c, }
    }
}

pub struct SatelliteImage {
    pixels: Vec<Vec<char>>,
    rows: usize,
    cols: usize,
}

impl SatelliteImage {
    pub fn from(image: Image) -> SatelliteImage {
        let mut pixels = Vec::new();

        for line in image.stitch() {
            let mut row = Vec::new();
            for c in line.chars() {
                row.push(c);
            }
            pixels.push(row);
        }

        let rows = pixels.len();
        let cols = pixels[0].len();

        SatelliteImage { pixels, rows, cols }
    }

    fn get(&self, coord: Coord) -> Option<char> {
        if coord.r >= self.rows || coord.c >= self.cols {
            None
        } else {
            Some(self.pixels[coord.r][coord.c])
        }
    }

    fn pixel_count(&self, char: char) -> usize {
        let mut count = 0;

        for row in &self.pixels {
            for pixel in row {
                if *pixel == char {
                    count += 1;
                }
            }
        }
        
        count
    }

    fn pattern_pixels(&self, pattern: Pattern) -> HashSet<Coord> {
        let mut result = HashSet::new();

        for r in 0..self.rows {
            for c in 0..self.cols {
                let start_coord = Coord::from(r, c);
                for rotation in Rotation::values() {
                    'pattern: for mirror in [Mirror::None, Mirror::Horizontal] {
                        let transform_pattern = pattern
                            .rotate(rotation)
                            .mirror(mirror)
                            .shift(start_coord);
                        let mut match_coords = HashSet::new();
                        for p in transform_pattern.pixels {
                            if let Some(char) = self.get(p) {
                                if char == '#' {
                                    match_coords.insert(p.clone());
                                } else {
                                    continue 'pattern;
                                }
                            } else {
                                continue 'pattern;
                            }
                        }
                        for c in match_coords.drain() {
                            result.insert(c);
                        }
                    }
                }
            }
        }

        result
    }

    pub fn roughness(&self) -> usize {
        self.pixel_count('#') - self.pattern_pixels(Pattern::sea_monster()).len()
    }
}

#[derive(Clone)]
struct Pattern {
    pixels: Vec<Coord>,
    rows: usize,
    cols: usize,
}

impl Pattern {
    pub fn sea_monster() -> Pattern {
        let mut pixels = Vec::new();
        
        /*
                          # 
        #    ##    ##    ###
         #  #  #  #  #  #   
        */

        let coords = vec![
            (1,0),
            (2,1),
            (2,4),
            (1,5),
            (1,6),
            (2,7),
            (2,10),
            (1,11),
            (1,12),
            (2,13),
            (2,16),
            (1,17),
            (0,18),
            (1,18),
            (1,19),
        ];

        for coord in coords {
            pixels.push(Coord::from(coord.0, coord.1));
        }

        Pattern { pixels, rows: 3, cols: 20 }
    }

    pub fn rotate(&self, rotation: Rotation) -> Pattern {
        let rows = self.rows - 1;
        let cols = self.cols - 1;
        match rotation {
            Rotation::None => {
                self.clone()
            },
            Rotation::Clock => {
                let pixels = self.pixels.iter().map(|c| {Coord::from(c.c, rows - c.r)}).collect();
                Pattern { pixels, rows: self.cols, cols: self.rows }
            },
            Rotation::Counter => {
                let pixels = self.pixels.iter().map(|c| {Coord::from(cols - c.c, c.r)}).collect();
                Pattern { pixels, rows: self.cols, cols: self.rows }
            },
            Rotation::About => {
                let pixels = self.pixels.iter().map(|c| {Coord::from(rows - c.r, cols - c.c)}).collect();
                Pattern { pixels, rows: self.rows, cols: self.cols }
            },
        }
    }

    pub fn mirror(&self, mirror: Mirror) -> Pattern {
        let rows = self.rows - 1;
        let cols = self.cols - 1;
        match mirror {
            Mirror::None => {
                self.clone()
            },
            Mirror::Horizontal => {
                let pixels = self.pixels.iter().map(|c| {Coord::from(rows - c.r, c.c)}).collect();
                Pattern { pixels, rows: self.rows, cols: self.cols }
            },
            Mirror::Vertical => {
                let pixels = self.pixels.iter().map(|c| {Coord::from(c.r, cols - c.c)}).collect();
                Pattern { pixels, rows: self.rows, cols: self.cols }
            },
        }
    }

    pub fn shift(&self, base: Coord) -> Pattern {
        let pixels = self.pixels.iter().map(|c| {Coord::from(c.r + base.r, c.c + base.c)}).collect();
        Pattern { pixels, rows: self.rows, cols: self.cols }
    }
}