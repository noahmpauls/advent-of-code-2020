use regex::Regex;
use std::fs;
use std::collections::HashSet;

use monster_messages::{Rule, RuleSet};

fn main() {
    day_1();
    day_2();
}

fn day_1() {
    let filename = "19_monster_messages/input/input.txt";
    let input = fs::read_to_string(filename).unwrap();
    let mut ruleset = RuleSet::new();

    for line in input.lines() {
        let rule = Rule::from(line);
        if let Some(rule) = rule {
            ruleset.insert_rule(rule);
        }
    }

    let regex = Regex::new(&ruleset.as_regex_string()).unwrap();
    let mut match_count = 0;

    for line in input.lines() {
        if regex.is_match(line) {
            match_count += 1;
        }
    }

    println!("{}", match_count);
}

fn day_2() {
    let filename = "19_monster_messages/input/input.txt";
    let input = fs::read_to_string(filename).unwrap();
    let mut ruleset = RuleSet::new();

    let mut chars = String::new();
    for line in input.lines() {
        let rule = Rule::from(line);
        if let Some(rule) = rule {
            if let Rule::Char{ char, .. } = rule {
                chars.push(char);
            }
            ruleset.insert_rule(rule);
        }
    }

    let test_string_regex = Regex::new(&format!("[{}]+", chars)).unwrap();
    let mut test_strings = Vec::new();
    let mut max_match_length = 0;
    for line in input.lines() {
        if test_string_regex.is_match(line) {
            max_match_length = std::cmp::max(max_match_length, line.len());
            test_strings.push(line);
        }
    }

    let test_strings = test_strings;
    let mut matches = HashSet::new();

    let mut eight_repeats = 1;
    'eight: loop {
        ruleset.insert_rule(make_rule_8(eight_repeats));
        if ruleset.min_length() > max_match_length {
            break 'eight;
        }

        let mut eleven_repeats = 1;
        'eleven: loop {
            ruleset.insert_rule(make_rule_11(eleven_repeats));
            if ruleset.min_length() > max_match_length {
                eleven_repeats = 1;
                ruleset.insert_rule(make_rule_11(eleven_repeats));
                break 'eleven;
            }

            let regex = Regex::new(&ruleset.as_regex_string()).unwrap();
            for test in test_strings.iter() {
                if regex.is_match(test) {
                    matches.insert(test);
                }
            }
            eleven_repeats += 1;
        }
        eight_repeats += 1;
    }
    println!("{}", matches.len());
}

fn make_rule_8(repeats: usize) -> Rule {
    let rule = vec![42; repeats];
    Rule::SubRule{
        id: 8, 
        rule,
        or: None
    }
}

fn make_rule_11(repeats: usize) -> Rule {
    let mut rule = Vec::new();
    for num in [42, 31] {
        for _ in 0..repeats {
            rule.push(num);
        }
    }
    Rule::SubRule{
        id: 11,
        rule,
        or: None
    }
}
