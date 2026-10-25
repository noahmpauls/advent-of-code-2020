use std::collections::{HashSet,VecDeque};

type Deck = VecDeque<usize>;

pub struct Combat {
    player1: Deck,
    player2: Deck,
    memo: HashSet<(Deck, Deck)>
}

impl Combat {
    pub fn from_strings(player1: &str, player2: &str) -> Combat {
        Combat {
            player1: Self::deck_from_string(player1),
            player2: Self::deck_from_string(player2),
            memo: HashSet::new(),
        }
    }

    fn deck_from_string(deck: &str) -> VecDeque<usize> {
        deck.lines()
            .filter(|l| l.parse::<usize>().is_ok())
            .map(|l| l.parse::<usize>().unwrap())
            .collect()
    }

    fn from_decks(player1: &[usize], player2: &[usize]) -> Combat {
        Combat {
            player1: player1.iter().copied().collect(),
            player2: player2.iter().copied().collect(),
            memo: HashSet::new(),
        }
    }

    pub fn step(&mut self) -> Option<usize> {
        if self.player1.len() == 0 {
            return Some(2);
        } else if self.player2.len() == 0 {
            return Some(1);
        }
        let p1 = self.player1.pop_front().unwrap();
        let p2 = self.player2.pop_front().unwrap();
        if p1 > p2 {
            self.player1.push_back(p1);
            self.player1.push_back(p2);
        } else if p2 > p1 {
            self.player2.push_back(p2);
            self.player2.push_back(p1);
        }
        None
    }

    pub fn step_recursive(&mut self) -> Option<usize> {
        // check for repeated state
        let state = (self.player1.clone(), self.player2.clone());
        if self.memo.contains(&state) {
            return Some(1);
        }
        self.memo.insert(state);

        // check for player win
        if self.player1.len() == 0 {
            return Some(2);
        } else if self.player2.len() == 0 {
            return Some(1);
        }

        let p1 = self.player1.pop_front().unwrap();
        let p2 = self.player2.pop_front().unwrap();
        let winner = if p1 <= self.player1.len() && p2 <= self.player2.len() {
            // play recursive game
            let deck1 = &self.player1.make_contiguous()[0..p1];
            let deck2 = &self.player2.make_contiguous()[0..p2];
            let mut recurse_game = Combat::from_decks(deck1, deck2);
            loop {
                if let Some(winner) = recurse_game.step_recursive() {
                    break winner;
                }
            }
        } else {
            // otherwise compare cards
            if p1 > p2 {
                1
            } else {
                2
            }
        };

        if winner == 1 {
            self.player1.push_back(p1);
            self.player1.push_back(p2);
        } else {
            self.player2.push_back(p2);
            self.player2.push_back(p1);
        }
        None
    }

    pub fn deck(&self, player: usize) -> Option<Vec<usize>> {
        let deck = match player {
            1 => &self.player1,
            2 => &self.player2,
            _ => return None
        };

        Some(deck.iter().copied().collect())
    }
}