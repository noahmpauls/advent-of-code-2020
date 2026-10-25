pub struct Cups {
    cups: Vec<usize>,
    current: usize,
    cursor: usize,
}

impl Cups {
    pub fn new(cups_list: &Vec<usize>) -> Cups {
        let mut cups = cups_list.clone();

        let mut cups_iter = cups_list.iter();
        let mut index = cups_iter.next();
        let mut next = cups_iter.next();

        while next.is_some() {
            cups[*index.unwrap()] = *next.unwrap();
            index = next;
            next = cups_iter.next();
        }

        cups[*cups_list.last().unwrap()] = *cups_list.first().unwrap();
        let current = *cups_list.first().unwrap();

        Cups { cups, current, cursor: 0, }
    }

    pub fn step(&mut self) {
        // do the transformation
        self.cursor = self.current;
        let taken_cups: Vec<usize> = (0..3).map(|_| self.next_cup()).collect();

        self.cups[self.current] = self.next_cup();

        let mut destination = self.destination_cup(self.current);
        while taken_cups.contains(&destination) {
            destination = self.destination_cup(destination);
        }

        let after_destination = self.cup_after(destination);
        self.cups[destination] = *taken_cups.first().unwrap();
        self.cups[*taken_cups.last().unwrap()] = after_destination;

        // make current next
        self.current = self.cup_after(self.current);
    }

    fn next_cup(&mut self) -> usize {
        self.cursor = self.cups[self.cursor];
        self.cursor
    }

    fn destination_cup(&self, cup: usize) -> usize {
        if cup == 0 {
            self.cups.len() - 1
        } else {
            cup - 1
        }
    }

    fn cup_after(&self, cup: usize) -> usize {
        self.cups[cup]
    }

    pub fn cups_from(&mut self, cup: usize, count: usize) -> Vec<usize>{
        self.cursor = cup;
        let mut result = Vec::new();

        for _ in 0..count {
            result.push(self.cursor);
            self.next_cup();
        }

        result
    }    
}